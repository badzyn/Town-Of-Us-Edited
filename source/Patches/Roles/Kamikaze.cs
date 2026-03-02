using Steamworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TownOfUsEdited.CrewmateRoles.ClericMod;
using TownOfUsEdited.CrewmateRoles.MedicMod;
using TownOfUsEdited.ImpostorRoles.KamikazeMod;
using TownOfUsEdited.Patches;
using TownOfUsEdited.Roles.Modifiers;
using UnityEngine;

namespace TownOfUsEdited.Roles
{
    public class Kamikaze : Role
    {
        public KillButton _plantButton;
        public float TimeRemaining;
        public bool Enabled = false;
        public bool Detonated = true;
        public Vector3 DetonatePoint;
        public Bomb Bomb = new Bomb();
        public static Material bombMaterial = TownOfUsEdited.bundledAssets.Get<Material>("bomb");
        public DateTime StartingCooldown { get; set; }

        public Kamikaze(PlayerControl player) : base(player)
        {
            Name = "Kamikaze";
            ImpostorText = () => "Sacrifice For Greater Purpose";
            TaskText = () => "Kill crewmates and sacrifice yourself at good moment";
            Color = Palette.ImpostorRed;
            StartingCooldown = DateTime.UtcNow;
            RoleType = RoleEnum.Kamikaze;
            AddToRoleHistory(RoleType);
            Faction = Faction.Impostors;
            Alignment = Alignment.ImpostorKilling;
        }
        public KillButton PlantButton
        {
            get => _plantButton;
            set
            {
                _plantButton = value;
                ExtraButtons.Clear();
                ExtraButtons.Add(value);
            }
        }
        public float StartTimer()
        {
            var utcNow = DateTime.UtcNow;
            var timeSpan = utcNow - StartingCooldown;
            var num = 10000f;
            var flag2 = num - (float)timeSpan.TotalMilliseconds < 0f;
            if (flag2) return 0;
            return (num - (float)timeSpan.TotalMilliseconds) / 1000f;
        }
        public bool Detonating => TimeRemaining > 0f;
        public void DetonateTimer()
        {
            Enabled = true;
            TimeRemaining -= Time.deltaTime;
            if (MeetingHud.Instance) Detonated = true;
            if(TimeRemaining <= 0 && !Detonated)
            {
                var kami = GetRole<Kamikaze>(PlayerControl.LocalPlayer);
                kami.Bomb.ClearBomb();
                DetonateKillStart();
            }
        }
        public void DetonateKillStart()
        {
            Detonated = true;
            var playersToDie = Utils.GetClosestPlayers(DetonatePoint, CustomGameOptions.DetonateRadius, false);
            playersToDie = Shuffle(playersToDie);
            while (playersToDie.Count > CustomGameOptions.KamikazeMaxKillInDetonation) playersToDie.Remove(playersToDie[playersToDie.Count - 1]);
            foreach (var player in playersToDie)
            {
                if(!player.Is(RoleEnum.Pestilence) && !player.IsShielded() && !player.IsProtected() && !player.IsBarriered() && player != ShowShield.FirstRoundShielded)
                {
                    Utils.RpcMultiMurderPlayer(Player, player);
                }
                else if (player.IsShielded())
                {
                    foreach(var medic in player.GetMedic())
                    {
                        Utils.Rpc(CustomRPC.AttemptSound, medic.Player.PlayerId, player.PlayerId);
                        StopKill.BreakShield(medic.Player.PlayerId, player.PlayerId, CustomGameOptions.ShieldBreaks);
                    }
                }
                else if (player.IsBarriered())
                {
                    foreach(var cleric in player.GetCleric())
                    {
                        StopAttack.NotifyCleric(cleric.Player.PlayerId, false);
                    }
                }

            }
            if (PlayerControl.LocalPlayer.Is(ModifierEnum.Bloodlust) && playersToDie.Count > 0)
            {
                var modifier = Modifier.GetModifier<Bloodlust>(PlayerControl.LocalPlayer);
                var diedPlayers = playersToDie.Count;
                while (diedPlayers > 0)
                {
                    modifier.KilledThisRound++;
                    diedPlayers--;
                }
            }
        }
        public static Il2CppSystem.Collections.Generic.List<PlayerControl> Shuffle(Il2CppSystem.Collections.Generic.List<PlayerControl> playersToDie)
        {
            var count = playersToDie.Count;
            var last = count - 1;
            for (var i = 0; i < last; ++i)
            {
                var r = UnityEngine.Random.Range(i, count);
                var tmp = playersToDie[i];
                playersToDie[i] = playersToDie[r];
                playersToDie[r] = tmp;
            }
            return playersToDie;
        }
    }
}
