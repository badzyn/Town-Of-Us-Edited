using AmongUs.GameOptions;
using HarmonyLib;
using TownOfUsEdited.Extensions;
using TownOfUsEdited.Roles;
using TownOfUsEdited.Roles.Modifiers;
using UnityEngine;

namespace TownOfUsEdited
{
    [HarmonyPatch(typeof(KillButton), nameof(KillButton.Start))]
    public static class KillButtonAwake
    {
        public static void Prefix(KillButton __instance)
        {
            __instance.transform.Find("Text_TMP").gameObject.SetActive(false);
        }
    }

    [HarmonyPatch(typeof(HudManager), nameof(HudManager.Update))]
    public class KillButtonSprite
    {
        private static Sprite Kill;
        public static void Postfix(HudManager __instance)
        {
            if (__instance.KillButton == null) return;

            if (!Kill) Kill = __instance.KillButton.graphic.sprite;

            if (!__instance.KillButton.isActiveAndEnabled) return;
            __instance.KillButton.graphic.SetCooldownNormalizedUvs();

            var flag = false;
            if (PlayerControl.LocalPlayer.Is(RoleEnum.Seer))
            {
                __instance.KillButton.graphic.sprite = TownOfUsEdited.SeerSprite;
                flag = true;
            }
            else if (PlayerControl.LocalPlayer.Is(RoleEnum.Medic))
            {
                __instance.KillButton.graphic.sprite = TownOfUsEdited.MedicSprite;
                flag = true;
            }
            else if (PlayerControl.LocalPlayer.Is(RoleEnum.Arsonist))
            {
                __instance.KillButton.graphic.sprite = TownOfUsEdited.DouseSprite;
                flag = true;
            }
            else if (PlayerControl.LocalPlayer.Is(RoleEnum.Altruist))
            {
                __instance.KillButton.graphic.sprite = TownOfUsEdited.ReviveSprite;
                flag = true;
            }
            else if (PlayerControl.LocalPlayer.Is(RoleEnum.Veteran) || PlayerControl.LocalPlayer.Is(RoleEnum.Helper))
            {
                __instance.KillButton.graphic.sprite = TownOfUsEdited.AlertSprite;
                flag = true;
            }
            else if (PlayerControl.LocalPlayer.Is(RoleEnum.Astral))
            {
                __instance.KillButton.graphic.sprite = TownOfUsEdited.Dissociate;
                flag = true;
            }
            else if (PlayerControl.LocalPlayer.Is(RoleEnum.TimeLord))
            {
                __instance.KillButton.graphic.sprite = TownOfUsEdited.Rewind;
                flag = true;
            }
            else if (PlayerControl.LocalPlayer.Is(RoleEnum.Amnesiac))
            {
                __instance.KillButton.graphic.sprite = TownOfUsEdited.RememberSprite;
                flag = true;
            }
            else if (PlayerControl.LocalPlayer.Is(RoleEnum.Tracker))
            {
                __instance.KillButton.graphic.sprite = TownOfUsEdited.TrackSprite;
                flag = true;
            }
            else if (PlayerControl.LocalPlayer.Is(RoleEnum.Transporter))
            {
                __instance.KillButton.graphic.sprite = TownOfUsEdited.TransportSprite;
                flag = true;
            }
            else if (PlayerControl.LocalPlayer.Is(RoleEnum.Spy))
            {
                __instance.KillButton.graphic.sprite = TownOfUsEdited.AdminSprite;
                flag = true;
            }
            else if (PlayerControl.LocalPlayer.Is(RoleEnum.Medium))
            {
                __instance.KillButton.graphic.sprite = TownOfUsEdited.MediateSprite;
                flag = true;
            }
            else if (PlayerControl.LocalPlayer.Is(RoleEnum.Survivor))
            {
                __instance.KillButton.graphic.sprite = TownOfUsEdited.VestSprite;
                flag = true;
            }
            else if (PlayerControl.LocalPlayer.Is(RoleEnum.GuardianAngel) || PlayerControl.LocalPlayer.Is(RoleEnum.Guardian))
            {
                var ga = RoleManager.Instance.GetRole(AmongUs.GameOptions.RoleTypes.GuardianAngel).Cast<GuardianAngelRole>();
                __instance.KillButton.graphic.sprite = ga.Ability.Image;
                flag = true;
            }
            else if (PlayerControl.LocalPlayer.Is(RoleEnum.Plaguebearer))
            {
                __instance.KillButton.graphic.sprite = TownOfUsEdited.InfectSprite;
                flag = true;
            }
            else if (PlayerControl.LocalPlayer.Is(RoleEnum.Engineer))
            {
                __instance.KillButton.graphic.sprite = TownOfUsEdited.EngineerFix;
                flag = true;
            }
            else if (PlayerControl.LocalPlayer.Is(RoleEnum.Trapper))
            {
                __instance.KillButton.graphic.sprite = TownOfUsEdited.TrapSprite;
                flag = true;
            }
            else if (PlayerControl.LocalPlayer.Is(RoleEnum.Captain))
            {
                __instance.KillButton.graphic.sprite = TownOfUsEdited.CapZoomButton;
                flag = true;
            }
            else if (PlayerControl.LocalPlayer.Is(RoleEnum.Jailor))
            {
                __instance.KillButton.graphic.sprite = TownOfUsEdited.JailSprite;
                flag = true;
            }
            
            else if (PlayerControl.LocalPlayer.Is(RoleEnum.Warden))
            {
                __instance.KillButton.graphic.sprite = TownOfUsEdited.FortifySprite;
                flag = true;
            }
            else if (PlayerControl.LocalPlayer.Is(RoleEnum.Doctor))
            {
                __instance.KillButton.graphic.sprite = TownOfUsEdited.DocReviveButton;
                flag = true;
            }
            else if (PlayerControl.LocalPlayer.Is(RoleEnum.Detective))
            {
                __instance.KillButton.graphic.sprite = TownOfUsEdited.InspectSprite;
                flag = true;
            }
            else if (PlayerControl.LocalPlayer.Is(RoleEnum.Chameleon))
            {
                __instance.KillButton.graphic.sprite = TownOfUsEdited.ChameleonSwoop;
                flag = true;
            }
            else if (PlayerControl.LocalPlayer.Is(RoleEnum.Doomsayer))
            {
                __instance.KillButton.graphic.sprite = TownOfUsEdited.ObserveSprite;
                flag = true;
            }
            else if (PlayerControl.LocalPlayer.Is(RoleEnum.VampireHunter))
            {
                __instance.KillButton.graphic.sprite = TownOfUsEdited.StakeSprite;
                flag = true;
            }
            else if (PlayerControl.LocalPlayer.Is(RoleEnum.Oracle))
            {
                __instance.KillButton.graphic.sprite = TownOfUsEdited.ConfessSprite;
                flag = true;
            }
            else if (PlayerControl.LocalPlayer.Is(RoleEnum.Shifter))
            {
                __instance.KillButton.graphic.sprite = TownOfUsEdited.ShiftButton;
                flag = true;
            }
            else if (PlayerControl.LocalPlayer.Is(RoleEnum.Vulture))
            {
                __instance.KillButton.graphic.sprite = TownOfUsEdited.Eat;
                flag = true;
            }
            else if (PlayerControl.LocalPlayer.Is(RoleEnum.HexMaster))
            {
                __instance.KillButton.graphic.sprite = TownOfUsEdited.Hex;
                flag = true;
            }
            else if (PlayerControl.LocalPlayer.Is(RoleEnum.SoulCollector))
            {
                __instance.KillButton.graphic.sprite = TownOfUsEdited.ReapSprite;
                flag = true;
            }
            else if (PlayerControl.LocalPlayer.Is(RoleEnum.Plumber))
            {
                __instance.KillButton.graphic.sprite = TownOfUsEdited.BarricadeSprite;
                flag = true;
            }
            else if (PlayerControl.LocalPlayer.Is(RoleEnum.Mercenary))
            {
                __instance.KillButton.graphic.sprite = TownOfUsEdited.BribeSprite;
                flag = true;
            }
            else if (PlayerControl.LocalPlayer.Is(RoleEnum.Cleric))
            {
                __instance.KillButton.graphic.sprite = TownOfUsEdited.BarrierSprite;
                flag = true;
            }
            else if (PlayerControl.LocalPlayer.Is(RoleEnum.Deputy))
            {
                __instance.KillButton.graphic.sprite = TownOfUsEdited.CampSprite;
                flag = true;
            }
            else if (PlayerControl.LocalPlayer.Is(RoleEnum.Lookout) || PlayerControl.LocalPlayer.Is(RoleEnum.Watcher))
            {
                __instance.KillButton.graphic.sprite = TownOfUsEdited.WatchSprite;
                flag = true;
            }
            else if (PlayerControl.LocalPlayer.Is(RoleEnum.Poisoner))
            {
                __instance.KillButton.graphic.sprite = TownOfUsEdited.PoisonSprite;
                flag = true;
            }
            else
            {
                __instance.KillButton.graphic.sprite = Kill;
                __instance.KillButton.buttonLabelText.gameObject.SetActive(true);
                __instance.KillButton.buttonLabelText.text = "Kill";
                flag = PlayerControl.LocalPlayer.Is(RoleEnum.Sheriff) || PlayerControl.LocalPlayer.Is(RoleEnum.Pestilence) ||
                    PlayerControl.LocalPlayer.Is(RoleEnum.Werewolf) || PlayerControl.LocalPlayer.Is(RoleEnum.Juggernaut) ||
                    PlayerControl.LocalPlayer.Is(RoleEnum.Fighter) || PlayerControl.LocalPlayer.Is(RoleEnum.Knight) ||
                    PlayerControl.LocalPlayer.Is(RoleEnum.Mutant) || PlayerControl.LocalPlayer.Is(RoleEnum.SerialKiller) ||
                    PlayerControl.LocalPlayer.Is(Faction.Impostors) || PlayerControl.LocalPlayer.Is(RoleEnum.Troll) ||
                    PlayerControl.LocalPlayer.Is(RoleEnum.Player) || PlayerControl.LocalPlayer.Is(RoleEnum.Doppelganger) ||
                    PlayerControl.LocalPlayer.Is(RoleEnum.Crusader) || PlayerControl.LocalPlayer.Is(RoleEnum.Avenger) ||
                    PlayerControl.LocalPlayer.Is(RoleEnum.Terrorist) || PlayerControl.LocalPlayer.Is(RoleEnum.Hunter) ||
                    PlayerControl.LocalPlayer.Is(RoleEnum.Vampire) || PlayerControl.LocalPlayer.Is(Faction.Coven) ||
                    PlayerControl.LocalPlayer.Is(RoleEnum.Infectious);
            }
            if (!PlayerControl.LocalPlayer.Is(Faction.Impostors) &&
                GameOptionsManager.Instance.CurrentGameOptions.GameMode != GameModes.HideNSeek)
            {
                __instance.KillButton.transform.localPosition = new Vector3(0f, 1f, 0f);
            }
            if (PlayerControl.LocalPlayer.Is(RoleEnum.Engineer) || PlayerControl.LocalPlayer.Is(RoleEnum.Glitch)
                 || PlayerControl.LocalPlayer.Is(RoleEnum.Pestilence) || PlayerControl.LocalPlayer.Is(RoleEnum.Juggernaut)
                 || PlayerControl.LocalPlayer.Is(RoleEnum.Vampire) || PlayerControl.LocalPlayer.Is(RoleEnum.SerialKiller)
                 || PlayerControl.LocalPlayer.Is(RoleEnum.Mutant) || (PlayerControl.LocalPlayer.Is(RoleEnum.Doctor) && PlayerControl.LocalPlayer.Is(Faction.Madmates))
                 || PlayerControl.LocalPlayer.Is(RoleEnum.Paranoïac) || PlayerControl.LocalPlayer.Is(RoleEnum.Terrorist)
                 || PlayerControl.LocalPlayer.Is(RoleEnum.Vulture) || PlayerControl.LocalPlayer.Is(Faction.Coven)
                 || PlayerControl.LocalPlayer.Is(RoleEnum.Infectious) || PlayerControl.LocalPlayer.Is(RoleEnum.Doppelganger)
                 || PlayerControl.LocalPlayer.Is(RoleEnum.SoulCollector))
            {
                __instance.ImpostorVentButton.transform.localPosition = new Vector3(-2f, 0f, 0f);
            }
            else if (PlayerControl.LocalPlayer.Is(RoleEnum.Player))
            {
                __instance.ImpostorVentButton.transform.localPosition = new Vector3(-1f, 0f, 0f);
            }
            else if (PlayerControl.LocalPlayer.Is(RoleEnum.Werewolf) || PlayerControl.LocalPlayer.Is(RoleEnum.Arsonist)
            || PlayerControl.LocalPlayer.Is(RoleEnum.Plumber))
            {
                __instance.ImpostorVentButton.transform.localPosition = new Vector3(-1f, 1f, 0f);
            }
            else if (PlayerControl.LocalPlayer.Is(Faction.Impostors))
            {
                __instance.ImpostorVentButton.transform.localPosition = new Vector3(-1f, 1f, 0f);
            }

            bool KillKey = Rewired.ReInput.players.GetPlayer(0).GetButtonDown("Kill");
            var controller = ConsoleJoystick.player.GetButtonDown(8);
            if ((KillKey || controller) && __instance.KillButton != null && flag && !PlayerControl.LocalPlayer.Data.IsDead)
                __instance.KillButton.DoClick();

            var role = Role.GetRole(PlayerControl.LocalPlayer);
            bool AbilityKey = Rewired.ReInput.players.GetPlayer(0).GetButtonDown("ToU imp/nk");
            var controller2 = ConsoleJoystick.player.GetButtonDown(49); // they use this for vanilla role ability button normally
            if (role?.ExtraButtons != null && (AbilityKey || controller2) && !PlayerControl.LocalPlayer.Data.IsDead)
                role?.ExtraButtons[0]?.DoClick();

            var controllerRBLB = ConsoleJoystick.player.GetButtonDown(20) || ConsoleJoystick.player.GetButtonDown(22);
            if (Modifier.GetModifier<ButtonBarry>(PlayerControl.LocalPlayer)?.ButtonUsed == false &&
                (Rewired.ReInput.players.GetPlayer(0).GetButtonDown("ToU bb/disperse/mimic") ||
                controllerRBLB) &&
                !PlayerControl.LocalPlayer.Data.IsDead)
            {
                Modifier.GetModifier<ButtonBarry>(PlayerControl.LocalPlayer).ButtonButton.DoClick();
            }
            else if (Modifier.GetModifier<Disperser>(PlayerControl.LocalPlayer)?.ButtonUsed == false &&
                     (Rewired.ReInput.players.GetPlayer(0).GetButtonDown("ToU bb/disperse/mimic") ||
                     controllerRBLB) &&
                     !PlayerControl.LocalPlayer.Data.IsDead)
            {
                Modifier.GetModifier<Disperser>(PlayerControl.LocalPlayer).DisperseButton.DoClick();
            }
            else if (Modifier.GetModifier<Satellite>(PlayerControl.LocalPlayer)?.DetectUsed == false &&
                     (Rewired.ReInput.players.GetPlayer(0).GetButtonDown("ToU bb/disperse/mimic") ||
                     controllerRBLB) &&
                     !PlayerControl.LocalPlayer.Data.IsDead)
            {
                Modifier.GetModifier<Satellite>(PlayerControl.LocalPlayer).DetectButton.DoClick();
            }
        }

