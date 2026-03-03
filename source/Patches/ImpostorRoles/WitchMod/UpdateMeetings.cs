using HarmonyLib;
using Reactor.Utilities.Extensions;
using System;
using System.Linq;
using TownOfUsEdited.CrewmateRoles.ImitatorMod;
using TownOfUsEdited.CrewmateRoles.MedicMod;
using TownOfUsEdited.CrewmateRoles.SwapperMod;
using TownOfUsEdited.CrewmateRoles.VigilanteMod;
using TownOfUsEdited.Extensions;
using TownOfUsEdited.Modifiers.AssassinMod;
using TownOfUsEdited.NeutralRoles.DoomsayerMod;
using TownOfUsEdited.Patches;
using TownOfUsEdited.Roles;
using TownOfUsEdited.Roles.Modifiers;
using UnityEngine;
using UnityEngine.UI;
using Assassin = TownOfUsEdited.Roles.Modifiers.Assassin;
using Object = UnityEngine.Object;

namespace TownOfUsEdited.ImpostorRoles.WitchMod
{
    public class ApplyCurse
    {
        [HarmonyPatch(typeof(ExileController), nameof(ExileController.BeginForGameplay))]
        public class KillCurse
        {
            public static void Postfix(ExileController __instance)
            {
                foreach (var role in Role.GetRoles(RoleEnum.Prosecutor))
                {
                    var prosecutor = (Prosecutor)role;
                    if (prosecutor.ProsecuteThisMeeting) return;
                }
                var witchs = Role.AllRoles.Where(x => x.RoleType == RoleEnum.Witch && !x.Player.Data.IsDead).Cast<Witch>();
                foreach (var role in witchs)
                {
                    var cursed = role.CursedList;
                    foreach (var playerid in cursed)
                    {
                        var exiled = __instance.initData?.networkedPlayer?.Object;
                        var player = Utils.PlayerById(playerid);
                        if (exiled == role.Player) return;
                        if (!player.Data.IsDead && !player.Is(RoleEnum.Pestilence))
                        {
                            MurderPlayer(player, role.Player, true);
                            Utils.Rpc(CustomRPC.WitchMurder, player.PlayerId, role.Player.PlayerId, true);
                        }
                    }
                }
            }
        }

        [HarmonyPatch(typeof(AirshipExileController._WrapUpAndSpawn_d__11), nameof(AirshipExileController._WrapUpAndSpawn_d__11.MoveNext))]
        public static class AirshipExileController_WrapUpAndSpawn
        {
            public static void Postfix(AirshipExileController._WrapUpAndSpawn_d__11 __instance) => ClearList.ExileControllerPostfix(__instance.__4__this);
        }

        [HarmonyPatch(typeof(ExileController), nameof(ExileController.WrapUp))]
        public class ClearList
        {
            public static void ExileControllerPostfix(ExileController __instance)
            {
                var witchs = Role.AllRoles.Where(x => x.RoleType == RoleEnum.Witch && !x.Player.Data.IsDead).Cast<Witch>();
                foreach (var role in witchs)
                {
                    var cursed = role.CursedList;
                    cursed.Clear();
                }
            }

            public static void Postfix(ExileController __instance) => ExileControllerPostfix(__instance);

            [HarmonyPatch(typeof(Object), nameof(Object.Destroy), new Type[] { typeof(GameObject) })]
            public static void Prefix(GameObject obj)
            {
                if (!SubmergedCompatibility.Loaded || GameOptionsManager.Instance?.currentNormalGameOptions?.MapId != 6) return;
                if (obj.name?.Contains("ExileCutscene") == true) ExileControllerPostfix(ExileControllerPatch.lastExiled);
            }
        }

        public static void MurderPlayer(PlayerControl player, PlayerControl witch, bool checkLover)
        {
            var hudManager = HudManager.Instance;
            var amOwner = player.AmOwner;
            if (amOwner)
            {
                Utils.ShowDeadBodies = true;
                hudManager.ShadowQuad.gameObject.SetActive(false);
                player.nameText().GetComponent<MeshRenderer>().material.SetInt("_Mask", 0);
                player.RpcSetScanner(false);

                if (player.Is(RoleEnum.Swapper))
                {
                    var swapper = Role.GetRole<Swapper>(PlayerControl.LocalPlayer);
                    swapper.ListOfActives.Clear();
                    swapper.Buttons.Clear();
                    SwapVotes.Swap1 = null;
                    SwapVotes.Swap2 = null;
                    Utils.Rpc(CustomRPC.SetSwaps, sbyte.MaxValue, sbyte.MaxValue);
                    var buttons = Role.GetRole<Swapper>(player).Buttons;
                    foreach (var button in buttons)
                    {
                        button.SetActive(false);
                        button.GetComponent<PassiveButton>().OnClick = new Button.ButtonClickedEvent();
                    }
                }

                if (player.Is(RoleEnum.Imitator))
                {
                    var imitator = Role.GetRole<Imitator>(PlayerControl.LocalPlayer);
                    imitator.ListOfActives.Clear();
                    imitator.Buttons.Clear();
                    SetImitate.Imitate = null;
                    var buttons = Role.GetRole<Imitator>(player).Buttons;
                    foreach (var button in buttons)
                    {
                        button.SetActive(false);
                        button.GetComponent<PassiveButton>().OnClick = new Button.ButtonClickedEvent();
                    }
                }

                if (player.Is(RoleEnum.Vigilante))
                {
                    var retributionist = Role.GetRole<Vigilante>(PlayerControl.LocalPlayer);
                    ShowHideButtonsVigi.HideButtonsVigi(retributionist);
                }

                if (player.Is(AbilityEnum.Assassin))
                {
                    var assassin = Ability.GetAbility<Assassin>(PlayerControl.LocalPlayer);
                    ShowHideButtons.HideButtons(assassin);
                }

                if (player.Is(RoleEnum.Doomsayer))
                {
                    var doomsayer = Role.GetRole<Doomsayer>(PlayerControl.LocalPlayer);
                    ShowHideButtonsDoom.HideButtonsDoom(doomsayer);
                }

                
            }
            player.Die(DeathReason.Kill, false);
            if (PlayerControl.LocalPlayer == witch)
            {
                SoundManager.Instance.PlaySound(player.KillSfx, false, 0.8f);
            }
            if (player.IsLover() && CustomGameOptions.BothLoversDie)
            {
                var otherLover = Modifier.GetModifier<Lover>(player).OtherLover.Player;
                if (!otherLover.Is(RoleEnum.Pestilence)) MurderPlayer(otherLover, witch, false);
            }

            if (checkLover == true)
            {
                var playerRole = Role.GetRole(player);
                playerRole.DeathReason = DeathReasons.Cursed;
                Utils.Rpc(CustomRPC.SetDeathReason, player.PlayerId, (byte)DeathReasons.Cursed);
            }
            else
            {
                var playerRole = Role.GetRole(player);
                playerRole.DeathReason = DeathReasons.Suicided;
                Utils.Rpc(CustomRPC.SetDeathReason, player.PlayerId, (byte)DeathReasons.Suicided);
            }

            var deadPlayer = new DeadPlayer
            {
                PlayerId = player.PlayerId,
                KillerId = witch.PlayerId,
                KillTime = System.DateTime.UtcNow,
            };

            var witchRole = Role.GetRole<Witch>(witch);
            witchRole.Kills += 1;

            Murder.KilledPlayers.Add(deadPlayer);

            AddHauntPatch.AssassinatedPlayers.Add(player);
        }
    }
}