using Reactor.Utilities;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using TownOfUsEdited.CrewmateRoles.VampireHunterMod;
using TownOfUsEdited.NeutralRoles.ExecutionerMod;
using TownOfUsEdited.NeutralRoles.GuardianAngelMod;
using TownOfUsEdited.Patches;
using UnityEngine;
using static TownOfUsEdited.Roles.Modifiers.Madmate;

namespace TownOfUsEdited.Roles
{
    public class Doomsayer : Role, IGuesser
    {
        public Dictionary<byte, (GameObject, GameObject, GameObject, TMP_Text)> Buttons { get; set; } = new();

        private Dictionary<string, Color> ColorMapping = new();

        public Dictionary<string, Color> SortedColorMapping;

        public Dictionary<byte, string> Guesses = new();
        public float Cooldown;
        public bool coolingDown => Cooldown > 0f;
        public PlayerControl ClosestPlayer;
        public PlayerControl LastObservedPlayer;

        public Doomsayer(PlayerControl player) : base(player)
        {
            Name = "Doomsayer";
            ImpostorText = () => "Guess People's Roles To Win!";
            TaskText = () => "Win by guessing player's roles\nFake Tasks:";
            Color = Patches.Colors.Doomsayer;
            RoleType = RoleEnum.Doomsayer;
            Cooldown = CustomGameOptions.ObserveCooldown;
            AddToRoleHistory(RoleType);
            Faction = Faction.NeutralEvil;

            ColorMapping.Add("Crewmate", Colors.Crewmate);
            if (CustomGameOptions.ChameleonOn > 0) ColorMapping.Add("Chameleon", Colors.Chameleon);
            if (CustomGameOptions.SheriffOn > 0 || (CustomGameOptions.VampireHunterOn > 0 && CustomGameOptions.BecomeOnVampDeaths == BecomeEnum.Sheriff)) ColorMapping.Add("Sheriff", Colors.Sheriff);
            if (CustomGameOptions.KnightOn > 0) ColorMapping.Add("Knight", Colors.Knight);
            if (CustomGameOptions.FighterOn > 0) ColorMapping.Add("Fighter", Colors.Fighter);
            if (CustomGameOptions.JailorOn > 0) ColorMapping.Add("Jailor", Colors.Jailor);
            if (CustomGameOptions.DeputyOn > 0) ColorMapping.Add("Deputy", Colors.Deputy);
            if (CustomGameOptions.EngineerOn > 0) ColorMapping.Add("Engineer", Colors.Engineer);
            if (CustomGameOptions.InformantOn > 0) ColorMapping.Add("Informant", Colors.Informant);
            if (CustomGameOptions.SwapperOn > 0) ColorMapping.Add("Swapper", Colors.Swapper);
            if (CustomGameOptions.AvengerOn > 0) ColorMapping.Add("Avenger", Colors.Avenger);
            if (CustomGameOptions.InvestigatorOn > 0) ColorMapping.Add("Investigator", Colors.Investigator);
            if (CustomGameOptions.MedicOn > 0) ColorMapping.Add("Medic", Colors.Medic);
            if (CustomGameOptions.AstralOn > 0) ColorMapping.Add("Astral", Colors.Astral);
            if (CustomGameOptions.LookoutOn > 0) ColorMapping.Add("Lookout", Colors.Lookout);
            if (CustomGameOptions.SeerOn > 0) ColorMapping.Add("Seer", Colors.Seer);
            if (CustomGameOptions.SpyOn > 0) ColorMapping.Add("Spy", Colors.Spy);
            if (CustomGameOptions.SnitchOn > 0) ColorMapping.Add("Snitch", Colors.Snitch);
            if (CustomGameOptions.AltruistOn > 0) ColorMapping.Add("Altruist", Colors.Altruist);
            if (CustomGameOptions.VigilanteOn > 0 || (CustomGameOptions.VampireHunterOn > 0 && CustomGameOptions.BecomeOnVampDeaths == BecomeEnum.Vigilante)) ColorMapping.Add("Vigilante", Colors.Vigilante);
            if (CustomGameOptions.VeteranOn > 0 || (CustomGameOptions.VampireHunterOn > 0 && CustomGameOptions.BecomeOnVampDeaths == BecomeEnum.Veteran)) ColorMapping.Add("Veteran", Colors.Veteran);
            if (CustomGameOptions.HunterOn > 0 || (CustomGameOptions.VampireHunterOn > 0 && CustomGameOptions.BecomeOnVampDeaths == BecomeEnum.Hunter)) ColorMapping.Add("Hunter", Colors.Hunter);
            if (CustomGameOptions.TrackerOn > 0) ColorMapping.Add("Tracker", Colors.Tracker);
            if (CustomGameOptions.TrapperOn > 0) ColorMapping.Add("Trapper", Colors.Trapper);
            if (CustomGameOptions.TransporterOn > 0) ColorMapping.Add("Transporter", Colors.Transporter);
            if (CustomGameOptions.MediumOn > 0) ColorMapping.Add("Medium", Colors.Medium);
            if (CustomGameOptions.MysticOn > 0) ColorMapping.Add("Mystic", Colors.Mystic);
            if (CustomGameOptions.CaptainOn > 0) ColorMapping.Add("Captain", Colors.Captain);
            if (CustomGameOptions.DoctorOn > 0) ColorMapping.Add("Doctor", Colors.Doctor);
            if (CustomGameOptions.TimeLordOn > 0) ColorMapping.Add("Time Lord", Colors.TimeLord);
            if (CustomGameOptions.CrusaderOn > 0) ColorMapping.Add("Crusader", Colors.Crusader);
            if (CustomGameOptions.BodyguardOn > 0) ColorMapping.Add("Bodyguard", Colors.Bodyguard);
            if (CustomGameOptions.ParanoïacOn > 0) ColorMapping.Add("Paranoïac", Colors.Paranoïac);
            if (CustomGameOptions.DetectiveOn > 0) ColorMapping.Add("Detective", Colors.Detective);
            if (CustomGameOptions.ImitatorOn > 0) ColorMapping.Add("Imitator", Colors.Imitator);
            if (CustomGameOptions.VampireHunterOn > 0) ColorMapping.Add("Vampire Hunter", Colors.VampireHunter);
            if (CustomGameOptions.ProsecutorOn > 0) ColorMapping.Add("Prosecutor", Colors.Prosecutor);
            if (CustomGameOptions.OracleOn > 0) ColorMapping.Add("Oracle", Colors.Oracle);
            if (CustomGameOptions.AurialOn > 0) ColorMapping.Add("Aurial", Colors.Aurial);
            if (CustomGameOptions.WardenOn > 0) ColorMapping.Add("Warden", Colors.Warden);
            if (CustomGameOptions.PresidentOn > 0) ColorMapping.Add("President", Colors.Mayor);
            if (CustomGameOptions.PlumberOn > 0) ColorMapping.Add("Plumber", Colors.Plumber);
            if (CustomGameOptions.ClericOn > 0) ColorMapping.Add("Cleric", Colors.Cleric);
            if (CustomGameOptions.WatcherOn > 0) ColorMapping.Add("Watcher", Colors.Lookout);
            ColorMapping.Add("Impostor", Colors.Impostor);
            if (CustomGameOptions.JanitorOn > 0) ColorMapping.Add("Janitor", Colors.Impostor);
            if (CustomGameOptions.HypnotistOn > 0) ColorMapping.Add("Hypnotist", Colors.Impostor);
            if (CustomGameOptions.WitchOn > 0) ColorMapping.Add("Witch", Colors.Impostor);
            if (CustomGameOptions.MorphlingOn > 0) ColorMapping.Add("Morphling", Colors.Impostor);
            if (CustomGameOptions.AssassinOn > 0 && CustomGameOptions.AssassinImpostorRole) ColorMapping.Add("Assassin", Colors.Impostor);
            if (CustomGameOptions.MinerOn > 0) ColorMapping.Add("Miner", Colors.Impostor);
            if (CustomGameOptions.SwooperOn > 0) ColorMapping.Add("Swooper", Colors.Impostor);
            if (CustomGameOptions.UndertakerOn > 0) ColorMapping.Add("Undertaker", Colors.Impostor);
            if (CustomGameOptions.ManipulatorOn > 0) ColorMapping.Add("Manipulator", Colors.Impostor);
            if (CustomGameOptions.EscapistOn > 0) ColorMapping.Add("Escapist", Colors.Impostor);
            if (CustomGameOptions.GrenadierOn > 0) ColorMapping.Add("Grenadier", Colors.Impostor);
            if (CustomGameOptions.TraitorOn > 0 || (CustomGameOptions.MadmateOn > 0 && CustomGameOptions.MadmateOnImpoDeath == BecomeMadmateOptions.Impostor)) ColorMapping.Add("Traitor", Colors.Impostor);
            if (CustomGameOptions.PoisonerOn > 0) ColorMapping.Add("Poisoner", Colors.Impostor);
            if (CustomGameOptions.ShooterOn > 0) ColorMapping.Add("Shooter", Colors.Impostor);
            if (CustomGameOptions.BlackmailerOn > 0) ColorMapping.Add("Blackmailer", Colors.Impostor);
            if (CustomGameOptions.ConverterOn > 0) ColorMapping.Add("Converter", Colors.Impostor);
            if (CustomGameOptions.BomberOn > 0) ColorMapping.Add("Bomber", Colors.Impostor);
            if(CustomGameOptions.KamikazeOn > 0) ColorMapping.Add("Kamikaze", Colors.Impostor);
            if (CustomGameOptions.ConjurerOn > 0) ColorMapping.Add("Conjurer", Colors.Impostor);
            if (CustomGameOptions.BountyHunterOn > 0) ColorMapping.Add("Bounty Hunter", Colors.Impostor);
            if (CustomGameOptions.WarlockOn > 0) ColorMapping.Add("Warlock", Colors.Impostor);
            if (CustomGameOptions.MafiosoOn > 0) ColorMapping.Add("Mafioso", Colors.Impostor);
            if (CustomGameOptions.ReviverOn > 0) ColorMapping.Add("Reviver", Colors.Impostor);
            if (CustomGameOptions.VenererOn > 0) ColorMapping.Add("Venerer", Colors.Impostor);
            if (CustomGameOptions.MadmateOn > 0 || CustomGameOptions.ConverterOn > 0 || CustomGameOptions.FighterOn > 0) ColorMapping.Add("Madmate", Colors.Impostor);
            if (CustomGameOptions.NoclipOn > 0) ColorMapping.Add("Noclip", Colors.Impostor);
            // Neutral Benign
            if (CustomGameOptions.AmnesiacOn > 0 || (CustomGameOptions.ExecutionerOn > 0 && CustomGameOptions.OnTargetDead == OnTargetDead.Amnesiac) || (CustomGameOptions.GuardianAngelOn > 0 && CustomGameOptions.GaOnTargetDeath == BecomeOptions.Amnesiac)) ColorMapping.Add("Amnesiac", Colors.Amnesiac);
            if (CustomGameOptions.GuardianAngelOn > 0) ColorMapping.Add("Guardian Angel", Colors.GuardianAngel);
            if (CustomGameOptions.SurvivorOn > 0 || (CustomGameOptions.ExecutionerOn > 0 && CustomGameOptions.OnTargetDead == OnTargetDead.Survivor) || (CustomGameOptions.GuardianAngelOn > 0 && CustomGameOptions.GaOnTargetDeath == BecomeOptions.Survivor)) ColorMapping.Add("Survivor", Colors.Survivor);
            if (CustomGameOptions.MercenaryOn > 0 || (CustomGameOptions.ExecutionerOn > 0 && CustomGameOptions.OnTargetDead == OnTargetDead.Mercenary) || (CustomGameOptions.GuardianAngelOn > 0 && CustomGameOptions.GaOnTargetDeath == BecomeOptions.Mercenary)) ColorMapping.Add("Mercenary", Colors.Mercenary);
            if (CustomGameOptions.ShifterOn > 0 || (CustomGameOptions.ExecutionerOn > 0 && CustomGameOptions.OnTargetDead == OnTargetDead.Shifter) || (CustomGameOptions.GuardianAngelOn > 0 && CustomGameOptions.GaOnTargetDeath == BecomeOptions.Shifter)) ColorMapping.Add("Shifter", Colors.Shifter);
            // Neutral Evil
            if (!CustomGameOptions.UniqueRoles) ColorMapping.Add("Doomsayer", Colors.Doomsayer);
            if (CustomGameOptions.ExecutionerOn > 0) ColorMapping.Add("Executioner", Colors.Executioner);
            if (CustomGameOptions.TrollOn > 0) ColorMapping.Add("Troll", Colors.Troll);
            if (CustomGameOptions.VultureOn > 0) ColorMapping.Add("Vulture", Colors.Vulture);
            if (CustomGameOptions.SoulCollectorOn > 0) ColorMapping.Add("Soul Collector", Colors.SoulCollector);
            if (CustomGameOptions.JesterOn > 0 || (CustomGameOptions.ExecutionerOn > 0 && CustomGameOptions.OnTargetDead == OnTargetDead.Jester) || (CustomGameOptions.GuardianAngelOn > 0 && CustomGameOptions.GaOnTargetDeath == BecomeOptions.Jester)) ColorMapping.Add("Jester", Colors.Jester);
            // Neutral Killing
            if (CustomGameOptions.ArsonistOn > 0) ColorMapping.Add("Arsonist", Colors.Arsonist);
            if (CustomGameOptions.SerialKillerOn > 0) ColorMapping.Add("Serial Killer", Colors.SerialKiller);
            if (CustomGameOptions.DoppelgangerOn > 0) ColorMapping.Add("Doppelganger", Colors.Doppelganger);
            if (CustomGameOptions.MutantOn > 0) ColorMapping.Add("Mutant", Colors.Mutant);
            if (CustomGameOptions.InfectiousOn > 0) ColorMapping.Add("Infectious", Colors.Infectious);
            if (CustomGameOptions.GlitchOn > 0) ColorMapping.Add("The Glitch", Colors.Glitch);
            if (CustomGameOptions.PlaguebearerOn > 0) ColorMapping.Add("Plaguebearer", Colors.Plaguebearer);
            if (CustomGameOptions.AttackerOn > 0) ColorMapping.Add("Attacker", Colors.Attacker);
            if (CustomGameOptions.VampireOn > 0) ColorMapping.Add("Vampire", Colors.Vampire);
            if (CustomGameOptions.WerewolfOn > 0) ColorMapping.Add("Maul", Colors.Werewolf);
            if (CustomGameOptions.JuggernautOn > 0) ColorMapping.Add("Juggernaut", Colors.Juggernaut);
            ColorMapping.Add("Coven", Colors.Coven);
            if (CustomGameOptions.RitualistOn > 0) ColorMapping.Add("Ritualist", Colors.Coven);
            if (CustomGameOptions.HexMasterOn > 0) ColorMapping.Add("Hex Master", Colors.Coven);
            if (CustomGameOptions.CovenLeaderOn > 0) ColorMapping.Add("Coven Leader", Colors.Coven);
            if (CustomGameOptions.SpiritualistOn > 0) ColorMapping.Add("Spiritualist", Colors.Coven);
            if (CustomGameOptions.VoodooMasterOn > 0) ColorMapping.Add("Voodoo Master", Colors.Coven);
            if (CustomGameOptions.PotionMasterOn > 0) ColorMapping.Add("Potion Master", Colors.Coven);

            SortedColorMapping = ColorMapping.OrderBy(x => x.Key).ToDictionary(x => x.Key, x => x.Value);
        }

