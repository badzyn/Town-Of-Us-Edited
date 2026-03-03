using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using TownOfUsEdited.Extensions;
using TownOfUsEdited.Patches;
using TownOfUsEdited.Roles;
using UnityEngine;
using Object = UnityEngine.Object;

namespace TownOfUsEdited.CrewmateRoles.ImitatorMod
{
    [HarmonyPatch(typeof(AirshipExileController._WrapUpAndSpawn_d__11), nameof(AirshipExileController._WrapUpAndSpawn_d__11.MoveNext))]
    public static class AirshipExileController_WrapUpAndSpawn
    {
        public static void Postfix(AirshipExileController._WrapUpAndSpawn_d__11 __instance) => StartImitate.ExileControllerPostfix(__instance.__4__this);
    }

    [HarmonyPatch(typeof(ExileController), nameof(ExileController.WrapUp))]
    public class StartImitate
    {
        public static List<byte> ImitatingPlayers = new List<byte>();
        public static void ExileControllerPostfix(ExileController __instance)
        {
            var exiled = __instance.initData?.networkedPlayer?.Object;
            if (!PlayerControl.LocalPlayer.Is(RoleEnum.Imitator)) return;
            if (PlayerControl.LocalPlayer.Data.IsDead || PlayerControl.LocalPlayer.Data.Disconnected) return;
            if (exiled == PlayerControl.LocalPlayer) return;

            var imitator = Role.GetRole<Imitator>(PlayerControl.LocalPlayer);
            if (imitator.ImitatePlayer == null || imitator.ImitatePlayer.Is(RoleEnum.Imitator)) return;

            Imitate(imitator);

            Utils.Rpc(CustomRPC.StartImitate, imitator.Player.PlayerId);
        }

        public static void Postfix(ExileController __instance) => ExileControllerPostfix(__instance);

        [HarmonyPatch(typeof(Object), nameof(Object.Destroy), new Type[] { typeof(GameObject) })]
        public static void Prefix(GameObject obj)
        {
            if (!SubmergedCompatibility.Loaded || GameOptionsManager.Instance?.currentNormalGameOptions?.MapId != 6) return;
            if (obj.name?.Contains("ExileCutscene") == true) ExileControllerPostfix(ExileControllerPatch.lastExiled);
        }

