using HarmonyLib;
using Hazel;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using InnerNet;
using Reactor.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using TownOfUsEdited;
using TownOfUsEdited.Extensions;
using TownOfUsEdited.Roles;
using TownOfUsEdited.Roles.Modifiers;
using UnityEngine;
using Object = UnityEngine.Object;

namespace TownOfUsEdited.CrewmateRoles.PresidentMod
{
    [HarmonyPatch(typeof(MeetingHud))]
    public class RegisterExtraVotes
    {
        [HarmonyPatch(nameof(MeetingHud.Update))]
        public static void Postfix(MeetingHud __instance)
        {
            if (!PlayerControl.LocalPlayer.Is(RoleEnum.President)) return;
            if (PlayerControl.LocalPlayer.Data.IsDead) return;
            if (__instance.TimerText.text.Contains("Can Vote")) return;
            var role = Role.GetRole<President>(PlayerControl.LocalPlayer);
            __instance.TimerText.text = "Can Vote: " + role.VoteBank + " time(s)\n" + __instance.TimerText.text;
        }

        public static Dictionary<byte, int> CalculateAllVotes(MeetingHud __instance)
        {
            var dictionary = new Dictionary<byte, int>();
            for (var i = 0; i < __instance.playerStates.Length; i++)
            {
                var playerVoteArea = __instance.playerStates[i];
                var player = Utils.PlayerById(playerVoteArea.TargetPlayerId);
                if (!player.Is(RoleEnum.Prosecutor)) continue;
                var pros = Role.GetRole<Prosecutor>(player);
                if (pros.Player.Data.IsDead || pros.Player.Data.Disconnected) continue;
                if (!playerVoteArea.DidVote
                    || playerVoteArea.AmDead
                    || playerVoteArea.VotedFor == PlayerVoteArea.MissedVote
                    || playerVoteArea.VotedFor == PlayerVoteArea.DeadVote)
                {
                    pros.ProsecuteThisMeeting = false;
                    continue;
                }
                else if (pros.ProsecuteThisMeeting)
                {
                    if (dictionary.TryGetValue(playerVoteArea.VotedFor, out var num2))
                        dictionary[playerVoteArea.VotedFor] = num2 + 5;
                    else
                        dictionary[playerVoteArea.VotedFor] = 5;
                    return dictionary;
                }
            }

            for (var i = 0; i < __instance.playerStates.Length; i++)
            {
                var playerVoteArea = __instance.playerStates[i];
                if (!playerVoteArea.DidVote
                    || playerVoteArea.AmDead
                    || playerVoteArea.VotedFor == PlayerVoteArea.MissedVote
                    || playerVoteArea.VotedFor == PlayerVoteArea.DeadVote) continue;

                if (dictionary.TryGetValue(playerVoteArea.VotedFor, out var num))
                    dictionary[playerVoteArea.VotedFor] = num + 1;
                else
                    dictionary[playerVoteArea.VotedFor] = 1;
            }

            foreach (var role in Role.GetRoles(RoleEnum.President))
                foreach (var number in ((President)role).ExtraVotes)
                    if (dictionary.TryGetValue(number, out var num))
                        dictionary[number] = num + 1;
                    else
                        dictionary[number] = 1;

            dictionary.MaxPair(out var tie);

            if (tie)
                foreach (var player in __instance.playerStates)
                {
                    if (!player.DidVote
                        || player.AmDead
                        || player.VotedFor == PlayerVoteArea.MissedVote
                        || player.VotedFor == PlayerVoteArea.DeadVote) continue;

                    var modifiers = Modifier.GetModifiers(player);
                    if (modifiers == null || modifiers.Length == 0) continue;
                    if (modifiers.Any(x => x.ModifierType == ModifierEnum.Tiebreaker))
                    {
                        if (dictionary.TryGetValue(player.VotedFor, out var num))
                            dictionary[player.VotedFor] = num + 1;
                        else
                            dictionary[player.VotedFor] = 1;
                    }
                }

            return dictionary;
        }

        [HarmonyPatch(nameof(MeetingHud.Start))]
        public static void Prefix()
        {
            foreach (var role in Role.GetRoles(RoleEnum.President))
            {
                var president = (President)role;
                president.ExtraVotes.Clear();
                if (president.VoteBank < 0)
                    president.VoteBank = 0;

                if (president.VoteBank < CustomGameOptions.PresidentMaximumBank) president.VoteBank++;
                president.SelfVote = false;
                president.VotedOnce = false;
            }
        }

