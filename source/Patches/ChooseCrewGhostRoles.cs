using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using TownOfUsEdited.CrewmateRoles.GuardianMod;
using TownOfUsEdited.CrewmateRoles.HaunterMod;
using TownOfUsEdited.CrewmateRoles.HelperMod;

namespace TownOfUsEdited.ChooseCrewGhostRoles
{
    [HarmonyPatch(typeof(ExileController), nameof(ExileController.BeginForGameplay))]
    public class PickCrewRole
    {
        public static List<string> GhostRoles = new List<string> { };
        public static bool ranlist = false;

        public static void Postfix(ExileController __instance)
        {
            var exiled = __instance.initData?.networkedPlayer?.Object;
            PickGhostRole(exiled);
        }

        public static void PickGhostRole(PlayerControl exiled)
        {
            if (CustomGameOptions.GameMode != GameMode.Classic && CustomGameOptions.GameMode != GameMode.RoleList) return;
            if (AmongUsClient.Instance.GameState != InnerNet.InnerNetClient.GameStates.Started) return;
            if (!AmongUsClient.Instance.AmHost) return;
            var random = new System.Random();
            if (RpcHandling.Check(CustomGameOptions.GuardianOn) && SetGuardian.WillBeGuardian == null && !ranlist && !GhostRoles.Contains("Guardian")) GhostRoles.Add("Guardian");
            if (RpcHandling.Check(CustomGameOptions.HelperOn) && SetHelper.WillBeHelper == null && !ranlist && !GhostRoles.Contains("Helper")) GhostRoles.Add("Helper");
            if (RpcHandling.Check(CustomGameOptions.HaunterOn) && SetHaunter.WillBeHaunter == null && !ranlist && !GhostRoles.Contains("Haunter")) GhostRoles.Add("Haunter");
            if (GhostRoles.Count <= 0)
            {
                ranlist = false;
                return;
            }
            int index = random.Next(GhostRoles.Count);
            var chosenRole = GhostRoles[index];
            GhostRoles.RemoveAt(index);
            var toChooseFromCrew = PlayerControl.AllPlayerControls.ToArray().Where(x => x.Is(Faction.Crewmates) && !x.Is(RoleEnum.President) && !x.Is(ModifierEnum.Lover) && (x.Data.IsDead || x == exiled)).ToList();
            toChooseFromCrew.RemoveAll(player => SetHaunter.WillBeHaunter == player);
            toChooseFromCrew.RemoveAll(player => SetGuardian.WillBeGuardian == player);
            toChooseFromCrew.RemoveAll(player => SetHelper.WillBeHelper == player);
            // Set the Guardian, if there is one enabled.
            if (chosenRole == "Guardian" && toChooseFromCrew.Count != 0)
            {
                var rand = UnityEngine.Random.RandomRangeInt(0, toChooseFromCrew.Count);
                var pc = toChooseFromCrew[rand];

                SetGuardian.WillBeGuardian = pc;

                Utils.Rpc(CustomRPC.SetGuardian, pc.PlayerId);
            }
            // Set the Helper, if there is one enabled.
            else if (chosenRole == "Helper" && toChooseFromCrew.Count != 0)
            {
                var rand = UnityEngine.Random.RandomRangeInt(0, toChooseFromCrew.Count);
                var pc = toChooseFromCrew[rand];

                SetHelper.WillBeHelper = pc;

                Utils.Rpc(CustomRPC.SetHelper, pc.PlayerId);
            }
            // Set the Haunter, if there is one enabled.
            else if (chosenRole == "Haunter" && toChooseFromCrew.Count != 0)
            {
                var rand = UnityEngine.Random.RandomRangeInt(0, toChooseFromCrew.Count);
                var pc = toChooseFromCrew[rand];

                SetHaunter.WillBeHaunter = pc;

                Utils.Rpc(CustomRPC.SetHaunter, pc.PlayerId);
            }
            if (GhostRoles.Count >= 1)
            {
                ranlist = true;
                GhostRoles.Clear();
                PickGhostRole(exiled);
            }
            else
            {
                ranlist = false;
                GhostRoles.Clear();
            }
        }
    }
}