        public static void Imitate(Imitator imitator)
        {
            bool isMadmate = false;
            if (imitator.Player.Is(Faction.Madmates)) isMadmate = true;
            if (imitator.ImitatePlayer == null) return;
            var imi = imitator.Player;
            ImitatingPlayers.Add(imi.PlayerId);
            var imitatorRole = Role.GetRole(imitator.ImitatePlayer).RoleType;
            if (imitatorRole == RoleEnum.Haunter)
            {
                var haunter = Role.GetRole<Haunter>(imitator.ImitatePlayer);
                imitatorRole = haunter.formerRole;
            }
            if (imitatorRole == RoleEnum.Helper)
            {
                var helper = Role.GetRole<Helper>(imitator.ImitatePlayer);
                imitatorRole = helper.formerRole;
            }
            if (imitatorRole == RoleEnum.Guardian)
            {
                var guardian = Role.GetRole<Guardian>(imitator.ImitatePlayer);
                imitatorRole = guardian.formerRole;
            }
            var role = Role.GetRole(imi);
            var killsList = (role.Kills, role.CorrectKills, role.IncorrectKills, role.CorrectAssassinKills, role.IncorrectAssassinKills);
            Role.RoleDictionary.Remove(imi.PlayerId);
            if (imitatorRole == RoleEnum.Detective) new Detective(imi);
            if (imitatorRole == RoleEnum.Captain) new Captain(imi);
            if (imitatorRole == RoleEnum.Doctor) new Doctor(imi);
            if (imitatorRole == RoleEnum.TimeLord) new TimeLord(imi);
            if (imitatorRole == RoleEnum.Avenger) new Avenger(imi);
            if (imitatorRole == RoleEnum.Investigator) new Investigator(imi);
            if (imitatorRole == RoleEnum.Mystic) new Mystic(imi);
            if (imitatorRole == RoleEnum.Seer) new Seer(imi);
            if (imitatorRole == RoleEnum.Spy) new Spy(imi);
            if (imitatorRole == RoleEnum.Tracker) new Tracker(imi);
            if (imitatorRole == RoleEnum.Sheriff) new Sheriff(imi);
            if (imitatorRole == RoleEnum.Veteran) new Veteran(imi);
            if (imitatorRole == RoleEnum.Astral) new Astral(imi);
            if (imitatorRole == RoleEnum.Altruist) new Altruist(imi);
            if (imitatorRole == RoleEnum.Engineer) new Engineer(imi);
            if (imitatorRole == RoleEnum.Medium) new Medium(imi);
            if (imitatorRole == RoleEnum.Transporter) new Transporter(imi);
            if (imitatorRole == RoleEnum.Informant) new Informant(imi);
            if (imitatorRole == RoleEnum.Trapper) new Trapper(imi);
            if (imitatorRole == RoleEnum.Oracle) new Oracle(imi);
            if (imitatorRole == RoleEnum.Aurial) new Aurial(imi);
            if (imitatorRole == RoleEnum.Crewmate) new Crewmate(imi);
            else if (imitatorRole == RoleEnum.Lookout) new Lookout(imi);
            else if (imitatorRole == RoleEnum.Watcher) new Watcher(imi);
            else if (imitatorRole == RoleEnum.Crusader) new Crusader(imi);
            else if (imitatorRole == RoleEnum.Paranoïac) new Paranoïac(imi);
            else if (imitatorRole == RoleEnum.Chameleon) new Chameleon(imi);
            else if (imitatorRole == RoleEnum.Bodyguard) new Bodyguard(imi);
            else if (imitatorRole == RoleEnum.Fighter) new Fighter(imi);


            else if (imitatorRole == RoleEnum.Snitch)
            {
                var snitch = new Snitch(imi);
                var taskinfos = imi.Data.Tasks.ToArray();
                var tasksLeft = taskinfos.Count(x => !x.Complete);
                if (tasksLeft <= CustomGameOptions.SnitchTasksRemaining && ((PlayerControl.LocalPlayer.Data.IsImpostor() && (Role.GetRole(PlayerControl.LocalPlayer).formerRole == RoleEnum.None || CustomGameOptions.SnitchSeesTraitor))
                            || (PlayerControl.LocalPlayer.Is(Faction.NeutralKilling) && CustomGameOptions.SnitchSeesNeutrals)))
                {
                    var gameObj = new GameObject();
                    var arrow = gameObj.AddComponent<ArrowBehaviour>();
                    gameObj.transform.parent = PlayerControl.LocalPlayer.gameObject.transform;
                    var renderer = gameObj.AddComponent<SpriteRenderer>();
                    renderer.sprite = TownOfUsEdited.Arrow;
                    arrow.image = renderer;
                    gameObj.layer = 5;
                    snitch.ImpArrows.Add(arrow);
                }
                else if (tasksLeft == 0 && PlayerControl.LocalPlayer == imi)
                {
                    var impostors = PlayerControl.AllPlayerControls.ToArray().Where(x => x.Data.IsImpostor());
                    foreach (var imp in impostors)
                    {
                        if (!imp.Is(RoleEnum.Traitor) || CustomGameOptions.SnitchSeesTraitor)
                        {
                            var gameObj = new GameObject();
                            var arrow = gameObj.AddComponent<ArrowBehaviour>();
                            gameObj.transform.parent = PlayerControl.LocalPlayer.gameObject.transform;
                            var renderer = gameObj.AddComponent<SpriteRenderer>();
                            renderer.sprite = TownOfUsEdited.Arrow;
                            arrow.image = renderer;
                            gameObj.layer = 5;
                            snitch.SnitchArrows.Add(imp.PlayerId, arrow);
                        }
                    }
                }
            }
            else if (imitatorRole == RoleEnum.Deputy) new Deputy(imi);
            else if (imitatorRole == RoleEnum.Hunter) new Hunter(imi);
            else if (imitatorRole == RoleEnum.Jailor) new Jailor(imi);
            else if (imitatorRole == RoleEnum.Vigilante) new Vigilante(imi);
            else if (imitatorRole == RoleEnum.Warden) new Warden(imi);
            else if (imitatorRole == RoleEnum.President) new President(imi);
            else if (imitatorRole == RoleEnum.Prosecutor) new Prosecutor(imi);
            else if (imitatorRole == RoleEnum.Swapper) new Swapper(imi);
            if (imitatorRole == RoleEnum.Medic)
            {
                var medic = new Medic(imi);
                medic.StartingCooldown = medic.StartingCooldown.AddSeconds(-10f);
            }
            if (imitatorRole == RoleEnum.VampireHunter)
            {
                var vh = new VampireHunter(imi);
                vh.UsesLeft = CustomGameOptions.MaxFailedStakesPerGame;
                vh.AddedStakes = true;
            }
            if (imitatorRole == RoleEnum.Knight)
            {
                var knightRole = new Knight(imi);
                knightRole.Cooldown = CustomGameOptions.KnightKCD;
                knightRole.UsesLeft = 0;
            }

            var newRole = Role.GetRole(imi);
            newRole.RemoveFromRoleHistory(newRole.RoleType);
            newRole.Kills = killsList.Kills;
            newRole.CorrectKills = killsList.CorrectKills;
            newRole.IncorrectKills = killsList.IncorrectKills;
            newRole.CorrectAssassinKills = killsList.CorrectAssassinKills;
            newRole.IncorrectAssassinKills = killsList.IncorrectAssassinKills;
            newRole.DeathReason = role.DeathReason;
            if (isMadmate) Utils.TurnMadmate(imitator.Player, false);
        }
    }
}