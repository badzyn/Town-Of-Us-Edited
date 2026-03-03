using HarmonyLib;
using UnityEngine;

namespace TownOfUsEdited.Patches.Modifiers.MadmateMod
{
    [HarmonyPatch(typeof(HudManager), nameof(HudManager.Update))]
    public class HudManagerUpdate
    {
        public static void Prefix(HudManager __instance)
        {
            if (PlayerControl.AllPlayerControls.Count <= 1) return;
            if (PlayerControl.LocalPlayer == null) return;
            if (PlayerControl.LocalPlayer.Data == null) return;
            if (!PlayerControl.LocalPlayer.Is(Faction.Madmates)) return;
            var flag = PlayerControl.LocalPlayer.Is(RoleEnum.Crewmate) || PlayerControl.LocalPlayer.Is(RoleEnum.Imitator) || PlayerControl.LocalPlayer.Is(RoleEnum.Investigator) ||
            PlayerControl.LocalPlayer.Is(RoleEnum.President) || PlayerControl.LocalPlayer.Is(RoleEnum.Mystic) ||
            PlayerControl.LocalPlayer.Is(RoleEnum.Prosecutor) || PlayerControl.LocalPlayer.Is(RoleEnum.Snitch) ||
            PlayerControl.LocalPlayer.Is(RoleEnum.Spy) || PlayerControl.LocalPlayer.Is(RoleEnum.Swapper) ||
            PlayerControl.LocalPlayer.Is(RoleEnum.Vigilante) || PlayerControl.LocalPlayer.Is(RoleEnum.Bodyguard);
            if (flag)
            {
                var position = __instance.KillButton.transform.localPosition;
                __instance.ImpostorVentButton.transform.localPosition = new Vector3(position.x,
                position.y, position.z);
            }
            else if (!PlayerControl.LocalPlayer.Is(RoleEnum.Engineer) && !PlayerControl.LocalPlayer.Is(RoleEnum.Doctor) && !PlayerControl.LocalPlayer.Is(RoleEnum.Paranoïac)
            && !PlayerControl.LocalPlayer.Is(RoleEnum.Plumber))
            {
                __instance.ImpostorVentButton.transform.localPosition = new Vector3(-1f, 1f, 0f);
            }
        }
    }
}