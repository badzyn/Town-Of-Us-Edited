using Il2CppSystem.Collections.Generic;
using Reactor.Utilities;
using System.Linq;
using TownOfUsEdited.ImpostorRoles.TraitorMod;
using UnityEngine;

namespace TownOfUsEdited.Roles
{
    public class Executioner : Role
    {
        public PlayerControl target;
        public bool TargetVotedOut;

        public Executioner(PlayerControl player) : base(player)
        {
            Name = "Executioner";
            ImpostorText = () =>
                target == null ? "You don't have a target for some reason... weird..." : $"Vote {target.name} Out";
            TaskText = () =>
                target == null
                    ? "You don't have a target for some reason... weird..."
                    : $"Vote {target.name} out!\nFake Tasks:";
            Color = Patches.Colors.Executioner;
            RoleType = RoleEnum.Executioner;
            AddToRoleHistory(RoleType);
            Faction = Faction.NeutralEvil;
            Scale = 1.4f;
        }

        protected override void IntroPrefix(IntroCutscene._ShowTeam_d__38 __instance)
        {
            var exeTeam = new List<PlayerControl>();
            exeTeam.Add(PlayerControl.LocalPlayer);
            __instance.teamToShow = exeTeam;
        }

        public void Wins()
        {
            if (Player.Data.IsDead || Player.Data.Disconnected) return;
            if (AmongUsClient.Instance.NetworkMode != NetworkModes.FreePlay)
            {
                TargetVotedOut = true;
                if (AmongUsClient.Instance.AmHost && CustomGameOptions.NeutralEvilWinEndsGame)
                {
                    Coroutines.Start(WaitForEnd());
                    PluginSingleton<TownOfUsEdited>.Instance.Log.LogMessage("GAME OVER REASON: Executioner Win");
                }
            }
            else
            {
                HudManager.Instance.ShowPopUp("Normally, the game would've ended and the Executioner would've won. In Freeplay, we just assign a new target to the Executioner.");
                var exeTargets = PlayerControl.AllPlayerControls.ToArray().Where(x => x.Is(Faction.Crewmates) && !x.Is(ModifierEnum.Lover) && !x.Is(RoleEnum.Jailor) && !x.Is(RoleEnum.President) && !x.Is(RoleEnum.Prosecutor) && !x.Is(RoleEnum.Swapper) && !x.Is(RoleEnum.Vigilante) && x != SetTraitor.WillBeTraitor).ToList();
                if (exeTargets.Count > 0)
                {
                    target = exeTargets[Random.RandomRangeInt(0, exeTargets.Count)];
                }
            }
        }
    }
}