        [HarmonyPatch(typeof(HudManager), nameof(HudManager.Update))]
        class AbilityButtonUpdatePatch
        {
            static void Postfix()
            {
                if (AmongUsClient.Instance.GameState != InnerNet.InnerNetClient.GameStates.Started && AmongUsClient.Instance.NetworkMode != NetworkModes.FreePlay)
                {
                    HudManager.Instance.AbilityButton.gameObject.SetActive(false);
                    return;
                }
                else if (GameOptionsManager.Instance.CurrentGameOptions.GameMode == GameModes.HideNSeek)
                {
                    HudManager.Instance.AbilityButton.gameObject.SetActive(!PlayerControl.LocalPlayer.Data.IsImpostor());
                    return;
                }
                var ghostRole = false;
                if (PlayerControl.LocalPlayer.Is(RoleEnum.Haunter))
                {
                    var haunter = Role.GetRole<Haunter>(PlayerControl.LocalPlayer);
                    if (!haunter.Caught) ghostRole = true;
                }
                else if (PlayerControl.LocalPlayer.Is(RoleEnum.Phantom))
                {
                    var phantom = Role.GetRole<Phantom>(PlayerControl.LocalPlayer);
                    if (!phantom.Caught) ghostRole = true;
                }
                else if (PlayerControl.LocalPlayer.Is(RoleEnum.Wraith))
                {
                    var wraith = Role.GetRole<Wraith>(PlayerControl.LocalPlayer);
                    if (!wraith.Caught) ghostRole = true;
                }
                var ghost = RoleManager.Instance.GetRole(AmongUs.GameOptions.RoleTypes.CrewmateGhost).Cast<CrewmateGhostRole>();
                HudManager.Instance.AbilityButton.graphic.sprite = ghost.Ability.Image;
                HudManager.Instance.AbilityButton.SetInfiniteUses();
                HudManager.Instance.AbilityButton.buttonLabelText.text = "Haunt";
                HudManager.Instance.AbilityButton.transform.localPosition = new Vector3(-2f, 0f, 0f);
                HudManager.Instance.AbilityButton.buttonLabelText.SetOutlineColor(Palette.CrewmateBlue);
                HudManager.Instance.AbilityButton.gameObject.SetActive(PlayerControl.LocalPlayer.Data.IsDead && !ghostRole && Utils.ShowDeadBodies && !MeetingHud.Instance);
            }
        }

        [HarmonyPatch(typeof(AbilityButton), nameof(AbilityButton.DoClick))]
        class AbilityButtonDoClickPatch
        {
            static bool Prefix(AbilityButton __instance)
            {
                if (__instance != HudManager.Instance.AbilityButton) return true;
                if (!PlayerControl.LocalPlayer.Data.IsDead) return false;
                if (HudManager.Instance.Chat.IsOpenOrOpening)
                {
                    return false;
                }
                if (Minigame.Instance)
                {
                    if (Minigame.Instance is HauntMenuMinigame)
                    {
                        Minigame.Instance.Close();
                    }
                    return false;
                }
                var ghost = RoleManager.Instance.GetRole(AmongUs.GameOptions.RoleTypes.CrewmateGhost).Cast<CrewmateGhostRole>();
                Minigame minigame = Object.Instantiate<Minigame>(ghost.HauntMenu);
                minigame.transform.SetParent(HudManager.Instance.AbilityButton.transform, false);
                minigame.transform.SetLocalZ(-5f);
                minigame.Begin(null);
                HudManager.Instance.AbilityButton.SetDisabled();
                return false;
            }
        }
    }
}
