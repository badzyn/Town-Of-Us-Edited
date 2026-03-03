using HarmonyLib;
using Hazel;
using TownOfUsEdited.Extensions;
using TownOfUsEdited.NeutralRoles.ExecutionerMod;
using TownOfUsEdited.NeutralRoles.GuardianAngelMod;
using TownOfUsEdited.Roles;
using TownOfUsEdited.Roles.Modifiers;
using UnityEngine;
using Assassin = TownOfUsEdited.Roles.Modifiers.Assassin;
using Object = UnityEngine.Object;

namespace TownOfUsEdited.Patches
{
    [HarmonyPatch(typeof(IntroCutscene._CoBegin_d__35), nameof(IntroCutscene._CoBegin_d__35.MoveNext))]
    public static class Start
    {
        public static void Postfix(IntroCutscene._CoBegin_d__35 __instance)
        {
            HudUpdate.Zooming = false;
            HudUpdate.ZoomStart();

            if (CustomGameOptions.GameMode == GameMode.Chaos)
            {
                foreach (var player in PlayerControl.AllPlayerControls)
                {
                    if (player.Data.IsImpostor())
                    {
                        player.MyPhysics.SetBodyType(PlayerBodyTypes.Seeker);
                    }
                }
            }

            foreach (var player in PlayerControl.AllPlayerControls)
            {
                if (player.Is(ModifierEnum.Mini) && player.transform.localPosition.y > 4 && GameOptionsManager.Instance.currentNormalGameOptions.MapId == 1)
                {
                    player.transform.localPosition = new Vector3(player.transform.localPosition.x, 4f, player.transform.localPosition.z);
                }
            }

            if (PlayerControl.LocalPlayer.Is(RoleEnum.BountyHunter))
            {
                var bh = Role.GetRole<BountyHunter>(PlayerControl.LocalPlayer);
                bh.BountyTarget = null; //Forces to reset the target
            }

            if (PlayerControl.LocalPlayer.Is(RoleEnum.TimeLord))
            {
                var tl = Role.GetRole<TimeLord>(PlayerControl.LocalPlayer);
                tl.Cooldown = CustomGameOptions.InitialCooldowns;
            }

            if (PlayerControl.LocalPlayer.Is(RoleEnum.Captain))
            {
                var cap = Role.GetRole<Captain>(PlayerControl.LocalPlayer);
                cap.Cooldown = CustomGameOptions.InitialCooldowns;
            }

            if (PlayerControl.LocalPlayer.Is(RoleEnum.Plumber))
            {
                var plumber = Role.GetRole<Plumber>(PlayerControl.LocalPlayer);
                plumber.Cooldown = CustomGameOptions.InitialCooldowns;
            }

            if (PlayerControl.LocalPlayer.Is(RoleEnum.Player))
            {
                var playerRole = Role.GetRole<Player>(PlayerControl.LocalPlayer);
                playerRole.Cooldown = CustomGameOptions.BattleRoyaleStartingCD;
            }

            if (PlayerControl.LocalPlayer.Is(RoleEnum.Jailor))
            {
                var jailor = Role.GetRole<Jailor>(PlayerControl.LocalPlayer);
                jailor.Cooldown = CustomGameOptions.InitialCooldowns;
            }

            if (PlayerControl.LocalPlayer.Is(RoleEnum.Doctor))
            {
                var doc = Role.GetRole<Doctor>(PlayerControl.LocalPlayer);
                doc.Cooldown = CustomGameOptions.InitialCooldowns;
            }

            if (PlayerControl.LocalPlayer.Is(RoleEnum.Crusader))
            {
                var crus = Role.GetRole<Crusader>(PlayerControl.LocalPlayer);
                crus.Cooldown = CustomGameOptions.InitialCooldowns;
            }

            if (PlayerControl.LocalPlayer.Is(RoleEnum.Detective))
            {
                var detective = Role.GetRole<Detective>(PlayerControl.LocalPlayer);
                detective.Cooldown = CustomGameOptions.InitialCooldowns;
            }

            if (PlayerControl.LocalPlayer.Is(RoleEnum.Medium))
            {
                var medium = Role.GetRole<Medium>(PlayerControl.LocalPlayer);
                medium.Cooldown = CustomGameOptions.InitialCooldowns;
            }

            if (PlayerControl.LocalPlayer.Is(RoleEnum.Seer))
            {
                var seer = Role.GetRole<Seer>(PlayerControl.LocalPlayer);
                seer.Cooldown = CustomGameOptions.InitialCooldowns;
            }

            if (PlayerControl.LocalPlayer.Is(ModifierEnum.Drunk))
            {
                var drunk = Modifier.GetModifier<Drunk>(PlayerControl.LocalPlayer);
                drunk.RoundsLeft = CustomGameOptions.DrunkDuration;
            }

            if (PlayerControl.LocalPlayer.Is(RoleEnum.Oracle))
            {
                var oracle = Role.GetRole<Oracle>(PlayerControl.LocalPlayer);
                oracle.Cooldown = CustomGameOptions.InitialCooldowns;
                oracle.BlessCooldown = CustomGameOptions.InitialCooldowns;
            }

            if (PlayerControl.LocalPlayer.Is(RoleEnum.SoulCollector))
            {
                var sc = Role.GetRole<SoulCollector>(PlayerControl.LocalPlayer);
                sc.Cooldown = CustomGameOptions.InitialCooldowns;
            }

            if (PlayerControl.LocalPlayer.Is(RoleEnum.Sheriff))
            {
                var sheriff = Role.GetRole<Sheriff>(PlayerControl.LocalPlayer);
                sheriff.Cooldown = CustomGameOptions.InitialCooldowns;
            }

            if (PlayerControl.LocalPlayer.Is(RoleEnum.Knight))
            {
                var knight = Role.GetRole<Knight>(PlayerControl.LocalPlayer);
                knight.Cooldown = CustomGameOptions.InitialCooldowns;
            }

            if (PlayerControl.LocalPlayer.Is(RoleEnum.Fighter))
            {
                var fighter = Role.GetRole<Fighter>(PlayerControl.LocalPlayer);
                fighter.Cooldown = CustomGameOptions.InitialCooldowns;
            }

            if (PlayerControl.LocalPlayer.Is(RoleEnum.Tracker))
            {
                var tracker = Role.GetRole<Tracker>(PlayerControl.LocalPlayer);
                tracker.Cooldown = CustomGameOptions.InitialCooldowns;
            }

            if (PlayerControl.LocalPlayer.Is(RoleEnum.Hunter))
            {
                var hunter = Role.GetRole<Hunter>(PlayerControl.LocalPlayer);
                hunter.Cooldown = CustomGameOptions.InitialCooldowns;
                hunter.StalkCooldown = CustomGameOptions.InitialCooldowns;
            }

            if (PlayerControl.LocalPlayer.Is(RoleEnum.VampireHunter))
            {
                var vh = Role.GetRole<VampireHunter>(PlayerControl.LocalPlayer);
                vh.Cooldown = CustomGameOptions.InitialCooldowns;
            }

            if (CustomGameOptions.CanStakeRoundOne)
            {
                foreach (var vh in Role.GetRoles(RoleEnum.VampireHunter))
                {
                    var vhRole = (VampireHunter)vh;
                    vhRole.UsesLeft = CustomGameOptions.MaxFailedStakesPerGame;
                    vhRole.AddedStakes = true;
                }
            }

            if (PlayerControl.LocalPlayer.Is(RoleEnum.Cleric))
            {
                var cleric = Role.GetRole<Cleric>(PlayerControl.LocalPlayer);
                cleric.Cooldown = CustomGameOptions.InitialCooldowns;
            }

            if (PlayerControl.LocalPlayer.Is(RoleEnum.Transporter))
            {
                var transporter = Role.GetRole<Transporter>(PlayerControl.LocalPlayer);
                transporter.Cooldown = CustomGameOptions.InitialCooldowns;
            }

            if (PlayerControl.LocalPlayer.Is(RoleEnum.Trapper))
            {
                var trapper = Role.GetRole<Trapper>(PlayerControl.LocalPlayer);
                trapper.Cooldown = CustomGameOptions.InitialCooldowns;
            }

            if (PlayerControl.LocalPlayer.Is(RoleEnum.Watcher))
            {
                var watcher = Role.GetRole<Watcher>(PlayerControl.LocalPlayer);
                watcher.Cooldown = CustomGameOptions.InitialCooldowns;
            }

            if (PlayerControl.LocalPlayer.Is(RoleEnum.Veteran))
            {
                var veteran = Role.GetRole<Veteran>(PlayerControl.LocalPlayer);
                veteran.Cooldown = CustomGameOptions.InitialCooldowns;
            }

            if (PlayerControl.LocalPlayer.Is(RoleEnum.Astral))
            {
                var astral = Role.GetRole<Astral>(PlayerControl.LocalPlayer);
                astral.Cooldown = CustomGameOptions.InitialCooldowns;
            }

            if (PlayerControl.LocalPlayer.Is(RoleEnum.Lookout))
            {
                var lookout = Role.GetRole<Lookout>(PlayerControl.LocalPlayer);
                lookout.Cooldown = CustomGameOptions.InitialCooldowns;
            }

            if (PlayerControl.LocalPlayer.Is(RoleEnum.Chameleon))
            {
                var chameleon = Role.GetRole<Chameleon>(PlayerControl.LocalPlayer);
                chameleon.Cooldown = CustomGameOptions.InitialCooldowns;
            }


            if (PlayerControl.LocalPlayer.Data.IsImpostor())
            {
                Role.GetRole(PlayerControl.LocalPlayer).KillCooldown = CustomGameOptions.InitialCooldowns;
            }

            if (PlayerControl.LocalPlayer.Is(RoleEnum.Assassin) && !PlayerControl.LocalPlayer.Is(AbilityEnum.Assassin))
            {
                new Assassin(PlayerControl.LocalPlayer);
            }

            if (PlayerControl.LocalPlayer.Is(RoleEnum.Blackmailer))
            {
                var blackmailer = Role.GetRole<Blackmailer>(PlayerControl.LocalPlayer);
                blackmailer.Cooldown = CustomGameOptions.InitialCooldowns;
            }

            if (PlayerControl.LocalPlayer.Is(RoleEnum.Manipulator))
            {
                var Manipulator = Role.GetRole<Manipulator>(PlayerControl.LocalPlayer);
                Manipulator.Cooldown = CustomGameOptions.InitialCooldowns;
                Manipulator.IsManipulating = false;
                Manipulator.ManipulatedPlayer = null;
            }

            if (PlayerControl.LocalPlayer.Is(RoleEnum.Poisoner))
            {
                var poisoner = Role.GetRole<Poisoner>(PlayerControl.LocalPlayer);
                poisoner.Cooldown = CustomGameOptions.InitialCooldowns;
            }

            if (PlayerControl.LocalPlayer.Is(RoleEnum.Escapist))
            {
                var escapist = Role.GetRole<Escapist>(PlayerControl.LocalPlayer);
                escapist.Cooldown = CustomGameOptions.InitialCooldowns;
            }

            if (PlayerControl.LocalPlayer.Is(RoleEnum.Grenadier))
            {
                var grenadier = Role.GetRole<Grenadier>(PlayerControl.LocalPlayer);
                grenadier.Cooldown = CustomGameOptions.InitialCooldowns;
            }

            if (PlayerControl.LocalPlayer.Is(RoleEnum.Miner))
            {
                var miner = Role.GetRole<Miner>(PlayerControl.LocalPlayer);
                miner.Cooldown = CustomGameOptions.InitialCooldowns;
                var vents = Object.FindObjectsOfType<Vent>();
                miner.VentSize =
                    Vector2.Scale(vents[0].GetComponent<BoxCollider2D>().size, vents[0].transform.localScale) * 0.75f;
            }

            if (PlayerControl.LocalPlayer.Is(RoleEnum.Morphling))
            {
                var morphling = Role.GetRole<Morphling>(PlayerControl.LocalPlayer);
                morphling.Cooldown = CustomGameOptions.InitialCooldowns;
            }

            if (PlayerControl.LocalPlayer.Is(RoleEnum.Converter))
            {
                var converter = Role.GetRole<Converter>(PlayerControl.LocalPlayer);
                converter.Cooldown = CustomGameOptions.InitialCooldowns;
            }

            if (PlayerControl.LocalPlayer.Is(RoleEnum.Swooper))
            {
                var swooper = Role.GetRole<Swooper>(PlayerControl.LocalPlayer);
                swooper.Cooldown = CustomGameOptions.InitialCooldowns;
            }

            if (PlayerControl.LocalPlayer.Is(RoleEnum.Hypnotist))
            {
                var hypno = Role.GetRole<Hypnotist>(PlayerControl.LocalPlayer);
                hypno.Cooldown = CustomGameOptions.InitialCooldowns;
            }

            if (PlayerControl.LocalPlayer.Is(RoleEnum.Venerer))
            {
                var venerer = Role.GetRole<Venerer>(PlayerControl.LocalPlayer);
                venerer.Cooldown = CustomGameOptions.InitialCooldowns;
            }

            if (PlayerControl.LocalPlayer.Is(RoleEnum.Undertaker))
            {
                var undertaker = Role.GetRole<Undertaker>(PlayerControl.LocalPlayer);
                undertaker.Cooldown = CustomGameOptions.InitialCooldowns;
            }

            if (PlayerControl.LocalPlayer.Is(RoleEnum.Noclip))
            {
                var noclip = Role.GetRole<Noclip>(PlayerControl.LocalPlayer);
                noclip.Cooldown = CustomGameOptions.InitialCooldowns;
            }

            if (PlayerControl.LocalPlayer.Is(Faction.Coven))
            {
                Role.GetRole(PlayerControl.LocalPlayer).KillCooldown = CustomGameOptions.InitialCooldowns;
            }

            if (PlayerControl.LocalPlayer.Is(RoleEnum.HexMaster))
            {
                var hm = Role.GetRole<HexMaster>(PlayerControl.LocalPlayer);
                hm.Cooldown = CustomGameOptions.InitialCooldowns;
            }

            if (PlayerControl.LocalPlayer.Is(RoleEnum.PotionMaster))
            {
                var pm = Role.GetRole<PotionMaster>(PlayerControl.LocalPlayer);
                pm.PotionCooldown = CustomGameOptions.InitialCooldowns;
            }

            if (PlayerControl.LocalPlayer.Is(RoleEnum.Arsonist))
            {
                var arsonist = Role.GetRole<Arsonist>(PlayerControl.LocalPlayer);
                arsonist.Cooldown = CustomGameOptions.InitialCooldowns;
            }

            if (PlayerControl.LocalPlayer.Is(RoleEnum.Doomsayer))
            {
                var doomsayer = Role.GetRole<Doomsayer>(PlayerControl.LocalPlayer);
                doomsayer.Cooldown = CustomGameOptions.InitialCooldowns;
            }

            if (PlayerControl.LocalPlayer.Is(RoleEnum.Vulture))
            {
                var vulture = Role.GetRole<Vulture>(PlayerControl.LocalPlayer);
                vulture.Cooldown = CustomGameOptions.InitialCooldowns;
            }

            if (PlayerControl.LocalPlayer.Is(RoleEnum.Executioner))
            {
                var exe = Role.GetRole<Executioner>(PlayerControl.LocalPlayer);
                if (exe.target == null)
                {
                    var writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
                        254, SendOption.Reliable, -1);
                    writer.Write((int)CustomRPC.ExecutionerToJester);
                    writer.Write(exe.Player.PlayerId);
                    AmongUsClient.Instance.FinishRpcImmediately(writer);

                    TargetColor.ExeToJes(exe.Player);
                }
            }