        [HarmonyPrefix]
        [HarmonyPatch(
            nameof(MeetingHud.HandleDisconnect),
            typeof(PlayerControl), typeof(DisconnectReasons)
        )]
        public static void Prefix(
            MeetingHud __instance, [HarmonyArgument(0)] PlayerControl player)
        {
            if (AmongUsClient.Instance.AmHost && MeetingHud.Instance)
            {
                foreach (var role in Role.GetRoles(RoleEnum.President))
                {
                    if (role is President president)
                    {
                        if (president.VotedOnce)
                        {
                            var votesRegained = president.ExtraVotes.RemoveAll(x => x == player.PlayerId);

                            if (president.Player == PlayerControl.LocalPlayer)
                                president.VoteBank += votesRegained;

                            var writer = AmongUsClient.Instance.StartRpcImmediately(
                                player.NetId,
                                (byte)CustomRPC.SetExtraVotes,
                                SendOption.Reliable,
                                -1);
                            writer.Write(president.Player.PlayerId);
                            writer.Write(president.VoteBank);
                            AmongUsClient.Instance.FinishRpcImmediately(writer);
                        }
                    }
                }
            }
        }


        [HarmonyPatch(typeof(MeetingHud), nameof(MeetingHud.Confirm))]
        public static class Confirm
        {
            public static bool Prefix(MeetingHud __instance)
            {
                if (!PlayerControl.LocalPlayer.Is(RoleEnum.President)) return true;
                if (__instance.state != MeetingHud.VoteStates.Voted) return true;
                __instance.state = MeetingHud.VoteStates.NotVoted;
                return true;
            }

            [HarmonyPriority(Priority.First)]
            public static void Postfix(MeetingHud __instance)
            {
                if (!PlayerControl.LocalPlayer.Is(RoleEnum.President)) return;
                var role = Role.GetRole<President>(PlayerControl.LocalPlayer);
                if (role.CanVote) __instance.SkipVoteButton.gameObject.SetActive(true);
            }
        }


        [HarmonyPatch(typeof(MeetingHud), nameof(MeetingHud.CastVote))]
        public static class CastVote
        {
            public static bool Prefix(MeetingHud __instance, [HarmonyArgument(0)] byte srcPlayerId,
                [HarmonyArgument(1)] byte suspectPlayerId)
            {
                var player = PlayerControl.AllPlayerControls.ToArray().FirstOrDefault(x => x.PlayerId == srcPlayerId);
                if (!player.Is(RoleEnum.President)) return true;

                var playerVoteArea = __instance.playerStates.ToArray().First(pv => pv.TargetPlayerId == srcPlayerId);

                if (playerVoteArea.AmDead)
                    return false;

                if (PlayerControl.LocalPlayer.PlayerId == srcPlayerId || AmongUsClient.Instance.NetworkMode != NetworkModes.LocalGame)
                {
                    SoundManager.Instance.PlaySound(__instance.VoteLockinSound, false, 1f);
                }

                var role = Role.GetRole<President>(player);
                if (playerVoteArea.DidVote)
                {
                    if (AmongUsClient.Instance.AmHost)
                    {
                        role.ExtraVotes.Add(suspectPlayerId);

                        var writer = AmongUsClient.Instance.StartRpcImmediately(
                            PlayerControl.LocalPlayer.NetId,
                            (byte)CustomRPC.SetExtraVotes,
                            SendOption.Reliable,
                            -1
                        );

                        writer.Write(player.PlayerId);
                        writer.WriteBytesAndSize(role.ExtraVotes.ToArray());
                        AmongUsClient.Instance.FinishRpcImmediately(writer);
                    }

                    return false;
                }
                else
                {
                    playerVoteArea.SetVote(suspectPlayerId);
                    playerVoteArea.Flag.enabled = true;
                    PlayerControl.LocalPlayer.RpcSendChatNote(srcPlayerId, ChatNoteTypes.DidVote);
                }
                __instance.Cast<InnerNetObject>().SetDirtyBit(1U);
                __instance.CheckForEndVoting();

                return false;
            }
        }

