using HarmonyLib;
using TownOfUsEdited.Extensions;
using TownOfUsEdited.Roles;

namespace TownOfUsEdited.NeutralRoles.DoomsayerMod
{
    [HarmonyPatch(typeof(MeetingHud), nameof(MeetingHud.Start))]
    public class MeetingStart
    {
        public static void Postfix(MeetingHud __instance)
        {
            if (PlayerControl.LocalPlayer.Data.IsDead) return;
            if (!PlayerControl.LocalPlayer.Is(RoleEnum.Doomsayer)) return;
            var doomsayerRole = Role.GetRole<Doomsayer>(PlayerControl.LocalPlayer);
            if (doomsayerRole.LastObservedPlayer != null)
            {
                var playerResults = PlayerReportFeedback(doomsayerRole.LastObservedPlayer);
                var roleResults = RoleReportFeedback(doomsayerRole.LastObservedPlayer);

                if (!string.IsNullOrWhiteSpace(playerResults)) HudManager.Instance.Chat.AddChat(PlayerControl.LocalPlayer, playerResults);
                if (!string.IsNullOrWhiteSpace(roleResults)) HudManager.Instance.Chat.AddChat(PlayerControl.LocalPlayer, roleResults);
            }
        }

        public static string PlayerReportFeedback(PlayerControl player)
        {
            if (player.Is(RoleEnum.Aurial) || player.Is(RoleEnum.Imitator) || player.Is(RoleEnum.Morphling) || player.Is(RoleEnum.Mystic)
                 || player.Is(RoleEnum.Spy) || player.Is(RoleEnum.Glitch) || player.Is(RoleEnum.TimeLord)
                 || player.Is(RoleEnum.Doppelganger) || player.Is(RoleEnum.Lookout))
                return $"You observe that {player.GetDefaultOutfit().PlayerName} has an altered perception of reality";
            else if (player.Is(RoleEnum.Blackmailer) || player.Is(RoleEnum.Detective) || player.Is(RoleEnum.Doomsayer)
                 || player.Is(RoleEnum.Oracle) || player.Is(RoleEnum.Snitch) || player.Is(RoleEnum.Trapper) || player.Is(RoleEnum.Captain)
                 || player.Is(RoleEnum.Informant) || player.Is(RoleEnum.CovenLeader) || player.Is(RoleEnum.Mercenary))
                return $"You observe that {player.GetDefaultOutfit().PlayerName} has an insight for private information";
            else if (player.Is(RoleEnum.Altruist) || player.Is(RoleEnum.Amnesiac) || player.Is(RoleEnum.Janitor) || player.Is(RoleEnum.Doctor)
                 || player.Is(RoleEnum.Medium) || player.Is(RoleEnum.Undertaker) || player.Is(RoleEnum.Vampire) || player.Is(RoleEnum.Mafioso)
                 || player.Is(RoleEnum.Avenger) || player.Is(RoleEnum.Astral) || player.Is(RoleEnum.Vulture) || player.Is(RoleEnum.Reviver))
                return $"You observe that {player.GetDefaultOutfit().PlayerName} has an unusual obsession with dead bodies";
            else if (player.Is(RoleEnum.Investigator) || player.Is(RoleEnum.Swooper) || player.Is(RoleEnum.Tracker)
                || player.Is(RoleEnum.VampireHunter) || player.Is(RoleEnum.Venerer) || player.Is(RoleEnum.Werewolf)
                || player.Is(RoleEnum.Hunter) || player.Is(RoleEnum.Mutant) || player.Is(RoleEnum.BountyHunter)
                || player.Is(RoleEnum.Noclip) || player.Is(RoleEnum.Watcher))
                return $"You observe that {player.GetDefaultOutfit().PlayerName} is well trained in hunting down prey";
            else if (player.Is(RoleEnum.Arsonist) || player.Is(RoleEnum.Miner) || player.Is(RoleEnum.Plaguebearer) || player.Is(RoleEnum.Hypnotist)
                  || player.Is(RoleEnum.Prosecutor) || player.Is(RoleEnum.Seer) || player.Is(RoleEnum.Transporter) || player.Is(RoleEnum.SerialKiller)
                  || player.Is(RoleEnum.Converter) || player.Is(RoleEnum.Crusader) || player.Is(RoleEnum.Attacker) || player.Is(RoleEnum.Infectious))
                return $"You observe that {player.GetDefaultOutfit().PlayerName} spreads fear amonst the group";
            else if (player.Is(RoleEnum.Engineer) || player.Is(RoleEnum.Escapist) || player.Is(RoleEnum.Shifter) || player.Is(RoleEnum.Grenadier)
                || player.Is(RoleEnum.GuardianAngel) || player.Is(RoleEnum.Medic) || player.Is(RoleEnum.Survivor) || player.Is(RoleEnum.Paranoïac)
                || player.Is(RoleEnum.Bodyguard) || player.Is(RoleEnum.Spiritualist) || player.Is(RoleEnum.Warden) || player.Is(RoleEnum.Cleric))
                return $"You observe that {player.GetDefaultOutfit().PlayerName} hides to protect themself or others";
            else if (player.Is(RoleEnum.Executioner) || player.Is(RoleEnum.Jester) || player.Is(RoleEnum.Mayor) || player.Is(RoleEnum.Politician)
                 || player.Is(RoleEnum.Swapper) || player.Is(RoleEnum.Traitor) || player.Is(Faction.Madmates) || player.Is(RoleEnum.Plumber)
                 || player.Is(RoleEnum.Veteran) || player.Is(RoleEnum.Troll) || player.Is(RoleEnum.Deputy)
                 || player.Is(RoleEnum.PotionMaster))
                return $"You observe that {player.GetDefaultOutfit().PlayerName} has a trick up their sleeve";
            else if (player.Is(RoleEnum.Bomber) || player.Is(RoleEnum.Juggernaut)
                 || player.Is(RoleEnum.Sheriff) || player.Is(RoleEnum.Vigilante) || player.Is(RoleEnum.Jailor) || player.Is(RoleEnum.Shooter)
                 || player.Is(RoleEnum.Warlock) || player.Is(RoleEnum.Fighter) || player.Is(RoleEnum.Knight) || player.Is(RoleEnum.Poisoner)
                 || player.Is(RoleEnum.Terrorist) || player.Is(RoleEnum.Kamikaze))
                return $"You observe that {player.GetDefaultOutfit().PlayerName} is capable of performing relentless attacks";
            else if (player.Is(RoleEnum.Witch) || player.Is(RoleEnum.Manipulator) || player.Is(RoleEnum.Pestilence) || player.Is(RoleEnum.Conjurer)
            || player.Is(RoleEnum.Assassin) || player.Is(RoleEnum.Ritualist) || player.Is(RoleEnum.HexMaster) || player.Is(RoleEnum.VoodooMaster))
                return $"You observe that {player.GetDefaultOutfit().PlayerName} is associated with dark powers";
            else if (player.Is(RoleEnum.Crewmate) || player.Is(RoleEnum.Impostor) || player.Is(RoleEnum.Coven))
                return $"You observe that {player.GetDefaultOutfit().PlayerName} appears to have a basic role";
            else
                return "Error";
        }