            if (PlayerControl.LocalPlayer.Is(RoleEnum.Glitch))
            {
                var glitch = Role.GetRole<Glitch>(PlayerControl.LocalPlayer);
                glitch.MimicCooldown = CustomGameOptions.InitialCooldowns;
                glitch.HackCooldown = CustomGameOptions.InitialCooldowns;
                glitch.Cooldown = CustomGameOptions.InitialCooldowns;
            }

            if (PlayerControl.LocalPlayer.Is(RoleEnum.GuardianAngel))
            {
                var ga = Role.GetRole<GuardianAngel>(PlayerControl.LocalPlayer);
                ga.Cooldown = CustomGameOptions.InitialCooldowns;
                if (ga.target == null)
                {
                    var writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
                        254, SendOption.Reliable, -1);
                    writer.Write((int)CustomRPC.GAToSurv);
                    writer.Write(ga.Player.PlayerId);
                    AmongUsClient.Instance.FinishRpcImmediately(writer);

                    GATargetColor.GAToSurv(ga.Player);
                }
            }

            if (PlayerControl.LocalPlayer.Is(RoleEnum.Juggernaut))
            {
                var juggernaut = Role.GetRole<Juggernaut>(PlayerControl.LocalPlayer);
                juggernaut.Cooldown = CustomGameOptions.InitialCooldowns;
            }

