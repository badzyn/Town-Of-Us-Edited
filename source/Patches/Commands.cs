using AmongUs.GameOptions;
using HarmonyLib;
using Reactor.Utilities;
using Reactor.Utilities.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using TownOfUsEdited.CrewmateRoles.MedicMod;
using TownOfUsEdited.Extensions;
using TownOfUsEdited.Roles;
using TownOfUsEdited.Roles.Modifiers;
using UnityEngine;

namespace TownOfUsEdited.Patches
{
    public class Commands
    {
        public static bool host = false;
        public static bool error = false;
        public static bool system = false;
        public static bool noaccess = false;

        [HarmonyPatch(typeof(ChatController), nameof(ChatController.AddChat))]
        public class SpecialChat
        {
            public static void ForceAddChat(PlayerControl srcPlayer, string chatText, string Id)
            {
                var Instance = HudManager.Instance.Chat;
                ChatBubble pooledBubble = Instance.GetPooledBubble();
                try
                {
                    pooledBubble.transform.SetParent(Instance.scroller.Inner);
                    pooledBubble.transform.localScale = Vector3.one;
                    bool flag = srcPlayer == PlayerControl.LocalPlayer;
                    if (flag)
                    {
                        pooledBubble.SetRight();
                    }
                    else
                    {
                        pooledBubble.SetLeft();
                    }
                    var data2 = srcPlayer.Data;
                    pooledBubble.name = Id;
                    pooledBubble.SetCosmetics(data2);
                    Instance.SetChatBubbleName(pooledBubble, data2, false, false, PlayerNameColor.Get(data2), null);
                    pooledBubble.SetText(chatText);
                    pooledBubble.AlignChildren();
                    Instance.AlignAllBubbles();
                    if (!Instance.IsOpenOrOpening && Instance.notificationRoutine == null)
                    {
                        Instance.notificationRoutine = Instance.StartCoroutine(Instance.BounceDot());
                    }
                    if (!flag && !Instance.IsOpenOrOpening)
                    {
                        SoundManager.Instance.PlaySound(Instance.messageSound, false, 1f, null).pitch = 0.5f + (float)srcPlayer.PlayerId / 15f;
                        Instance.chatNotification.SetUp(srcPlayer, chatText);
                    }
                }
                catch (Exception message)
                {
                    PluginSingleton<TownOfUsEdited>.Instance.Log.LogError($"Error forcing send message: {message.ToString()}");
                    Instance.chatBubblePool.Reclaim(pooledBubble);
                }
            }
            public static bool CheckRole(string role)
            {
                return role == "Chameleon" || role == "Altruist" || role == "Amnesiac" || role == "Arsonist" ||
                (role == "Assassin" && CustomGameOptions.AssassinImpostorRole) || role == "Astral" ||
                role == "Attacker" || role == "Aurial" || role == "Avenger" || role == "Blackmailer" ||
                role == "Bodyguard" || role == "Bomber" || role == "BountyHunter" || role == "Captain" ||
                role == "Conjurer" || role == "Converter" || role == "Coven" || role == "CovenLeader" ||
                role == "Crewmate" || role == "Crusader" || role == "Deputy" || role == "Detective" ||
                role == "Doctor" || role == "Doomsayer" || role == "Doppelganger" || role == "Engineer" ||
                role == "Escapist" || role == "Executioner" || role == "Fighter" || role == "Glitch" ||
                role == "Grenadier" || role == "GuardianAngel" || role == "Impostor" ||
                role == "HexMaster" || role == "Hunter" || role == "Hypnotist" || role == "Imitator"
                || role == "Infectious" || role == "Informant" || role == "Investigator" || role == "Jailor"
                || role == "Janitor" || role == "Jester" || role == "Juggernaut" || role == "Knight"
                || role == "Lookout" || role == "Mafioso" || role == "Manipulator" || role == "Mayor"
                || role == "Medic" || role == "Medium" || role == "Miner" || role == "Morphling"
                || role == "Mutant" || role == "Mystic" || role == "Noclip" || role == "Oracle"
                || role == "Paranoïac" || role == "Plaguebearer" || role == "Poisoner" || role == "Politician"
                || role == "PotionMaster" || role == "Prosecutor" || role == "Reviver" || role == "Ritualist"
                || role == "Seer" || role == "SerialKiller" || role == "Sheriff" || role == "Shifter"
                || role == "Shooter" || role == "Snitch" || role == "SoulCollector" || role == "Spiritualist"
                || role == "Spy" || role == "Survivor" || role == "Swapper" || role == "Swooper"
                || role == "TimeLord" || role == "Tracker" || role == "Transporter" || role == "Trapper"
                || role == "Troll" || role == "Undertaker" || role == "Vampire" || role == "Vigilante"
                || role == "Vulture" || role == "Warden" || role == "Warlock" || role == "Watcher"
                || role == "Werewolf" || role == "Witch" || role == "Veteran" || role == "VoodooMaster";
            }
            public static bool Prefix(ChatController __instance, [HarmonyArgument(0)] PlayerControl sourcePlayer, ref string chatText)
            {
                if (__instance != HudManager.Instance.Chat)
                    return true;

                if (chatText.ToLower().StartsWith("/help"))
                {
                    if (sourcePlayer.IsDev() && sourcePlayer.PlayerId == PlayerControl.LocalPlayer.PlayerId)
                    {
                        chatText = $"<size=75%>" + $"{Color.green.ToTextColor()}<b>Everyone</b></color>\n\n" +
                            $"/roles - List active roles\n" +
                            $"/modifier - List active modifiers\n" +
                            $"/note [your note] - Write your own note\n" +
                            $"/seenote - Your saved notes\n" +
                            $"/r [role name] - Displays the description of any role\n" +
                            $"/m [modifier name] - Display the description of any modifier\n" +
                            $"/infoup - See infos about /up command\n" +
                            $"/up - Choose any role in the game (if enabled)\n" +
                            $"/allup - See all currently chosen roles\n" +
                            $"/death - Shows your death reason\n" +
                            $"/shrug - Adds \"{@"¯\_(ツ)_/¯"}\" to your message\n\n" +
                            $"{Palette.ImpostorRed.ToTextColor()}<b>Host Commands</b></color>\n\n" +
                            $"Shift + G + ENTER - Force the game to end\n" +
                            $"/msg [message] - Send a message as host\n" +
                            $"/id - See players ids\n" +
                            $"/kick [id] - Kick a player by its id\n" +
                            $"/ban [id] - Ban a player by its id\n" +
                            $"/limit [number] - Set a player limit in the lobby" +
                            $"</size>";
                        system = true;
                    }
                    else if (sourcePlayer.IsTester() && sourcePlayer.PlayerId == PlayerControl.LocalPlayer.PlayerId)
                    {
                        chatText = $"<size=75%>" + $"{Color.green.ToTextColor()}<b>Everyone</b></color>\n\n" +
                            $"/roles - List active roles\n" +
                            $"/modifier - List active modifiers\n" +
                            $"/note [your note] - Write your own note\n" +
                            $"/seenote - Your saved notes\n" +
                            $"/r [role name] - Displays the description of any role\n" +
                            $"/m [modifier name] - Display the description of any modifier\n" +
                            $"/infoup - See infos about /up command\n" +
                            $"/up - Choose any role in the game (if enabled)\n" +
                            $"/allup - See all currently chosen roles\n" +
                            $"/death - Shows your death reason\n" +
                            $"/shrug - Adds \"{@"¯\_(ツ)_/¯"}\" to your message\n\n" +
                            $"{Palette.ImpostorRed.ToTextColor()}<b>Host Commands</b></color>\n\n" +
                            $"Shift + G + ENTER - Force the game to end\n" +
                            $"/msg [message] - Send a message as host\n" +
                            $"/id - See players ids\n" +
                            $"/kick [id] - Kick a player by its id\n" +
                            $"/ban [id] - Ban a player by its id\n" +
                            $"/limit [number] - Set a player limit in the lobby" +
                            $"</size>";
                        system = true;
                    }
                    else if (sourcePlayer.IsArtist() && sourcePlayer.PlayerId == PlayerControl.LocalPlayer.PlayerId)
                    {
                        chatText = $"<size=75%>" + $"{Color.green.ToTextColor()}<b>Everyone</b></color>\n\n" +
                            $"/roles - List active roles\n" +
                            $"/modifier - List active modifiers\n" +
                            $"/note [your note] - Write your own note\n" +
                            $"/seenote - Your saved notes\n" +
                            $"/r [role name] - Displays the description of any role\n" +
                            $"/m [modifier name] - Display the description of any modifier\n" +
                            $"/infoup - See infos about /up command\n" +
                            $"/up - Choose any role in the game (if enabled)\n" +
                            $"/allup - See all currently chosen roles\n" +
                            $"/death - Shows your death reason\n" +
                            $"/shrug - Adds \"{@"¯\_(ツ)_/¯"}\" to your message\n\n" +
                            $"{Palette.ImpostorRed.ToTextColor()}<b>Host Commands</b></color>\n\n" +
                            $"Shift + G + ENTER - Force the game to end\n" +
                            $"/msg [message] - Send a message as host\n" +
                            $"/id - See players ids\n" +
                            $"/kick [id] - Kick a player by its id\n" +
                            $"/ban [id] - Ban a player by its id\n" +
                            $"/limit [number] - Set a player limit in the lobby" +
                            $"</size>";
                        system = true;
                    }
                    else if (sourcePlayer.IsVip() && sourcePlayer.PlayerId == PlayerControl.LocalPlayer.PlayerId)
                    {
                        chatText = $"<size=75%>" + $"{Color.green.ToTextColor()}<b>Everyone</b></color>\n\n" +
                            $"/roles - List active roles\n" +
                            $"/modifier - List active modifiers\n" +
                            $"/note [your note] - Write your own note\n" +
                            $"/seenote - Your saved notes\n" +
                            $"/r [role name] - Displays the description of any role\n" +
                            $"/m [modifier name] - Display the description of any modifier\n" +
                            $"/infoup - See infos about /up command\n" +
                            $"/up - Choose any role in the game (if enabled)\n" +
                            $"/allup - See all currently chosen roles\n" +
                            $"/death - Shows your death reason\n" +
                            $"/shrug - Adds \"{@"¯\_(ツ)_/¯"}\" to your message\n\n" +
                            $"{Palette.ImpostorRed.ToTextColor()}<b>Host Commands</b></color>\n\n" +
                            $"Shift + G + ENTER - Force the game to end\n" +
                            $"/msg [message] - Send a message as host\n" +
                            $"/id - See players ids\n" +
                            $"/kick [id] - Kick a player by its id\n" +
                            $"/ban [id] - Ban a player by its id\n" +
                            $"/limit [number] - Set a player limit in the lobby" +
                            $"</size>";
                        system = true;
                    }
                    else if (sourcePlayer.PlayerId == PlayerControl.LocalPlayer.PlayerId)
                    {
                        chatText = $"<size=75%>" + $"{Color.green.ToTextColor()}<b>Everyone</b></color>\n\n" +
                            $"/roles - List active roles\n" +
                            $"/modifier - List active modifiers\n" +
                            $"/note [your note] - Write your own note\n" +
                            $"/seenote - Your saved notes\n" +
                            $"/r [role name] - Displays the description of any role\n" +
                            $"/m [modifier name] - Display the description of any modifier\n" +
                            $"/infoup - See infos about /up command\n" +
                            $"/up - Choose any role in the game (if enabled)\n" +
                            $"/allup - See all currently chosen roles\n" +
                            $"/death - Shows your death reason\n" +
                            $"/shrug - Adds \"{@"¯\_(ツ)_/¯"}\" to your message\n\n" +
                            $"{Palette.ImpostorRed.ToTextColor()}<b>Host Commands</b></color>\n\n" +
                            $"Shift + G + ENTER - Force the game to end\n" +
                            $"/msg [message] - Send a message as host\n" +
                            $"/id - See players ids\n" +
                            $"/kick [id] - Kick a player by its id\n" +
                            $"/ban [id] - Ban a player by its id\n" +
                            $"/limit [number] - Set a player limit in the lobby" +
                            $"</size>";
                        system = true;
                    }
                    return sourcePlayer.PlayerId == PlayerControl.LocalPlayer.PlayerId;
                }

                if (chatText.ToLower().StartsWith("/secret") && sourcePlayer.PlayerId == PlayerControl.LocalPlayer.PlayerId) // Real secret fr fr
                {
                    Application.OpenURL("https://www.youtube.com/watch?v=dQw4w9WgXcQ&ab_channel=RickAstley");
                    return false;
                }

                if (chatText.ToLower().StartsWith("/msg "))
                {
                    if (GameData.Instance.GetHost() == sourcePlayer.Data)
                    {
                        var message = chatText[5..];
                        host = true;
                        ForceAddChat(sourcePlayer, message, "HostBubble");
                        return false;
                    }
                    else if (sourcePlayer.PlayerId == PlayerControl.LocalPlayer.PlayerId)
                    {
                        chatText = "You don't have access to this command!";
                        noaccess = true;
                    }
                    return sourcePlayer.PlayerId == PlayerControl.LocalPlayer.PlayerId;
                }

                if (chatText.ToLower().StartsWith("/allup"))
                {
                    if (sourcePlayer.PlayerId == PlayerControl.LocalPlayer.PlayerId)
                    {
                        string message = "All currently chosen roles:\n";
                        foreach (var playerRole in RpcHandling.Upped.Values.ToArray().OrderBy(x => Guid.NewGuid()))
                        {
                            message += $"{playerRole}\n";
                        }
                        message.Remove(message.Length - 1, 1);
                        if (!RpcHandling.Upped.Any(x => x.Key == PlayerControl.AllPlayerControls.ToArray().Any()))
                        {
                            message = "No roles have been selected yet.";
                        }
                        chatText = message;
                        system = true;
                    }
                    return sourcePlayer.PlayerId == PlayerControl.LocalPlayer.PlayerId;
                }

                if (chatText.ToLower().StartsWith("/infoup"))
                {
                    if (sourcePlayer.PlayerId == PlayerControl.LocalPlayer.PlayerId)
                    {
                        chatText = "<b><size=3>UP</size></b>\n\nThe command \"/up\" allows you to choose any role in the game.\nThe host can freely enable / disable this command whenever they want using settings.\nBut, this command only works in Classic gamemodes. It also won't apply if the selected role isn't turned on by the host.\nAll players can see which roles have been chosen during the game using \"/allup\".\nTo use this command, you need to make sure to type the name of the role correctly and without space, the role must have a maj (ex: SerialKiller, Sheriff...).";
                        system = true;
                    }
                    return sourcePlayer.PlayerId == PlayerControl.LocalPlayer.PlayerId;
                }

                if (chatText.ToLower().StartsWith("/shrug"))
                {
                    if (chatText.Length > 7)
                    {
                        chatText = chatText[7..] + @" ¯\_(ツ)_/¯";
                    }
                    else
                    {
                        chatText = @"¯\_(ツ)_/¯";
                    }
                    return true;
                }

                if (chatText.ToLower().StartsWith("/up "))
                {
                    if (CustomGameOptions.AllowUp && sourcePlayer.PlayerId == PlayerControl.LocalPlayer.PlayerId)
                    {
                        if (!LobbyBehaviour.Instance)
                        {
                            chatText = "You can only use this command in lobby!";
                            error = true;
                        }
                        else if (CustomGameOptions.GameMode != GameMode.Classic)
                        {
                            chatText = "This command can only be used on Classic gamemodes.";
                            error = true;
                        }
                        else if (CheckRole(chatText[4..]))
                        {
                            if (!RpcHandling.Upped.ContainsValue(chatText[4..]))
                            {
                                if (!RpcHandling.Upped.ContainsKey(sourcePlayer))
                                {
                                    RpcHandling.Upped.Add(sourcePlayer, chatText[4..]);
                                    Utils.Rpc(CustomRPC.AddUp, sourcePlayer.PlayerId, chatText[4..]);
                                }
                                else
                                {
                                    RpcHandling.Upped.Remove(sourcePlayer);
                                    RpcHandling.Upped.Add(sourcePlayer, chatText[4..]);
                                    Utils.Rpc(CustomRPC.AddUp, sourcePlayer.PlayerId, chatText[4..]);
                                }
                                chatText = $"You have chosen {chatText[4..]} role for the next game. Type \"Cancel\" to cancel.";
                                system = true;
                            }
                            else
                            {
                                chatText = "Sorry, this role has already been taken. Please try another role.";
                                error = true;
                            }
                        }
                        else if (chatText[4..] == "Cancel")
                        {
                            if (!RpcHandling.Upped.ContainsKey(sourcePlayer))
                            {
                                chatText = "You don't have any selected role for the next game.";
                                error = true;
                            }
                            else
                            {
                                RpcHandling.Upped.Remove(sourcePlayer);
                                Utils.Rpc(CustomRPC.AddUp, sourcePlayer.PlayerId, "Cancel");
                                chatText = "Current selected role removed.";
                                system = true;
                            }
                        }
                        else
                        {
                            chatText = "Make sure you typed the role correctly.";
                            error = true;
                        }
                    }
                    else if (sourcePlayer.PlayerId == PlayerControl.LocalPlayer.PlayerId)
                    {
                        chatText = "This command was disabled by the host.";
                        noaccess = true;
                    }
                    return sourcePlayer.PlayerId == PlayerControl.LocalPlayer.PlayerId;
                }

                if (chatText.ToLower().StartsWith("/r "))
                {
                    if (sourcePlayer.PlayerId == PlayerControl.LocalPlayer.PlayerId)
                    {
                        chatText = UpdateRoleInfo.GetRoleInfos(chatText[3..]);
                        system = true;
                    }
                    return sourcePlayer.PlayerId == PlayerControl.LocalPlayer.PlayerId;
                }

                if (chatText.ToLower().StartsWith("/m "))
                {
                    if (sourcePlayer.PlayerId == PlayerControl.LocalPlayer.PlayerId)
                    {
                        chatText = UpdateRoleInfo.GetModifiersInfos(chatText[3..]);
                        system = true;
                    }
                    return sourcePlayer.PlayerId == PlayerControl.LocalPlayer.PlayerId;
                }

                if (chatText.ToLower().StartsWith("/id"))
                {
                    if (sourcePlayer.PlayerId == PlayerControl.LocalPlayer.PlayerId)
                    {
                        string message = "All players ids:\n";
                        foreach (var player in PlayerControl.AllPlayerControls.ToArray().OrderBy(x => x.PlayerId))
                        {
                            message += $"{player.GetDefaultOutfit().PlayerName}: {player.PlayerId}\n";
                        }
                        message.Remove(message.Length - 1, 1);
                        chatText = message;
                        system = true;
                    }
                    return sourcePlayer.PlayerId == PlayerControl.LocalPlayer.PlayerId;
                }

                if (chatText.ToLower().StartsWith("/kick "))
                {
                    if (GameData.Instance.GetHost() == sourcePlayer.Data && sourcePlayer.PlayerId == PlayerControl.LocalPlayer.PlayerId)
                    {
                        if (chatText[6..].IsNullOrWhiteSpace())
                        {
                            chatText = "You must specify a player id as parameter, use /id to get the list of all players ids.";
                            error = true;
                        }
                        if (Convert.ToByte(chatText[6..]) == PlayerControl.LocalPlayer.PlayerId)
                        {
                            chatText = "You can't kick yourself!";
                            error = true;
                        }
                        else
                        {
                            var id = Convert.ToByte(chatText[6..]);
                            var playerWithId = PlayerControl.AllPlayerControls.ToArray().Where(x => x.PlayerId == id).ToList();
                            if (!playerWithId.Any())
                            {
                                chatText = "You must give a valid player id, use /id to get the list of all players ids.";
                                error = true;
                            }
                            else
                            {
                                foreach (var player in playerWithId)
                                {
                                    AmongUsClient.Instance.KickPlayer(player.OwnerId, false);
                                }
                                chatText = "The player was kicked successfully.";
                                system = true;
                            }
                        }
                    }
                    else if (sourcePlayer.PlayerId == PlayerControl.LocalPlayer.PlayerId)
                    {
                        chatText = "You don't have access to this command!";
                        noaccess = true;
                    }
                    return sourcePlayer.PlayerId == PlayerControl.LocalPlayer.PlayerId;
                }

                if (chatText.ToLower().StartsWith("/ban "))
                {
                    if (GameData.Instance.GetHost() == sourcePlayer.Data && sourcePlayer.PlayerId == PlayerControl.LocalPlayer.PlayerId)
                    {
                        if (chatText[5..].IsNullOrWhiteSpace())
                        {
                            chatText = "You must specify a player id as parameter, use /id to get the list of all players ids.";
                            error = true;
                        }
                        if (Convert.ToByte(chatText[5..]) == PlayerControl.LocalPlayer.PlayerId)
                        {
                            chatText = "You can't ban yourself!";
                            error = true;
                        }
                        else
                        {
                            var id = Convert.ToByte(chatText[5..]);
                            var playerWithId = PlayerControl.AllPlayerControls.ToArray().Where(x => x.PlayerId == id).ToList();
                            if (!playerWithId.Any())
                            {
                                chatText = "You must give a valid player id, use /id to get the list of all players ids.";
                                error = true;
                            }
                            else
                            {
                                foreach (var player in playerWithId)
                                {
                                    AmongUsClient.Instance.KickPlayer(player.OwnerId, true);
                                }
                                chatText = "The player was banned successfully.";
                                system = true;
                            }
                        }
                    }
                    else if (sourcePlayer.PlayerId == PlayerControl.LocalPlayer.PlayerId)
                    {
                        chatText = "You don't have access to this command!";
                        noaccess = true;
                    }
                    return sourcePlayer.PlayerId == PlayerControl.LocalPlayer.PlayerId;
                }

                if (chatText.ToLower().StartsWith("/death"))
                {
                    if ((AmongUsClient.Instance.GameState == InnerNet.InnerNetClient.GameStates.Started || AmongUsClient.Instance.NetworkMode == NetworkModes.FreePlay) && sourcePlayer.PlayerId == PlayerControl.LocalPlayer.PlayerId)
                    {
                        var playerRole = Role.GetRole(PlayerControl.LocalPlayer);
                        if (!sourcePlayer.Data.IsDead)
                        {
                            if (sourcePlayer.PlayerId == PlayerControl.LocalPlayer.PlayerId)
                            {
                                chatText = "You can only use this command after death!";
                                error = true;
                            }
                        }
                        else if (playerRole.DeathReason == DeathReasons.Spectator && sourcePlayer.PlayerId == PlayerControl.LocalPlayer.PlayerId)
                        {
                            chatText = "You are the spectator, your died because your role can't influence the game.";
                            system = true;
                        }
                        else if (playerRole.DeathReason == DeathReasons.Exiled && sourcePlayer.PlayerId == PlayerControl.LocalPlayer.PlayerId)
                        {
                            chatText = "You died because you were ejected.";
                            system = true;
                        }
                        else if (playerRole.DeathReason == DeathReasons.Misfired && sourcePlayer.PlayerId == PlayerControl.LocalPlayer.PlayerId)
                        {
                            chatText = "You died because you misfired.";
                            system = true;
                        }
                        else if (playerRole.DeathReason == DeathReasons.Suicided && sourcePlayer.PlayerId == PlayerControl.LocalPlayer.PlayerId)
                        {
                            chatText = "You died because you suicided.";
                            system = true;
                        }
                        else if (sourcePlayer.PlayerId == PlayerControl.LocalPlayer.PlayerId)
                        {
                            foreach (var deadPlayer in Murder.KilledPlayers)
                            {
                                if (deadPlayer.PlayerId == PlayerControl.LocalPlayer.PlayerId)
                                {
                                    var killerPlayer = Utils.PlayerById(deadPlayer.KillerId);
                                    var roleName = killerPlayer.Is(RoleEnum.Glitch) ? $"The {Role.GetRole(killerPlayer).Name}" : Role.GetRole(killerPlayer).Name;
                                    if (CustomGameOptions.DeadSeeRoles && Utils.ShowDeadBodies)
                                    {
                                        chatText = $"You were killed by {killerPlayer.GetDefaultOutfit().PlayerName}.Their role is {roleName}.\nYour death reason is: {playerRole.DeathReason}.";
                                    }
                                    else chatText = $"You were killed by {killerPlayer.GetDefaultOutfit().PlayerName}.\nYour death reason is: {playerRole.DeathReason}.";
                                }
                                system = true;
                            }
                        }
                    }
                    else if (sourcePlayer.PlayerId == PlayerControl.LocalPlayer.PlayerId)
                    {
                        chatText = "You can only use this command in game!";
                        error = true;
                    }
                    return sourcePlayer.PlayerId == PlayerControl.LocalPlayer.PlayerId;
                }

                if (chatText.ToLower().StartsWith("/note "))
                {
                    if ((AmongUsClient.Instance.GameState == InnerNet.InnerNetClient.GameStates.Started || AmongUsClient.Instance.NetworkMode == NetworkModes.FreePlay) && sourcePlayer.PlayerId == PlayerControl.LocalPlayer.PlayerId)
                    {
                        string note = chatText.Substring(5).Trim();
                        var sourcePlayerRole = Role.GetRole(sourcePlayer);
                        sourcePlayerRole.PlayerNotes += note + "\n";
                        chatText = $"<b>New Note:</b>\n{note}";
                        system = true;
                    }
                    else if (sourcePlayer.PlayerId == PlayerControl.LocalPlayer.PlayerId)
                    {
                        chatText = "You can only use this command in game!";
                        error = true;
                    }
                    return sourcePlayer.PlayerId == PlayerControl.LocalPlayer.PlayerId;
                }

                if (chatText.ToLower().StartsWith("/seenote"))
                {
                    if ((AmongUsClient.Instance.GameState == InnerNet.InnerNetClient.GameStates.Started || AmongUsClient.Instance.NetworkMode == NetworkModes.FreePlay) && sourcePlayer.PlayerId == PlayerControl.LocalPlayer.PlayerId)
                    {
                        var sourcePlayerRole = Role.GetRole(sourcePlayer);
                        chatText = $"<b>Notes:</b>\n{sourcePlayerRole.PlayerNotes}";
                        system = true;
                    }
                    else if (sourcePlayer.PlayerId == PlayerControl.LocalPlayer.PlayerId)
                    {
                        chatText = "You can only use this command in game!";
                        error = true;
                    }
                    return sourcePlayer.PlayerId == PlayerControl.LocalPlayer.PlayerId;
                }

                if (chatText.ToLower().StartsWith("/roles"))
                {
                    if (sourcePlayer.PlayerId == PlayerControl.LocalPlayer.PlayerId)
                    {
                        system = true;
                        ActiveRolesMessage();
                        return false;
                    }
                    return sourcePlayer.PlayerId == PlayerControl.LocalPlayer.PlayerId;
                }

                if (chatText.ToLower().StartsWith("/modi"))
                {
                    if (sourcePlayer.PlayerId == PlayerControl.LocalPlayer.PlayerId)
                    {
                        system = true;
                        ActiveModifiersMessage();
                        return false;
                    }
                    return sourcePlayer.PlayerId == PlayerControl.LocalPlayer.PlayerId;
                }

                if (chatText.ToLower().StartsWith("/limit "))
                {
                    if ((AmongUsClient.Instance.GameState == InnerNet.InnerNetClient.GameStates.Started || AmongUsClient.Instance.NetworkMode == NetworkModes.FreePlay) && sourcePlayer.PlayerId == PlayerControl.LocalPlayer.PlayerId)
                    {
                        if (sourcePlayer.PlayerId == PlayerControl.LocalPlayer.PlayerId)
                        {
                            chatText = "You cannot use this command during the game!";
                            error = true;
                        }
                        return sourcePlayer.PlayerId == PlayerControl.LocalPlayer.PlayerId;
                    }
                    if (GameData.Instance.GetHost() == sourcePlayer.Data && sourcePlayer.PlayerId == PlayerControl.LocalPlayer.PlayerId)
                    {
                        string[] args = chatText.Split(' ');
                        if (args.Length > 1 && int.TryParse(args[1], out int newLimit))
                        {
                            if (newLimit >= 4 && newLimit <= 35)
                            {
                                try
                                {
                                    GameOptionsManager.Instance.CurrentGameOptions.SetInt(Int32OptionNames.MaxPlayers, newLimit);
                                    if (sourcePlayer.PlayerId == PlayerControl.LocalPlayer.PlayerId)
                                    {
                                        chatText = $"Player limit has been set for: <color=#D91919FF><b>{newLimit}</b></color>";
                                        system = true;
                                    }
                                }
                                catch { }
                            }
                            else
                            {
                                if (sourcePlayer.PlayerId == PlayerControl.LocalPlayer.PlayerId)
                                {
                                    chatText = "The /limit command has a range of 4 - 35!";
                                    error = true;
                                }
                            }
                        }
                        else
                        {
                            if (sourcePlayer.PlayerId == PlayerControl.LocalPlayer.PlayerId)
                            {
                                chatText = "Use /limit [number]. Example: /limit 20.";
                                error = true;
                            }
                        }
                    }
                    else if (sourcePlayer.PlayerId == PlayerControl.LocalPlayer.PlayerId)
                    {
                        if (sourcePlayer.PlayerId == PlayerControl.LocalPlayer.PlayerId)
                        {
                            chatText = "You don't have access to this command!";
                            noaccess = true;
                        }
                    }
                    return sourcePlayer.PlayerId == PlayerControl.LocalPlayer.PlayerId;
                }
                if (chatText.ToLower().StartsWith("/"))
                {
                    if (sourcePlayer.PlayerId == PlayerControl.LocalPlayer.PlayerId)
                    {
                        chatText = "Invalid Command. Type /help to display a list of available commands.";
                        error = true;
                    }
                    return sourcePlayer.PlayerId == PlayerControl.LocalPlayer.PlayerId;
                }
                return true;
            }
            public static void ActiveRolesMessage()
            {
                Dictionary<string, Color> ColorMapping = new Dictionary<string, Color>();

                ColorMapping.Add("<b>Crewmate <color=#F2F2F2FF>Basic</color></b>\n", Palette.CrewmateBlue);
                if (CustomGameOptions.CrewmateOn > 0) ColorMapping.Add("Crewmate", Palette.CrewmateBlue);
                ColorMapping.Add("\n<b>Crewmate <color=#D3D3D3FF>Ghost</color></b>\n", Palette.CrewmateBlue);
                if (CustomGameOptions.GuardianOn > 0) ColorMapping.Add("Guardian", Colors.Guardian);
                if (CustomGameOptions.HaunterOn > 0) ColorMapping.Add("Haunter", Colors.Haunter);
                if (CustomGameOptions.HelperOn > 0) ColorMapping.Add("Helper", Colors.Helper);
                ColorMapping.Add("\n<b>Crewmate <color=#00B3B3FF>Investigative</color></b>\n", Palette.CrewmateBlue);
                if (CustomGameOptions.AstralOn > 0) ColorMapping.Add("Astral", Colors.Astral);
                if (CustomGameOptions.AurialOn > 0) ColorMapping.Add("Aurial", Colors.Aurial);
                if (CustomGameOptions.CaptainOn > 0) ColorMapping.Add("Captain", Colors.Captain);
                if (CustomGameOptions.ChameleonOn > 0) ColorMapping.Add("Chameleon", Colors.Chameleon);
                if (CustomGameOptions.DetectiveOn > 0) ColorMapping.Add("Detective", Colors.Detective);
                if (CustomGameOptions.InformantOn > 0) ColorMapping.Add("Informant", Colors.Informant);
                if (CustomGameOptions.InvestigatorOn > 0) ColorMapping.Add("Investigator", Colors.Investigator);
                if (CustomGameOptions.LookoutOn > 0) ColorMapping.Add("Lookout", Colors.Lookout);
                if (CustomGameOptions.MysticOn > 0) ColorMapping.Add("Mystic", Colors.Mystic);
                if (CustomGameOptions.SeerOn > 0) ColorMapping.Add("Seer", Colors.Seer);
                if (CustomGameOptions.SnitchOn > 0) ColorMapping.Add("Snitch", Colors.Snitch);
                if (CustomGameOptions.SpyOn > 0) ColorMapping.Add("Spy", Colors.Spy);
                if (CustomGameOptions.TrackerOn > 0) ColorMapping.Add("Tracker", Colors.Tracker);
                if (CustomGameOptions.TrapperOn > 0) ColorMapping.Add("Trapper", Colors.Trapper);
                if (CustomGameOptions.WatcherOn > 0) ColorMapping.Add("Watcher", Colors.Lookout);
                ColorMapping.Add("\n<b>Crewmate <color=#FF0000FF>Killing</color></b>\n", Palette.CrewmateBlue);
                if (CustomGameOptions.AvengerOn > 0) ColorMapping.Add("Avenger", Colors.Avenger);
                if (CustomGameOptions.DeputyOn > 0) ColorMapping.Add("Deputy", Colors.Deputy);
                if (CustomGameOptions.FighterOn > 0) ColorMapping.Add("Fighter", Colors.Fighter);
                if (CustomGameOptions.HunterOn > 0) ColorMapping.Add("Hunter", Colors.Hunter);
                if (CustomGameOptions.KnightOn > 0) ColorMapping.Add("Knight", Colors.Knight);
                if (CustomGameOptions.SheriffOn > 0) ColorMapping.Add("Sheriff", Colors.Sheriff);
                if (CustomGameOptions.VampireHunterOn > 0) ColorMapping.Add("Vampire Hunter", Colors.VampireHunter);
                if (CustomGameOptions.VeteranOn > 0) ColorMapping.Add("Veteran", Colors.Veteran);
                if (CustomGameOptions.VigilanteOn > 0) ColorMapping.Add("Vigilante", Colors.Vigilante);
                ColorMapping.Add("\n<b>Crewmate <color=#704FA8FF>Power</color></b>\n", Palette.CrewmateBlue);
                if (CustomGameOptions.JailorOn > 0) ColorMapping.Add("Jailor", Colors.Jailor);
                if (CustomGameOptions.PoliticianOn > 0) ColorMapping.Add("Politician", Colors.Politician);
                if (CustomGameOptions.ProsecutorOn > 0) ColorMapping.Add("Prosecutor", Colors.Prosecutor);
                if (CustomGameOptions.SwapperOn > 0) ColorMapping.Add("Swapper", Colors.Swapper);
                ColorMapping.Add("\n<b>Crewmate <color=#006600FF>Protective</color></b>\n", Palette.CrewmateBlue);
                if (CustomGameOptions.AltruistOn > 0) ColorMapping.Add("Altruist", Colors.Altruist);
                if (CustomGameOptions.BodyguardOn > 0) ColorMapping.Add("Bodyguard", Colors.Bodyguard);
                if (CustomGameOptions.CrusaderOn > 0) ColorMapping.Add("Crusader", Colors.Crusader);
                if (CustomGameOptions.ClericOn > 0) ColorMapping.Add("Cleric", Colors.Cleric);
                if (CustomGameOptions.DoctorOn > 0) ColorMapping.Add("Doctor", Colors.Doctor);
                if (CustomGameOptions.MedicOn > 0) ColorMapping.Add("Medic", Colors.Medic);
                if (CustomGameOptions.OracleOn > 0) ColorMapping.Add("Oracle", Colors.Oracle);
                if (CustomGameOptions.WardenOn > 0) ColorMapping.Add("Warden", Colors.Warden);
                ColorMapping.Add("\n<b>Crewmate <color=#FFA60AFF>Support</color></b>\n", Palette.CrewmateBlue);
                if (CustomGameOptions.EngineerOn > 0) ColorMapping.Add("Engineer", Colors.Engineer);
                if (CustomGameOptions.ImitatorOn > 0) ColorMapping.Add("Imitator", Colors.Imitator);
                if (CustomGameOptions.MediumOn > 0) ColorMapping.Add("Medium", Colors.Medium);
                if (CustomGameOptions.ParanoïacOn > 0) ColorMapping.Add("Paranoïac", Colors.Paranoïac);
                if (CustomGameOptions.PlumberOn > 0) ColorMapping.Add("Plumber", Colors.Plumber);
                if (CustomGameOptions.TimeLordOn > 0) ColorMapping.Add("Time Lord", Colors.TimeLord);
                if (CustomGameOptions.TransporterOn > 0) ColorMapping.Add("Transporter", Colors.Transporter);
                ColorMapping.Add("\n<b>Neutral <color=#B3FFFFFF>Benign</color></b>\n", Palette.DisabledGrey);
                if (CustomGameOptions.AmnesiacOn > 0) ColorMapping.Add("Amnesiac", Colors.Amnesiac);
                if (CustomGameOptions.GuardianAngelOn > 0) ColorMapping.Add("Guardian Angel", Colors.GuardianAngel);
                if (CustomGameOptions.MercenaryOn > 0) ColorMapping.Add("Mercenary", Colors.Mercenary);
                if (CustomGameOptions.ShifterOn > 0) ColorMapping.Add("Shifter", Colors.Shifter);
                if (CustomGameOptions.SurvivorOn > 0) ColorMapping.Add("Survivor", Colors.Survivor);
                ColorMapping.Add("\n<b>Neutral <color=#8C4005FF>Evil</color></b>\n", Palette.DisabledGrey);
                if (CustomGameOptions.DoomsayerOn > 0) ColorMapping.Add("Doomsayer", Colors.Doomsayer);
                if (CustomGameOptions.ExecutionerOn > 0) ColorMapping.Add("Executioner", Colors.Executioner);
                if (CustomGameOptions.JesterOn > 0) ColorMapping.Add("Jester", Colors.Jester);
                if (CustomGameOptions.PhantomOn > 0) ColorMapping.Add("Phantom", Colors.Phantom);
                if (CustomGameOptions.TrollOn > 0) ColorMapping.Add("Troll", Colors.Troll);
                if (CustomGameOptions.VultureOn > 0) ColorMapping.Add("Vulture", Colors.Vulture);
                ColorMapping.Add("\n<b>Neutral <color=#FF0000FF>Killing</color></b>\n", Palette.DisabledGrey);
                if (CustomGameOptions.ArsonistOn > 0) ColorMapping.Add("Arsonist", Colors.Arsonist);
                if (CustomGameOptions.AttackerOn > 0) ColorMapping.Add("Attacker", Colors.Attacker);
                if (CustomGameOptions.DoppelgangerOn > 0) ColorMapping.Add("Doppelganger", Colors.Doppelganger);
                if (CustomGameOptions.InfectiousOn > 0) ColorMapping.Add("Infectious", Colors.Infectious);
                if (CustomGameOptions.JuggernautOn > 0) ColorMapping.Add("Juggernaut", Colors.Juggernaut);
                if (CustomGameOptions.WerewolfOn > 0) ColorMapping.Add("Maul", Colors.Werewolf);
                if (CustomGameOptions.MutantOn > 0) ColorMapping.Add("Mutant", Colors.Mutant);
                if (CustomGameOptions.PlaguebearerOn > 0) ColorMapping.Add("Plaguebearer", Colors.Plaguebearer);
                if (CustomGameOptions.SerialKillerOn > 0) ColorMapping.Add("Serial Killer", Colors.SerialKiller);
                if (CustomGameOptions.SoulCollectorOn > 0) ColorMapping.Add("Soul Collector", Colors.SoulCollector);
                if (CustomGameOptions.GlitchOn > 0) ColorMapping.Add("The Glitch", Colors.Glitch);
                if (CustomGameOptions.VampireOn > 0) ColorMapping.Add("Vampire", Colors.Vampire);
                ColorMapping.Add("\n<b>Impostor Basic</b>\n", Palette.ImpostorRed);
                if (CustomGameOptions.ImpostorOn > 0) ColorMapping.Add("Impostor", Colors.Impostor);
                ColorMapping.Add("\n<b>Impostor Concealing</b>\n", Palette.ImpostorRed);
                if (CustomGameOptions.EscapistOn > 0) ColorMapping.Add("Escapist", Colors.Impostor);
                if (CustomGameOptions.GrenadierOn > 0) ColorMapping.Add("Grenadier", Colors.Impostor);
                if (CustomGameOptions.MorphlingOn > 0) ColorMapping.Add("Morphling", Colors.Impostor);
                if (CustomGameOptions.NoclipOn > 0) ColorMapping.Add("Noclip", Colors.Impostor);
                if (CustomGameOptions.SwooperOn > 0) ColorMapping.Add("Swooper", Colors.Impostor);
                if (CustomGameOptions.VenererOn > 0) ColorMapping.Add("Venerer", Colors.Impostor);
                ColorMapping.Add("\n<b>Impostor Ghost</b>\n", Palette.ImpostorRed);
                if (CustomGameOptions.BlinderOn > 0) ColorMapping.Add("Blinder", Colors.Impostor);
                if (CustomGameOptions.FreezerOn > 0) ColorMapping.Add("Freezer", Colors.Impostor);
                if (CustomGameOptions.WraithOn > 0) ColorMapping.Add("Wraith", Colors.Impostor);
                ColorMapping.Add("\n<b>Impostor Killing</b>\n", Palette.ImpostorRed);
                if (CustomGameOptions.AssassinOn > 0 && CustomGameOptions.AssassinImpostorRole) ColorMapping.Add("Assassin", Colors.Impostor);
                if (CustomGameOptions.BomberOn > 0) ColorMapping.Add("Bomber", Colors.Impostor);
                if (CustomGameOptions.KamikazeOn > 0) ColorMapping.Add("Kamikaze", Colors.Impostor);
                if (CustomGameOptions.BountyHunterOn > 0) ColorMapping.Add("Bounty Hunter", Colors.Impostor);
                if (CustomGameOptions.ConjurerOn > 0) ColorMapping.Add("Conjurer", Colors.Impostor);
                if (CustomGameOptions.ManipulatorOn > 0) ColorMapping.Add("Manipulator", Colors.Impostor);
                if (CustomGameOptions.PoisonerOn > 0) ColorMapping.Add("Poisoner", Colors.Impostor);
                if (CustomGameOptions.ShooterOn > 0) ColorMapping.Add("Shooter", Colors.Impostor);
                if (CustomGameOptions.TraitorOn > 0) ColorMapping.Add("Traitor", Colors.Impostor);
                if (CustomGameOptions.WarlockOn > 0) ColorMapping.Add("Warlock", Colors.Impostor);
                if (CustomGameOptions.WitchOn > 0) ColorMapping.Add("Witch", Colors.Impostor);
                ColorMapping.Add("\n<b>Impostor Support</b>\n", Palette.ImpostorRed);
                if (CustomGameOptions.BlackmailerOn > 0) ColorMapping.Add("Blackmailer", Colors.Impostor);
                if (CustomGameOptions.ConverterOn > 0) ColorMapping.Add("Converter", Colors.Impostor);
                if (CustomGameOptions.ConverterOn > 0 || CustomGameOptions.MadmateOn > 0 || CustomGameOptions.FighterOn > 0) ColorMapping.Add("Madmate", Colors.Impostor);
                if (CustomGameOptions.HypnotistOn > 0) ColorMapping.Add("Hypnotist", Colors.Impostor);
                if (CustomGameOptions.JanitorOn > 0) ColorMapping.Add("Janitor", Colors.Impostor);
                if (CustomGameOptions.MafiosoOn > 0) ColorMapping.Add("Mafioso", Colors.Impostor);
                if (CustomGameOptions.MinerOn > 0) ColorMapping.Add("Miner", Colors.Impostor);
                if (CustomGameOptions.ReviverOn > 0) ColorMapping.Add("Reviver", Colors.Impostor);
                if (CustomGameOptions.UndertakerOn > 0) ColorMapping.Add("Undertaker", Colors.Impostor);
                ColorMapping.Add("\n<b>Coven Basic</b>\n", Colors.Coven);
                if (CustomGameOptions.CovenOn > 0) ColorMapping.Add("Coven", Colors.Coven);
                ColorMapping.Add("\n<b>Coven Killing</b>\n", Colors.Coven);
                if (CustomGameOptions.HexMasterOn > 0) ColorMapping.Add("Hex Master", Colors.Coven);
                if (CustomGameOptions.RitualistOn > 0) ColorMapping.Add("Ritualist", Colors.Coven);
                ColorMapping.Add("\n<b>Coven Support</b>\n", Colors.Coven);
                if (CustomGameOptions.CovenLeaderOn > 0) ColorMapping.Add("Coven Leader", Colors.Coven);
                if (CustomGameOptions.PotionMasterOn > 0) ColorMapping.Add("Potion Master", Colors.Coven);
                if (CustomGameOptions.SpiritualistOn > 0) ColorMapping.Add("Spiritualist", Colors.Coven);
                if (CustomGameOptions.VoodooMasterOn > 0) ColorMapping.Add("Voodoo Master", Colors.Coven);

                string mess = "";
                foreach (var roles in ColorMapping)
                {
                    mess += $"<color=#{roles.Value.ToHtmlStringRGBA()}> {roles.Key} </color>|";
                }

                HudManager.Instance.Chat.AddChat(PlayerControl.LocalPlayer, mess);
            }
            public static void ActiveModifiersMessage()
            {
                Dictionary<string, Color> ColorMapping = new Dictionary<string, Color>();

                ColorMapping.Add("<b>Crewmate Modifiers</b>\n", Palette.CrewmateBlue);
                if (CustomGameOptions.AftermathOn > 0) ColorMapping.Add("Aftermath", Colors.Aftermath);
                if (CustomGameOptions.BaitOn > 0) ColorMapping.Add("Bait", Colors.Bait);
                if (CustomGameOptions.CelebrityOn > 0) ColorMapping.Add("Celebrity", Colors.Celebrity);
                if (CustomGameOptions.DiseasedOn > 0) ColorMapping.Add("Diseased", Colors.Diseased);
                if (CustomGameOptions.FrostyOn > 0) ColorMapping.Add("Frosty", Colors.Frosty);
                if (CustomGameOptions.MadmateOn > 0) ColorMapping.Add("Madmate", Colors.Impostor);
                if (CustomGameOptions.MultitaskerOn > 0) ColorMapping.Add("Multitasker", Colors.Multitasker);
                if (CustomGameOptions.TaskmasterOn > 0) ColorMapping.Add("Taskmaster", Colors.Taskmaster);
                if (CustomGameOptions.TorchOn > 0) ColorMapping.Add("Torch", Colors.Torch);
                if (CustomGameOptions.VengefulOn > 0) ColorMapping.Add("Vengeful", Colors.Vengeful);
                ColorMapping.Add("\n<b>Global Modifiers</b>\n", Palette.DisabledGrey);
                if (CustomGameOptions.ButtonBarryOn > 0) ColorMapping.Add("Button Barry", Colors.ButtonBarry);
                if (CustomGameOptions.DrunkOn > 0) ColorMapping.Add("Drunk", Colors.Drunk);
                if (CustomGameOptions.FlashOn > 0) ColorMapping.Add("Flash", Colors.Flash);
                if (CustomGameOptions.GiantOn > 0) ColorMapping.Add("Giant", Colors.Giant);
                if (CustomGameOptions.LoversOn > 0) ColorMapping.Add("Lovers", Colors.Lovers);
                if (CustomGameOptions.MiniOn > 0) ColorMapping.Add("Mini", Colors.Mini);
                if (CustomGameOptions.MotionlessOn > 0) ColorMapping.Add("Motionless", Colors.Motionless);
                if (CustomGameOptions.RadarOn > 0) ColorMapping.Add("Radar", Colors.Radar);
                if (CustomGameOptions.SatelliteOn > 0) ColorMapping.Add("Satellite", Colors.Satellite);
                if (CustomGameOptions.ScientistOn > 0) ColorMapping.Add("Scientist", Colors.Scientist);
                if (CustomGameOptions.ShyOn > 0) ColorMapping.Add("Shy", Colors.Shy);
                if (CustomGameOptions.SixthSenseOn > 0) ColorMapping.Add("Sixth Sense", Colors.SixthSense);
                if (CustomGameOptions.SleuthOn > 0) ColorMapping.Add("Sleuth", Colors.Sleuth);
                if (CustomGameOptions.SuperstarOn > 0) ColorMapping.Add("Superstar", Colors.Superstar);
                if (CustomGameOptions.TiebreakerOn > 0) ColorMapping.Add("Tiebreaker", Colors.Tiebreaker);
                ColorMapping.Add("\n<b>Impostor Modifiers</b>\n", Palette.ImpostorRed);
                if (CustomGameOptions.BloodlustOn > 0) ColorMapping.Add("Bloodlust", Colors.Impostor);
                if (CustomGameOptions.DisperserOn > 0) ColorMapping.Add("Disperser", Colors.Impostor);
                if (CustomGameOptions.DoubleShotOn > 0) ColorMapping.Add("Double Shot", Colors.Impostor);
                if (CustomGameOptions.LuckyOn > 0) ColorMapping.Add("Lucky", Colors.Impostor);
                if (CustomGameOptions.SaboteurOn > 0) ColorMapping.Add("Saboteur", Colors.Impostor);
                if (CustomGameOptions.TaskerOn > 0) ColorMapping.Add("Tasker", Colors.Impostor);
                if (CustomGameOptions.UnderdogOn > 0) ColorMapping.Add("Underdog", Colors.Impostor);

                string mess = "";
                foreach (var modifiers in ColorMapping)
                {
                    mess += $"<color=#{modifiers.Value.ToHtmlStringRGBA()}> {modifiers.Key} </color>|";
                }

                HudManager.Instance.Chat.AddChat(PlayerControl.LocalPlayer, mess);
            }
        }

