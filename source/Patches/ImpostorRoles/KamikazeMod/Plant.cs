using HarmonyLib;
using TownOfUsEdited.ImpostorRoles.BomberMod;
using TownOfUsEdited.Modifiers.UnderdogMod;
using TownOfUsEdited.Roles;
using TownOfUsEdited.Roles.Modifiers;
using UnityEngine;

namespace TownOfUsEdited.ImpostorRoles.KamikazeMod
{
    [HarmonyPatch(typeof(KillButton), nameof(KillButton.DoClick))]
    public class Plant
    {
        public static Sprite SuicideSprite => TownOfUsEdited.SuicideSprite;
        public static bool Prefix(KillButton __instance)
        {
            var flag = PlayerControl.LocalPlayer.Is(RoleEnum.Kamikaze);
            if (!flag) return true;
            if (!PlayerControl.LocalPlayer.CanMove) return false;
            if (PlayerControl.LocalPlayer.Data.IsDead) return false;
            var role = Role.GetRole<Kamikaze>(PlayerControl.LocalPlayer);
            if (role.StartTimer() > 0) return false;

            if (__instance == role.PlantButton)
            {
                var flag2 = __instance.isCoolingDown;
                if (flag2) return false;
                if (role.Player.inVent) return false;
                if (!__instance.isActiveAndEnabled) return false;
                if (role.PlantButton.graphic.sprite == SuicideSprite)
                {
                    var abilityUsed = Utils.AbilityUsed(PlayerControl.LocalPlayer);
                    if (!abilityUsed) return false;
                    role.Detonated = false;
                    var pos = PlayerControl.LocalPlayer.transform.position;
                    pos.z += 0.001f;
                    role.DetonatePoint = pos;
                    role.PlantButton.graphic.sprite = SuicideSprite;
                    role.TimeRemaining = CustomGameOptions.KamikazeDetonateDelay;
                    role.PlantButton.SetCoolDown(role.TimeRemaining, CustomGameOptions.KamikazeDetonateDelay);
                    PlayerControl.LocalPlayer.SetKillTimer(GameOptionsManager.Instance.currentNormalGameOptions.KillCooldown + CustomGameOptions.KamikazeDetonateDelay);
                    HudManager.Instance.KillButton.SetTarget(null);
                    role.Bomb = BombExtentions.CreateBomb(pos);
                    if (CustomGameOptions.KamikazeAllImpsSeeBomb) Utils.Rpc(CustomRPC.Plant, pos.x, pos.y, pos.z);
                    return false;
                }
                else return false;
            }
            return true;
        }
    }
}