            if (PlayerControl.LocalPlayer.Is(RoleEnum.Plaguebearer))
            {
                var plaguebearer = Role.GetRole<Plaguebearer>(PlayerControl.LocalPlayer);
                plaguebearer.Cooldown = CustomGameOptions.InitialCooldowns;
            }

            if (PlayerControl.LocalPlayer.Is(RoleEnum.Survivor))
            {
                var surv = Role.GetRole<Survivor>(PlayerControl.LocalPlayer);
                surv.Cooldown = CustomGameOptions.InitialCooldowns;
            }

            if (PlayerControl.LocalPlayer.Is(RoleEnum.Mercenary))
            {
                var merc = Role.GetRole<Mercenary>(PlayerControl.LocalPlayer);
                merc.Cooldown = CustomGameOptions.InitialCooldowns;
            }

            if (PlayerControl.LocalPlayer.Is(RoleEnum.Shifter))
            {
                var shifter = Role.GetRole<Shifter>(PlayerControl.LocalPlayer);
                shifter.Cooldown = CustomGameOptions.InitialCooldowns;
            }

            if (PlayerControl.LocalPlayer.Is(RoleEnum.Werewolf))
            {
                var werewolf = Role.GetRole<Werewolf>(PlayerControl.LocalPlayer);
                werewolf.RampageCooldown = CustomGameOptions.InitialCooldowns;
                werewolf.Cooldown = CustomGameOptions.InitialCooldowns;
            }