        [HarmonyPatch(typeof(MeetingHud), nameof(MeetingHud.VotingComplete))]
        public static class VotingComplete
        {
            public static void Postfix(MeetingHud __instance,
                [HarmonyArgument(0)] Il2CppStructArray<MeetingHud.VoterState> states,
                [HarmonyArgument(1)] NetworkedPlayerInfo exiled,
                [HarmonyArgument(2)] bool tie)
            {
                // __instance.exiledPlayer = __instance.wasTie ? null : __instance.exiledPlayer;
                var exiledString = exiled == null ? "null" : exiled.PlayerName;
                PluginSingleton<TownOfUsEdited>.Instance.Log.LogMessage($"Exiled PlayerName = {exiledString}");
                PluginSingleton<TownOfUsEdited>.Instance.Log.LogMessage($"Was a tie = {tie}");
            }
        }

        [HarmonyPatch(typeof(MeetingHud), nameof(MeetingHud.PopulateResults))]
        public static class PopulateResults
        {
            public static bool Prefix(MeetingHud __instance,
                [HarmonyArgument(0)] Il2CppStructArray<MeetingHud.VoterState> statess)
            {

                var allNums = new Dictionary<int, int>();


                __instance.TitleText.text = Object.FindObjectOfType<TranslationController>()
                    .GetString(StringNames.MeetingVotingResults, Array.Empty<Il2CppSystem.Object>());
                var amountOfSkippedVoters = 0;

                var isProsecuting = false;
                foreach (var pros in Role.GetRoles(RoleEnum.Prosecutor))
                {
                    var prosRole = (Prosecutor)pros;
                    if (pros.Player.Data.IsDead || pros.Player.Data.Disconnected) continue;
                    if (prosRole.ProsecuteThisMeeting)
                    {
                        isProsecuting = true;
                    }
                }
                for (var i = 0; i < __instance.playerStates.Length; i++)
                {
                    var playerVoteArea = __instance.playerStates[i];
                    playerVoteArea.ClearForResults();
                    allNums.Add(i, 0);

                    for (var stateIdx = 0; stateIdx < statess.Length; stateIdx++)
                    {
                        var voteState = statess[stateIdx];
                        var playerInfo = GameData.Instance.GetPlayerById(voteState.VoterId);
                        foreach (var pros in Role.GetRoles(RoleEnum.Prosecutor))
                        {
                            var prosRole = (Prosecutor)pros;
                            if (pros.Player.Data.IsDead || pros.Player.Data.Disconnected) continue;
                            if (prosRole.ProsecuteThisMeeting)
                            {
                                if (voteState.VoterId == prosRole.Player.PlayerId)
                                {
                                    if (playerInfo == null)
                                    {
                                        Debug.LogError(string.Format("Couldn't find player info for voter: {0}",
                                            voteState.VoterId));
                                        prosRole.Prosecuted = true;
                                    }
                                    else if (i == 0 && voteState.SkippedVote)
                                    {
                                    }
                                    else if (voteState.VotedForId == playerVoteArea.TargetPlayerId)
                                    {
                                    }
                                }
                            }
                        }

                        if (isProsecuting) continue;

                        if (playerInfo == null)
                        {
                            Debug.LogError(string.Format("Couldn't find player info for voter: {0}",
                                voteState.VoterId));
                        }
                        else if (i == 0 && voteState.SkippedVote)
                        {
                        }
                        else if (voteState.VotedForId == playerVoteArea.TargetPlayerId)
                        {
                        }
                    }
                }

                foreach (var role in Role.GetRoles(RoleEnum.President))
                {
                    var president = (President)role;
                    var playerInfo = GameData.Instance.GetPlayerById(role.Player.PlayerId);

                    if (isProsecuting) continue;

                    foreach (var extraVote in president.ExtraVotes)
                    {
                        if (extraVote == PlayerVoteArea.HasNotVoted ||
                            extraVote == PlayerVoteArea.MissedVote ||
                            extraVote == PlayerVoteArea.DeadVote)
                        {
                            continue;
                        }
                        if (extraVote == PlayerVoteArea.SkippedVote)
                        {

                            __instance.BloopAVoteIcon(playerInfo, amountOfSkippedVoters, __instance.SkippedVoting.transform);
                            amountOfSkippedVoters++;
                        }
                        else
                        {
                            for (var i = 0; i < __instance.playerStates.Length; i++)
                            {
                                var area = __instance.playerStates[i];
                                if (extraVote != area.TargetPlayerId) continue;
                                __instance.BloopAVoteIcon(playerInfo, allNums[i], area.transform);
                                allNums[i]++;
                            }
                        }
                    }
                }

                return true; //false blokuje vanilla glosy
            }
        }
    }
}