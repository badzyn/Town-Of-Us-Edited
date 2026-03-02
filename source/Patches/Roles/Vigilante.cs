using System.Collections.Generic;
using System.Linq;
using TMPro;
using TownOfUsEdited.NeutralRoles.ExecutionerMod;
using TownOfUsEdited.NeutralRoles.GuardianAngelMod;
using TownOfUsEdited.Patches;
using UnityEngine;
using static TownOfUsEdited.Roles.Modifiers.Madmate;

namespace TownOfUsEdited.Roles
{
    public class Vigilante : Role, IGuesser
    {
        public Dictionary<byte, (GameObject, GameObject, GameObject, TMP_Text)> Buttons { get; set; } = new();

        private Dictionary<string, Color> ColorMapping = new();

        public Dictionary<string, Color> SortedColorMapping;

        public Dictionary<byte, string> Guesses = new();

        public Vigilante(PlayerControl player) : base(player)
        {
            Name = "Vigilante";
            ImpostorText = () => "Kill <color=#FF0000>Impostors</color> If You Can Guess Their Roles";
            TaskText = () => "Guess the roles of <color=#FF0000>Impostors</color> mid-meeting to kill them!";
            Color = Patches.Colors.Vigilante;
            RoleType = RoleEnum.Vigilante;
            Faction = Faction.Crewmates;
            Alignment = Alignment.CrewmateKilling;
            AddToRoleHistory(RoleType);

            RemainingKills = CustomGameOptions.VigilanteKills;

            ColorMapping.Add("Impostor", Colors.Impostor);
            if (CustomGameOptions.JanitorOn > 0) ColorMapping.Add("Janitor", Colors.Impostor);
            if (CustomGameOptions.WitchOn > 0) ColorMapping.Add("Witch", Colors.Impostor);
            if (CustomGameOptions.ManipulatorOn > 0) ColorMapping.Add("Manipulator", Colors.Impostor);
            if (CustomGameOptions.ConverterOn > 0) ColorMapping.Add("Converter", Colors.Impostor);
            if (CustomGameOptions.PoisonerOn > 0) ColorMapping.Add("Poisoner", Colors.Impostor);
            if (CustomGameOptions.ShooterOn > 0) ColorMapping.Add("Shooter", Colors.Impostor);
            if (CustomGameOptions.MorphlingOn > 0) ColorMapping.Add("Morphling", Colors.Impostor);
            if (CustomGameOptions.AssassinOn > 0 && CustomGameOptions.AssassinImpostorRole) ColorMapping.Add("Assassin", Colors.Impostor);
            if (CustomGameOptions.MinerOn > 0) ColorMapping.Add("Miner", Colors.Impostor);
            if (CustomGameOptions.SwooperOn > 0) ColorMapping.Add("Swooper", Colors.Impostor);
            if (CustomGameOptions.UndertakerOn > 0) ColorMapping.Add("Undertaker", Colors.Impostor);
            if (CustomGameOptions.EscapistOn > 0) ColorMapping.Add("Escapist", Colors.Impostor);
            if (CustomGameOptions.GrenadierOn > 0) ColorMapping.Add("Grenadier", Colors.Impostor);
            if (CustomGameOptions.TraitorOn > 0 || (CustomGameOptions.MadmateOn > 0 && CustomGameOptions.MadmateOnImpoDeath == BecomeMadmateOptions.Impostor)) ColorMapping.Add("Traitor", Colors.Impostor);
            if (CustomGameOptions.BlackmailerOn > 0) ColorMapping.Add("Blackmailer", Colors.Impostor);
            if (CustomGameOptions.BomberOn > 0) ColorMapping.Add("Bomber", Colors.Impostor);
            if (CustomGameOptions.KamikazeOn > 0) ColorMapping.Add("Kamikaze", Colors.Impostor);
            if (CustomGameOptions.ConjurerOn > 0) ColorMapping.Add("Conjurer", Colors.Impostor);
            if (CustomGameOptions.BountyHunterOn > 0) ColorMapping.Add("Bounty Hunter", Colors.Impostor);
            if (CustomGameOptions.WarlockOn > 0) ColorMapping.Add("Warlock", Colors.Impostor);
            if (CustomGameOptions.MafiosoOn > 0) ColorMapping.Add("Mafioso", Colors.Impostor);
            if (CustomGameOptions.ReviverOn > 0) ColorMapping.Add("Reviver", Colors.Impostor);
            if (CustomGameOptions.VenererOn > 0) ColorMapping.Add("Venerer", Colors.Impostor);
            if (CustomGameOptions.HypnotistOn > 0) ColorMapping.Add("Hypnotist", Colors.Impostor);
            if (CustomGameOptions.MadmateOn > 0 || CustomGameOptions.ConverterOn > 0 || CustomGameOptions.FighterOn > 0) ColorMapping.Add("Madmate", Colors.Impostor);
            if (CustomGameOptions.NoclipOn > 0) ColorMapping.Add("Noclip", Colors.Impostor);

            if (CustomGameOptions.VigilanteGuessNeutralBenign)
            {
                if (CustomGameOptions.AmnesiacOn > 0 || (CustomGameOptions.ExecutionerOn > 0 && CustomGameOptions.OnTargetDead == OnTargetDead.Amnesiac) || (CustomGameOptions.GuardianAngelOn > 0 && CustomGameOptions.GaOnTargetDeath == BecomeOptions.Amnesiac)) ColorMapping.Add("Amnesiac", Colors.Amnesiac);
                if (CustomGameOptions.GuardianAngelOn > 0) ColorMapping.Add("Guardian Angel", Colors.GuardianAngel);
                if (CustomGameOptions.MercenaryOn > 0 || (CustomGameOptions.ExecutionerOn > 0 && CustomGameOptions.OnTargetDead == OnTargetDead.Mercenary) || (CustomGameOptions.GuardianAngelOn > 0 && CustomGameOptions.GaOnTargetDeath == BecomeOptions.Mercenary)) ColorMapping.Add("Mercenary", Colors.Mercenary);
                if (CustomGameOptions.SurvivorOn > 0 || (CustomGameOptions.ExecutionerOn > 0 && CustomGameOptions.OnTargetDead == OnTargetDead.Survivor) || (CustomGameOptions.GuardianAngelOn > 0 && CustomGameOptions.GaOnTargetDeath == BecomeOptions.Survivor)) ColorMapping.Add("Survivor", Colors.Survivor);
                if (CustomGameOptions.ShifterOn > 0 || (CustomGameOptions.ExecutionerOn > 0 && CustomGameOptions.OnTargetDead == OnTargetDead.Shifter) || (CustomGameOptions.GuardianAngelOn > 0 && CustomGameOptions.GaOnTargetDeath == BecomeOptions.Shifter)) ColorMapping.Add("Shifter", Colors.Shifter);
            }
            if (CustomGameOptions.VigilanteGuessNeutralEvil)
            {
                if (CustomGameOptions.DoomsayerOn > 0) ColorMapping.Add("Doomsayer", Colors.Doomsayer);
                if (CustomGameOptions.ExecutionerOn > 0) ColorMapping.Add("Executioner", Colors.Executioner);
                if (CustomGameOptions.JesterOn > 0 || (CustomGameOptions.ExecutionerOn > 0 && CustomGameOptions.OnTargetDead == OnTargetDead.Jester) || (CustomGameOptions.GuardianAngelOn > 0 && CustomGameOptions.GaOnTargetDeath == BecomeOptions.Jester)) ColorMapping.Add("Jester", Colors.Jester);
                if (CustomGameOptions.TrollOn > 0) ColorMapping.Add("Troll", Colors.Troll);
                if (CustomGameOptions.VultureOn > 0) ColorMapping.Add("Vulture", Colors.Vulture);
            }
            if (CustomGameOptions.VigilanteGuessNeutralKilling)
            {
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
                if (CustomGameOptions.SoulCollectorOn > 0) ColorMapping.Add("Soul Collector", Colors.SoulCollector);
            }
            if (CustomGameOptions.VigilanteGuessCoven)
            {
                ColorMapping.Add("Coven", Colors.Coven);
                if (CustomGameOptions.RitualistOn > 0) ColorMapping.Add("Ritualist", Colors.Coven);
                if (CustomGameOptions.HexMasterOn > 0) ColorMapping.Add("Hex Master", Colors.Coven);
                if (CustomGameOptions.CovenLeaderOn > 0) ColorMapping.Add("Coven Leader", Colors.Coven);
                if (CustomGameOptions.SpiritualistOn > 0) ColorMapping.Add("Spiritualist", Colors.Coven);
                if (CustomGameOptions.VoodooMasterOn > 0) ColorMapping.Add("Voodoo Master", Colors.Coven);
                if (CustomGameOptions.PotionMasterOn > 0) ColorMapping.Add("Potion Master", Colors.Coven);
            }
            if (CustomGameOptions.VigilanteGuessLovers)
            {
                if (CustomGameOptions.LoversOn > 0) ColorMapping.Add("Lover", Colors.Lovers);
            }
            if (CustomGameOptions.VigilanteGuessImpModifiers)
            {
                if (CustomGameOptions.BloodlustOn > 0) ColorMapping.Add("Bloodlust", Colors.Impostor);
                if (CustomGameOptions.DisperserOn > 0) ColorMapping.Add("Disperser", Colors.Impostor);
                if (CustomGameOptions.LuckyOn > 0) ColorMapping.Add("Lucky", Colors.Impostor);
                if (CustomGameOptions.DoubleShotOn > 0) ColorMapping.Add("Double Shot", Colors.Impostor);
                if (CustomGameOptions.UnderdogOn > 0) ColorMapping.Add("Underdog", Colors.Impostor);
                if (CustomGameOptions.SaboteurOn > 0) ColorMapping.Add("Saboteur", Colors.Impostor);
                if (CustomGameOptions.TaskerOn > 0) ColorMapping.Add("Tasker", Colors.Impostor);
            }

            SortedColorMapping = ColorMapping.OrderBy(x => x.Key).ToDictionary(x => x.Key, x => x.Value);
        }

        public bool GuessedThisMeeting { get; set; } = false;

        public int RemainingKills { get; set; }

        public List<string> PossibleGuesses => SortedColorMapping.Keys.ToList();
    }
}