            if (PlayerControl.LocalPlayer.Is(RoleEnum.Vampire))
            {
                var vamp = Role.GetRole<Vampire>(PlayerControl.LocalPlayer);
                vamp.Cooldown = CustomGameOptions.InitialCooldowns;
            }

            if (PlayerControl.LocalPlayer.Is(RoleEnum.Doppelganger))
            {
                var doppel = Role.GetRole<Doppelganger>(PlayerControl.LocalPlayer);
                doppel.Cooldown = CustomGameOptions.InitialCooldowns;
            }

            if (PlayerControl.LocalPlayer.Is(RoleEnum.SerialKiller))
            {
                var sk = Role.GetRole<SerialKiller>(PlayerControl.LocalPlayer);
                sk.Cooldown = CustomGameOptions.InitialCooldowns;
                sk.ConvertCooldown = CustomGameOptions.InitialCooldowns;
                sk.Converted = false;
            }

            if (PlayerControl.LocalPlayer.Is(RoleEnum.Mutant))
            {
                var mutant = Role.GetRole<Mutant>(PlayerControl.LocalPlayer);
                mutant.Cooldown = CustomGameOptions.InitialCooldowns;
                mutant.TransformCooldown = CustomGameOptions.InitialCooldowns;
                mutant.IsTransformed = false;
            }

            if (PlayerControl.LocalPlayer.Is(RoleEnum.Infectious))
            {
                var infectious = Role.GetRole<Infectious>(PlayerControl.LocalPlayer);
                infectious.Cooldown = CustomGameOptions.InitialCooldowns;
            }

            foreach (var role in Role.GetRoles(RoleEnum.Spectator))
            {
                var spectator = (Spectator)role;
                if (!spectator.Player.Data.IsDead)
                {
                    spectator.StartSpectate(spectator.Player);
                }
            }

            PlayerControl_Die.CheckEnd();
        }
    }
}