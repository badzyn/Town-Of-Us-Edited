using AmongUs.GameOptions;
using HarmonyLib;
using TownOfUsEdited;
using TownOfUsEdited.Extensions;
using TownOfUsEdited.Roles;

namespace FlashlightPatch
{
    [HarmonyPatch]
    public class FixFlashlight
    {
        [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.IsFlashlightEnabled))]
        [HarmonyPrefix]
        public static bool Prefix(ref bool __result)
        {
            if (LobbyBehaviour.Instance != null)
            {
                __result = false;
                return false;
            }
            if (PlayerControl.LocalPlayer.Data.IsDead)
            {
                __result = false;
                return false;
            }
            if (CustomGameOptions.FlashlightMode)
            {
                __result = true;
                return false;
            }
            if (GameOptionsManager.Instance.CurrentGameOptions.GameMode != GameModes.HideNSeek && !CustomGameOptions.FlashlightMode)
            {
                __result = false;
                return false;
            }
            GameOptionsManager.Instance.CurrentGameOptions.TryGetBool(BoolOptionNames.UseFlashlight, out bool result);
            __result = result;
            return false;
        }
    }

    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.AdjustLighting))]
    public static class AdjustLight
    {
        public static bool Prefix(PlayerControl __instance)
        {
            if (__instance == null || PlayerControl.LocalPlayer == null) return true;
            if (GameOptionsManager.Instance.CurrentGameOptions.GameMode == GameModes.HideNSeek) return true;
            if (AmongUsClient.Instance.GameState != InnerNet.InnerNetClient.GameStates.Started && AmongUsClient.Instance.NetworkMode != NetworkModes.FreePlay) return true;
            if (__instance.isDummy) return true;
            if (__instance.Data == null) return true;
            if (__instance.Data.IsDead) return true;

            bool hasFlashlight = !PlayerControl.LocalPlayer.Data.IsDead && CustomGameOptions.FlashlightMode;
            __instance.SetFlashlightInputMethod();
            if (PlayerControl.LocalPlayer.Data.IsImpostor() || PlayerControl.LocalPlayer.Is(RoleEnum.Glitch) ||
            PlayerControl.LocalPlayer.Is(RoleEnum.Juggernaut) || PlayerControl.LocalPlayer.Is(RoleEnum.Pestilence) ||
            (PlayerControl.LocalPlayer.Is(RoleEnum.Jester) && CustomGameOptions.JesterImpVision) ||
            (PlayerControl.LocalPlayer.Is(RoleEnum.Vulture) && CustomGameOptions.VultureImpVision) ||
            PlayerControl.LocalPlayer.Is(RoleEnum.Arsonist) || PlayerControl.LocalPlayer.Is(RoleEnum.SoulCollector) ||
            (PlayerControl.LocalPlayer.Is(RoleEnum.Vampire) && CustomGameOptions.VampImpVision) ||
            (PlayerControl.LocalPlayer.Is(RoleEnum.SerialKiller) && CustomGameOptions.SkImpVision) ||
            (PlayerControl.LocalPlayer.Is(Faction.Madmates) && CustomGameOptions.MadmateHasImpoVision) ||
            PlayerControl.LocalPlayer.Is(RoleEnum.Player) || PlayerControl.LocalPlayer.Is(RoleEnum.Terrorist) ||
            PlayerControl.LocalPlayer.Is(RoleEnum.Infectious) || PlayerControl.LocalPlayer.Is(RoleEnum.Doppelganger))
            {
                __instance.lightSource.SetupLightingForGameplay(hasFlashlight, CustomGameOptions.ImpostorFlashlightVision, __instance.TargetFlashlight.transform);
            }
            else if (PlayerControl.LocalPlayer.Is(RoleEnum.Werewolf))
            {
                var role = Role.GetRole<Werewolf>(PlayerControl.LocalPlayer);
                if (role.Rampaged)
                {
                    __instance.lightSource.SetupLightingForGameplay(hasFlashlight, CustomGameOptions.ImpostorFlashlightVision, __instance.TargetFlashlight.transform);
                }
                else
                {
                    __instance.lightSource.SetupLightingForGameplay(hasFlashlight, CustomGameOptions.CrewmateFlashlightVision, __instance.TargetFlashlight.transform);
                }
            }
            else if (PlayerControl.LocalPlayer.Is(RoleEnum.Mutant))
            {
                var role = Role.GetRole<Mutant>(PlayerControl.LocalPlayer);
                if (role.IsTransformed == true)
                {
                    __instance.lightSource.SetupLightingForGameplay(hasFlashlight, CustomGameOptions.ImpostorFlashlightVision, __instance.TargetFlashlight.transform);
                }
                else
                {
                    __instance.lightSource.SetupLightingForGameplay(hasFlashlight, CustomGameOptions.CrewmateFlashlightVision, __instance.TargetFlashlight.transform);
                }
            }
            
            else
            {
                __instance.lightSource.SetupLightingForGameplay(hasFlashlight, CustomGameOptions.CrewmateFlashlightVision, __instance.TargetFlashlight.transform);
            }

            return false;
        }
    }
}