        [HarmonyPatch(typeof(PoolablePlayer), nameof(PoolablePlayer.UpdateFromPlayerOutfit))]
        public class ChatFixAlive
        {
            public static void Prefix(PoolablePlayer __instance, ref bool forceAlive)
            {
                var AllBubbles = GameObject.FindObjectsOfType<ChatBubble>();
                var CustomBubbles = AllBubbles.Where(x => x.name == "HostBubble" || x.name == "DevBubble");
                if (!CustomBubbles.Any(x => x.Player == __instance)) return;
                forceAlive = true;
            }
        }

        [HarmonyPatch(typeof(ChatBubble), nameof(ChatBubble.SetName))]
        public class ChatFixName
        {
            public static bool Prefix(ChatBubble __instance)
            {
                if (host)
                {
                    __instance.NameText.text = "HOST MESSAGE";
                    __instance.NameText.color = Palette.Orange;
                    __instance.NameText.ForceMeshUpdate(true, true);
                    __instance.Xmark.enabled = false;
                    __instance.Background.color = Palette.White;
                    __instance.votedMark.enabled = false;
                    host = false;
                    return false;
                }
                else if (error)
                {
                    __instance.NameText.text = "ERROR";
                    __instance.NameText.color = Palette.ImpostorRed;
                    __instance.NameText.ForceMeshUpdate(true, true);
                    __instance.Xmark.enabled = true;
                    __instance.Background.color = Palette.White;
                    __instance.votedMark.enabled = false;
                    error = false;
                    return false;
                }
                else if (system)
                {
                    __instance.NameText.text = "SYSTEM MESSAGE";
                    __instance.NameText.color = Palette.CrewmateBlue;
                    __instance.NameText.ForceMeshUpdate(true, true);
                    __instance.Xmark.enabled = false;
                    __instance.Background.color = Palette.White;
                    __instance.votedMark.enabled = false;
                    system = false;
                    return false;
                }
                else if (noaccess)
                {
                    __instance.NameText.text = "NO ACCESS";
                    __instance.NameText.color = Palette.Blue;
                    __instance.NameText.ForceMeshUpdate(true, true);
                    __instance.Xmark.enabled = true;
                    __instance.Background.color = Palette.White;
                    __instance.votedMark.enabled = false;
                    noaccess = false;
                    return false;
                }
                else return true;
            }
        }
    }
}