        public float ObserveTimer()
        {
            if (!coolingDown) return 0f;
            else if (!PlayerControl.LocalPlayer.inVent)
            {
                Cooldown -= Time.deltaTime;
                return Cooldown;
            }
            else return Cooldown;
        }

        public void Wins()
        {
            if (AmongUsClient.Instance.NetworkMode != NetworkModes.FreePlay)
            {
                WonByGuessing = true;
                if (AmongUsClient.Instance.AmHost && CustomGameOptions.NeutralEvilWinEndsGame)
                {
                    Coroutines.Start(WaitForEnd());
                    PluginSingleton<TownOfUsEdited>.Instance.Log.LogMessage("GAME OVER REASON: Doomsayer Win");
                }
            }
            else
            {
                HudManager.Instance.ShowPopUp("Normally, the game would've ended and the Doomsayer would've won. In Freeplay, we just reset the guesses required to win.");
                GuessedCorrectly = 0;
            }
        }

        public int GuessedCorrectly = 0;
        public bool WonByGuessing = false;

        public List<string> PossibleGuesses => SortedColorMapping.Keys.ToList();

        protected override void IntroPrefix(IntroCutscene._ShowTeam_d__38 __instance)
        {
            var doomTeam = new Il2CppSystem.Collections.Generic.List<PlayerControl>();
            doomTeam.Add(PlayerControl.LocalPlayer);
            __instance.teamToShow = doomTeam;
        }
    }
}