        public static string RoleReportFeedback(PlayerControl player)
        {
            if (player.Is(RoleEnum.Aurial) || player.Is(RoleEnum.Imitator) || player.Is(RoleEnum.Morphling) || player.Is(RoleEnum.Mystic)
                 || player.Is(RoleEnum.Spy) || player.Is(RoleEnum.Glitch) || player.Is(RoleEnum.TimeLord)
                 || player.Is(RoleEnum.Doppelganger) || player.Is(RoleEnum.Lookout))
                return "(Aurial, Doppelganger, Imitator, Lookout, Morphling, Mystic, Spy, The Glitch or Time Lord)";
            else if (player.Is(RoleEnum.Blackmailer) || player.Is(RoleEnum.Detective) || player.Is(RoleEnum.Doomsayer)
                 || player.Is(RoleEnum.Oracle) || player.Is(RoleEnum.Snitch) || player.Is(RoleEnum.Trapper) || player.Is(RoleEnum.Captain)
                 || player.Is(RoleEnum.Informant) || player.Is(RoleEnum.CovenLeader) || player.Is(RoleEnum.Mercenary))
                return "(Blackmailer, Captain, Coven Leader, Detective, Doomsayer, Informant, Mercenary, Oracle, Snitch or Trapper)";
            else if (player.Is(RoleEnum.Altruist) || player.Is(RoleEnum.Amnesiac) || player.Is(RoleEnum.Janitor) || player.Is(RoleEnum.Doctor)
                 || player.Is(RoleEnum.Medium) || player.Is(RoleEnum.Undertaker) || player.Is(RoleEnum.Vampire) || player.Is(RoleEnum.Reviver)
                 || player.Is(RoleEnum.Mafioso) || player.Is(RoleEnum.Avenger) || player.Is(RoleEnum.Astral) || player.Is(RoleEnum.Vulture))
                return "(Altruist, Amnesiac, Astral, Avenger, Doctor, Janitor, Mafioso, Medium, Reviver, Undertaker, Vampire or Vulture)";
            else if (player.Is(RoleEnum.Investigator) || player.Is(RoleEnum.Swooper) || player.Is(RoleEnum.Tracker)
                || player.Is(RoleEnum.VampireHunter) || player.Is(RoleEnum.Venerer) || player.Is(RoleEnum.Werewolf)
                || player.Is(RoleEnum.Hunter) || player.Is(RoleEnum.Mutant) || player.Is(RoleEnum.BountyHunter)
                || player.Is(RoleEnum.Noclip) || player.Is(RoleEnum.Watcher))
                return "(Bounty Hunter, Hunter, Investigator, Mutant, Noclip, Swooper, Tracker, Vampire Hunter, Venerer, Watcher or Werewolf)";
            else if (player.Is(RoleEnum.Arsonist) || player.Is(RoleEnum.Miner) || player.Is(RoleEnum.Plaguebearer) || player.Is(RoleEnum.Hypnotist)
                  || player.Is(RoleEnum.Prosecutor) || player.Is(RoleEnum.Seer) || player.Is(RoleEnum.Transporter) || player.Is(RoleEnum.SerialKiller)
                  || player.Is(RoleEnum.Converter) || player.Is(RoleEnum.Crusader) || player.Is(RoleEnum.Attacker) | player.Is(RoleEnum.Infectious))
                return "(Arsonist, Attacker, Converter, Crusader, Hypnotist, Infectious, Miner, Plaguebearer, Prosecutor, Seer, Serial Killer, Shifter or Transporter)";
            else if (player.Is(RoleEnum.Engineer) || player.Is(RoleEnum.Escapist) || player.Is(RoleEnum.Shifter) || player.Is(RoleEnum.Grenadier)
                || player.Is(RoleEnum.GuardianAngel) || player.Is(RoleEnum.Medic) || player.Is(RoleEnum.Survivor) || player.Is(RoleEnum.Paranoïac)
                || player.Is(RoleEnum.Bodyguard) || player.Is(RoleEnum.Spiritualist) || player.Is(RoleEnum.Warden) || player.Is(RoleEnum.Cleric))
                return "(Bodyguard, Cleric, Engineer, Escapist, Grenadier, Guardian Angel, Medic, Paranoïac, Shifter, Spiritualist, Survivor or Warden)";
            else if (player.Is(RoleEnum.Executioner) || player.Is(RoleEnum.Jester) || player.Is(RoleEnum.Mayor) || player.Is(RoleEnum.Politician)
                 || player.Is(RoleEnum.Swapper) || player.Is(RoleEnum.Traitor) || player.Is(Faction.Madmates) || player.Is(RoleEnum.Plumber)
                 || player.Is(RoleEnum.Veteran) || player.Is(RoleEnum.Troll) || player.Is(RoleEnum.Deputy)
                 || player.Is(RoleEnum.PotionMaster))
                return "(Deputy, Executioner, Jester, Plumber, Politician, Potion Master, Troll, Mayor, Swapper, Madmate, Traitor or Veteran)";
            else if (player.Is(RoleEnum.Bomber) || player.Is(RoleEnum.Juggernaut) || player.Is(RoleEnum.Kamikaze)
                 || player.Is(RoleEnum.Sheriff) || player.Is(RoleEnum.Vigilante) || player.Is(RoleEnum.Jailor) || player.Is(RoleEnum.Shooter)
                 || player.Is(RoleEnum.Warlock) || player.Is(RoleEnum.Fighter) || player.Is(RoleEnum.Knight) || player.Is(RoleEnum.Poisoner)
                 || player.Is(RoleEnum.Terrorist))
                return "(Bomber, Fighter, Jailor, Kamikaze, Juggernaut, Knight, Poisoner, Sheriff, Shooter, Terrorist, Vigilante or Warlock)";
            else if (player.Is(RoleEnum.Witch) || player.Is(RoleEnum.Manipulator) || player.Is(RoleEnum.Pestilence) || player.Is(RoleEnum.Conjurer)
            || player.Is(RoleEnum.Assassin) || player.Is(RoleEnum.Ritualist) || player.Is(RoleEnum.HexMaster) || player.Is(RoleEnum.VoodooMaster))
                return "(Assassin, Conjurer, Hex Master, Manipulator, Pestilence, Ritualist, Voodoo Master or Witch)";
            else if (player.Is(RoleEnum.Crewmate) || player.Is(RoleEnum.Impostor) || player.Is(RoleEnum.Coven))
                return "(Coven, Crewmate or Impostor)";
            else return "Error";
        }
    }
}