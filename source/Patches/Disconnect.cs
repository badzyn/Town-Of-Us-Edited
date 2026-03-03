using HarmonyLib;
using Reactor.Utilities.Extensions;
using System.Collections.Generic;
using System.Linq;
using TownOfUsEdited.CovenRoles.RitualistMod;
using TownOfUsEdited.CrewmateRoles.ImitatorMod;
using TownOfUsEdited.CrewmateRoles.SwapperMod;
using TownOfUsEdited.CrewmateRoles.VigilanteMod;
using TownOfUsEdited.ImpostorRoles.BlackmailerMod;
using TownOfUsEdited.ImpostorRoles.TraitorMod;
using TownOfUsEdited.Modifiers.AssassinMod;
using TownOfUsEdited.NeutralRoles.DoomsayerMod;
using TownOfUsEdited.NeutralRoles.SoulCollectorMod;
using TownOfUsEdited.Roles;
using TownOfUsEdited.Roles.Modifiers;
using UnityEngine;
using UnityEngine.UI;
using Assassin = TownOfUsEdited.Roles.Modifiers.Assassin;

namespace TownOfUsEdited.Patches
{
    [HarmonyPatch(typeof(GameData))]
    public class DisconnectHandler
    {
        [HarmonyPrefix]
        [HarmonyPatch(nameof(GameData.HandleDisconnect), typeof(PlayerControl), typeof(DisconnectReasons))]
        public static void Prefix([HarmonyArgument(0)] PlayerControl player)
        {
            if (AmongUsClient.Instance.AmHost)
            {
                if (player == SetTraitor.WillBeTraitor)
                {
                    var toChooseFrom = PlayerControl.AllPlayerControls.ToArray().Where(x => x.Is(Faction.Crewmates) &&
                        !x.Is(ModifierEnum.Lover) && !x.Data.IsDead && !x.Data.Disconnected && !x.Is(RoleEnum.President) && !x.IsExeTarget()).ToList();
                    if (toChooseFrom.Count == 0) return;
                    var rand = Random.RandomRangeInt(0, toChooseFrom.Count);
                    var pc = toChooseFrom[rand];

                    SetTraitor.WillBeTraitor = pc;

                    Utils.Rpc(CustomRPC.SetTraitor, pc.PlayerId);
                }
                PlayerControl_Die.CheckEnd();
            }
            if (RpcHandling.Upped.ContainsKey(player))
            {
                RpcHandling.Upped.Remove(player);
            }
            if (player.Is(RoleEnum.Cleric))
            {
                var cler = Role.GetRole<Cleric>(player);
                if (cler.Barriered != null) cler.UnBarrier();
            }
            if (player.Is(RoleEnum.GuardianAngel))
            {
                var ga = Role.GetRole<GuardianAngel>(player);
                if (ga.Protecting) ga.UnProtect();
            }
            if (player.Is(RoleEnum.Hypnotist))
            {
                var hypno = Role.GetRole<Hypnotist>(player);
                if (hypno.HysteriaActive && hypno.HypnotisedPlayers.Contains(PlayerControl.LocalPlayer.PlayerId))
                {
                    var removeHypno = true;
                    foreach (var role in Role.GetRoles(RoleEnum.Hypnotist))
                    {
                        if (role.Player == hypno.Player) continue;
                        var hypnoRole = (Hypnotist)role;
                        if (hypnoRole.HysteriaActive && hypno.HypnotisedPlayers.Contains(PlayerControl.LocalPlayer.PlayerId))
                        {
                            removeHypno = false;
                            break;
                        }
                    }
                    if (removeHypno) hypno.UnHysteria();
                }
                hypno.HysteriaActive = false;
            }
            if (player.Is(RoleEnum.SoulCollector))
            {
                var sc = Role.GetRole<SoulCollector>(player);
                SoulExtensions.ClearSouls(sc.Souls);
            }
            if (player.Is(ModifierEnum.Lover))
            {
                var lover = Modifier.GetModifier<Lover>(player);
                Modifier.ModifierDictionary.Remove(lover.OtherLover.Player.PlayerId);
            }
            if (player.IsManipulated())
            {
                var manipulator = PlayerControl.AllPlayerControls.ToArray().FirstOrDefault(x => x.Is(RoleEnum.Manipulator) && Role.GetRole<Manipulator>(x).ManipulatedPlayer == player);
                var manipRole = Role.GetRole<Manipulator>(manipulator);
                manipRole.StopManipulation();
                Utils.Rpc(CustomRPC.SetManipulateOff, manipulator.PlayerId);
            }
            if (player.IsWatched())
            {
                var lookout = PlayerControl.AllPlayerControls.ToArray().FirstOrDefault(x => x.Is(RoleEnum.Lookout) && Role.GetRole<Lookout>(x).WatchedPlayer == player);
                var lookoutRole = Role.GetRole<Lookout>(lookout);
                lookoutRole.StopWatching();
                if (CustomGameOptions.WatchedKnows) Utils.Rpc(CustomRPC.StopWatch, lookout.PlayerId);
            }
            if (MeetingHud.Instance)
            {
                PlayerVoteArea voteArea = MeetingHud.Instance.playerStates.First(x => x.TargetPlayerId == player.PlayerId);

                if (!player.Data.IsDead)
                {
                    if (voteArea == null) return;
                    if (voteArea.DidVote) voteArea.UnsetVote();
                    voteArea.AmDead = true;
                    voteArea.Overlay.gameObject.SetActive(true);
                    voteArea.Overlay.color = Color.white;
                    voteArea.XMark.gameObject.SetActive(true);
                    voteArea.XMark.transform.localScale = Vector3.one;
                }

                var blackmailers = Role.AllRoles.Where(x => x.RoleType == RoleEnum.Blackmailer && x.Player != null).Cast<Blackmailer>();
                var blackmailed = new List<PlayerControl>();
                foreach (var role in blackmailers)
                {
                    if (role.Blackmailed != null && !blackmailed.Contains(role.Blackmailed))
                    {
                        blackmailed.Add(role.Blackmailed);
                        if (voteArea.TargetPlayerId == role.Blackmailed.PlayerId)
                        {
                            if (BlackmailMeetingUpdate.PrevXMark != null && BlackmailMeetingUpdate.PrevOverlay != null)
                            {
                                voteArea.XMark.sprite = BlackmailMeetingUpdate.PrevXMark;
                                voteArea.Overlay.sprite = BlackmailMeetingUpdate.PrevOverlay;
                                voteArea.XMark.transform.localPosition = new Vector3(
                                    voteArea.XMark.transform.localPosition.x - BlackmailMeetingUpdate.LetterXOffset,
                                    voteArea.XMark.transform.localPosition.y - BlackmailMeetingUpdate.LetterYOffset,
                                    voteArea.XMark.transform.localPosition.z);
                            }
                        }
                    }
                }

                var jailors = Role.AllRoles.Where(x => x.RoleType == RoleEnum.Jailor && x.Player != null).Cast<Jailor>();
                foreach (var role in jailors)
                {
                    if (role.JailedPlayer == player || role.Player == player)
                    {
                        role.JailCell.Destroy();
                    }
                }

                if (PlayerControl.LocalPlayer.Is(RoleEnum.Deputy) && !PlayerControl.LocalPlayer.Data.IsDead)
                {
                    var dep = Role.GetRole<Deputy>(PlayerControl.LocalPlayer);
                    if (dep.Buttons.Count > 0 && dep.Buttons[voteArea.TargetPlayerId] != null)
                    {
                        dep.Buttons[voteArea.TargetPlayerId].SetActive(false);
                        dep.Buttons[voteArea.TargetPlayerId].GetComponent<PassiveButton>().OnClick = new Button.ButtonClickedEvent();
                    }
                }

                if (PlayerControl.LocalPlayer.Is(RoleEnum.Vigilante) && !PlayerControl.LocalPlayer.Data.IsDead)
                {
                    var vigi = Role.GetRole<Vigilante>(PlayerControl.LocalPlayer);
                    ShowHideButtonsVigi.HideTarget(vigi, voteArea.TargetPlayerId);
                }

                if (PlayerControl.LocalPlayer.Is(AbilityEnum.Assassin) && !PlayerControl.LocalPlayer.Data.IsDead)
                {
                    var assassin = Ability.GetAbility<Assassin>(PlayerControl.LocalPlayer);
                    ShowHideButtons.HideTarget(assassin, voteArea.TargetPlayerId);
                }

                if (PlayerControl.LocalPlayer.Is(RoleEnum.Ritualist) && !PlayerControl.LocalPlayer.Data.IsDead)
                {
                    var rit = Role.GetRole<Ritualist>(PlayerControl.LocalPlayer);
                    ShowHideButtonsRitualist.HideTarget(rit, voteArea.TargetPlayerId);
                }

                if (PlayerControl.LocalPlayer.Is(RoleEnum.Doomsayer) && !PlayerControl.LocalPlayer.Data.IsDead)
                {
                    var doom = Role.GetRole<Doomsayer>(PlayerControl.LocalPlayer);
                    ShowHideButtonsDoom.HideTarget(doom, voteArea.TargetPlayerId);
                }

                if (PlayerControl.LocalPlayer.Is(RoleEnum.Swapper) && !PlayerControl.LocalPlayer.Data.IsDead)
                {
                    var swapper = Role.GetRole<Swapper>(PlayerControl.LocalPlayer);
                    var index = int.MaxValue;
                    for (var i = 0; i < swapper.ListOfActives.Count; i++)
                    {
                        if (swapper.ListOfActives[i].Item1 == voteArea.TargetPlayerId)
                        {
                            index = i;
                            break;
                        }
                    }
                    if (index != int.MaxValue)
                    {
                        var button = swapper.Buttons[index];
                        if (button != null)
                        {
                            if (button.GetComponent<SpriteRenderer>().sprite == TownOfUsEdited.SwapperSwitch)
                            {
                                swapper.ListOfActives[index] = (swapper.ListOfActives[index].Item1, false);
                                if (SwapVotes.Swap1 == voteArea) SwapVotes.Swap1 = null;
                                if (SwapVotes.Swap2 == voteArea) SwapVotes.Swap2 = null;
                                Utils.Rpc(CustomRPC.SetSwaps, sbyte.MaxValue, sbyte.MaxValue);
                            }
                            button.SetActive(false);
                            button.GetComponent<PassiveButton>().OnClick = new Button.ButtonClickedEvent();
                            swapper.Buttons[index] = null;
                        }
                    }
                }

                foreach (var playerVoteArea in MeetingHud.Instance.playerStates)
                {
                    if (playerVoteArea.VotedFor != player.PlayerId) continue;
                    playerVoteArea.UnsetVote();
                    var voteAreaPlayer = Utils.PlayerById(playerVoteArea.TargetPlayerId);
                    if (voteAreaPlayer.Is(RoleEnum.Prosecutor))
                    {
                        var pros = Role.GetRole<Prosecutor>(voteAreaPlayer);
                        pros.ProsecuteThisMeeting = false;
                    }
                    if (!voteAreaPlayer.AmOwner) continue;
                    MeetingHud.Instance.ClearVote();
                }

                if (PlayerControl.LocalPlayer.Is(RoleEnum.Deputy) && !PlayerControl.LocalPlayer.Data.IsDead)
                {
                    var dep = Role.GetRole<Deputy>(PlayerControl.LocalPlayer);
                    if (dep.Buttons.Count > 0 && dep.Buttons[voteArea.TargetPlayerId] != null)
                    {
                        dep.Buttons[voteArea.TargetPlayerId].SetActive(false);
                        dep.Buttons[voteArea.TargetPlayerId].GetComponent<PassiveButton>().OnClick = new Button.ButtonClickedEvent();
                    }
                }

                if (PlayerControl.LocalPlayer.Is(RoleEnum.Imitator) && !PlayerControl.LocalPlayer.Data.IsDead)
                {
                    var imitatorRole = Role.GetRole<Imitator>(PlayerControl.LocalPlayer);
                    var index = int.MaxValue;
                    for (var i = 0; i < imitatorRole.ListOfActives.Count; i++)
                    {
                        if (imitatorRole.ListOfActives[i].Item1 == voteArea.TargetPlayerId)
                        {
                            index = i;
                            break;
                        }
                    }
                    if (index != int.MaxValue)
                    {
                        var button = imitatorRole.Buttons[index];
                        if (button != null)
                        {
                            if (button.GetComponent<SpriteRenderer>().sprite == TownOfUsEdited.ImitateSelectSprite)
                            {
                                imitatorRole.ListOfActives[index] = (imitatorRole.ListOfActives[index].Item1, false);
                                if (SetImitate.Imitate == voteArea) SetImitate.Imitate = null;
                            }
                            button.SetActive(false);
                            button.GetComponent<PassiveButton>().OnClick = new Button.ButtonClickedEvent();
                            imitatorRole.Buttons[index] = null;
                        }
                    }
                }
            }
        }
    }
}