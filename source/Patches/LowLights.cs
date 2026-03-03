using AmongUs.GameOptions;
using HarmonyLib;
using TownOfUsEdited.Extensions;
using TownOfUsEdited.Roles;
using UnityEngine;

namespace TownOfUsEdited
{
    [HarmonyPatch(typeof(ShipStatus), nameof(ShipStatus.CalculateLightRadius))]
    public static class LowLights
    {
        public static bool Prefix(ShipStatus __instance, [HarmonyArgument(0)] NetworkedPlayerInfo player,
            ref float __result)
        {
            if (GameOptionsManager.Instance.CurrentGameOptions.GameMode == GameModes.HideNSeek)
            {
                if (GameOptionsManager.Instance.currentHideNSeekGameOptions.useFlashlight)
                {
                    if (player.IsImpostor()) __result = __instance.MaxLightRadius * GameOptionsManager.Instance.currentHideNSeekGameOptions.ImpostorFlashlightSize;
                    else __result = __instance.MaxLightRadius * GameOptionsManager.Instance.currentHideNSeekGameOptions.CrewmateFlashlightSize;
                }
                else
                {
                    if (player.IsImpostor()) __result = __instance.MaxLightRadius * GameOptionsManager.Instance.currentHideNSeekGameOptions.ImpostorLightMod;
                    else __result = __instance.MaxLightRadius * GameOptionsManager.Instance.currentHideNSeekGameOptions.CrewLightMod;
                }
                return false;
            }

            if (player == null || player.IsDead)
            {
                __result = __instance.MaxLightRadius;
                return false;
            }

            if (player._object.IsBlinded())
            {
                __result = 0f;
                return false;
            }

            var switchSystem = (GameOptionsManager.Instance.currentNormalGameOptions.MapId == 5 || (AmongUsClient.Instance.NetworkMode == NetworkModes.FreePlay && AmongUsClient.Instance.TutorialMapId == 5)) ? null : __instance.Systems[SystemTypes.Electrical]?.TryCast<SwitchSystem>();
            var t = switchSystem != null ? switchSystem.Value / 255f : 1;

            var playerRole = Role.GetRole(player._object);
            foreach (var infectious in Role.GetRoles(RoleEnum.Infectious))
            {
                var infectiousRole = (Infectious)infectious;
                if (playerRole != null && playerRole.InfectionState > 2 && infectiousRole.Infected.Contains(player.PlayerId)
                && !infectiousRole.Player.Data.IsDead && !player._object.Is(RoleEnum.Infectious))
                {
                    if (!player._object.Is(Faction.Crewmates))
                    {
                        __result = __instance.MaxLightRadius * GameOptionsManager.Instance.currentNormalGameOptions.CrewLightMod / 2;
                    }
                    
                    else
                    {
                        __result = Mathf.Lerp(__instance.MinLightRadius, __instance.MaxLightRadius / 2, t) *
                            GameOptionsManager.Instance.currentNormalGameOptions.CrewLightMod;
                    }
                    return false;
                }
            }

            if (player.IsImpostor() || player._object.Is(RoleEnum.Glitch) ||
                player._object.Is(RoleEnum.Juggernaut) || player._object.Is(RoleEnum.Pestilence) ||
                (player._object.Is(RoleEnum.Jester) && CustomGameOptions.JesterImpVision) ||
                (player._object.Is(RoleEnum.Vulture) && CustomGameOptions.VultureImpVision) ||
                player._object.Is(RoleEnum.Arsonist) || player._object.Is(RoleEnum.SoulCollector) ||
                (player._object.Is(RoleEnum.Vampire) && CustomGameOptions.VampImpVision) ||
                (player._object.Is(RoleEnum.SerialKiller) && CustomGameOptions.SkImpVision) ||
                (player._object.Is(Faction.Madmates) && CustomGameOptions.MadmateHasImpoVision) ||
                player._object.Is(RoleEnum.Player) || player._object.Is(RoleEnum.Terrorist) ||
                player._object.Is(Faction.Coven) || player._object.Is(RoleEnum.Infectious) ||
                player._object.Is(RoleEnum.Doppelganger) || player._object.Is(ModifierEnum.Torch))
            {
                __result = __instance.MaxLightRadius * GameOptionsManager.Instance.currentNormalGameOptions.ImpostorLightMod;
                return false;
            }
            else if (player._object.Is(RoleEnum.Werewolf))
            {
                var role = Role.GetRole<Werewolf>(player._object);
                if (role.Rampaged)
                {
                    __result = __instance.MaxLightRadius * GameOptionsManager.Instance.currentNormalGameOptions.ImpostorLightMod;
                    return false;
                }
            }

            else if (player._object.Is(RoleEnum.Mutant))
            {
                var role = Role.GetRole<Mutant>(player._object);
                if (role.IsTransformed == true)
                {
                    __result = __instance.MaxLightRadius * GameOptionsManager.Instance.currentNormalGameOptions.ImpostorLightMod;
                    return false;
                }
            }

            

            __result = Mathf.Lerp(__instance.MinLightRadius, __instance.MaxLightRadius, t) *
                       GameOptionsManager.Instance.currentNormalGameOptions.CrewLightMod;
            return false;
        }
    }
}