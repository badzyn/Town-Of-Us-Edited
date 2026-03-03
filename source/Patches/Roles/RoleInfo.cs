using HarmonyLib;
using UnityEngine;

namespace TownOfUsEdited.Roles
{
    [HarmonyPatch(typeof(HudManager))]
    public class UpdateRoleInfo
    {
        [HarmonyPatch(nameof(HudManager.Update))]
        public static void Postfix(HudManager __instance)
        {
            if (Input.GetKeyDown(KeyCode.F2))
            {
                var pc = PlayerControl.LocalPlayer;
                // Spectator
                if (pc.Is(RoleEnum.Spectator))
                {
                    HudManager.Instance.ShowPopUp("You are the <color=#da6f00>Spectator</color>, enjoy watching the game.");
                }
                // Crewmates Roles infos
                if (pc.Is(RoleEnum.Crewmate))
                {
                    HudManager.Instance.ShowPopUp("You are the <color=#00FFFF>Crewmate</color>, do you tasks and find out who the <color=#FF0000>Impostors</color> are!");
                }
                if (pc.Is(RoleEnum.Aurial))
                {
                    HudManager.Instance.ShowPopUp("You are the <color=#B34D99FF>Aurial</color>, you can see things in your aura.\nIf anyone uses an ability near you, you will get an arrow pointing to where it has been used.");
                }
                if (pc.Is(RoleEnum.Captain))
                {
                    HudManager.Instance.ShowPopUp("You are the <color=#6B8DE1>Captain</color>, use your zoom ability to increase your vision.");
                }
                if (pc.Is(RoleEnum.Avenger))
                {
                    HudManager.Instance.ShowPopUp("You are the <color=#216f01>Avenger</color>, you can avenge a dead body if you find one, resulting in being able to kill his killer.");
                }
                if (pc.Is(RoleEnum.Chameleon))
                {
                    HudManager.Instance.ShowPopUp("You are the <color=#068c38>Chameleon</color>, use your ability to turn invisible and watch other players.");
                }
                if (pc.Is(RoleEnum.Detective))
                {
                    HudManager.Instance.ShowPopUp("You are the <color=#4D4DFFFF>Detective</color>, you can inspect crime scenes and then examine players to see if they were near the crime scene during the kill (they will show a red flash on your screen).");
                }
                if (pc.Is(RoleEnum.Haunter))
                {
                    HudManager.Instance.ShowPopUp("You are the <color=#D3D3D3FF>Haunter</color>, finish your tasks to reveal the <color=#FF0000FF>Impostors</color> to everyone.");
                }
                if (pc.Is(RoleEnum.Helper))
                {
                    HudManager.Instance.ShowPopUp("You are the <color=#7cb6c2>Helper</color>, you can use your ability on a player to alert them from a nearby danger.\nThe alerted player will get a pop up saying that someone alerted them and will get a speed boost during the Alert duration.");
                }
                if (pc.Is(RoleEnum.Guardian))
                {
                    HudManager.Instance.ShowPopUp("You are the <color=#67bb43>Guardian</color>, you can use your ability on a player to make them immune to direct kills during a certain amount of time.");
                }
                if (pc.Is(RoleEnum.Investigator))
                {
                    HudManager.Instance.ShowPopUp("You are the <color=#00B3B3FF>Investigator</color>, inspect footprints to know where anyone came from.");
                }
                if (pc.Is(RoleEnum.Mystic))
                {
                    HudManager.Instance.ShowPopUp("You are the <color=#4D99E6FF>Mystic</color>, when someone gets killed, you will see a pop up in real time indicating who is dead.");
                }
                if (pc.Is(RoleEnum.Oracle))
                {
                    HudManager.Instance.ShowPopUp("You are the <color=#BF00BFFF>Oracle</color>, you can force a player to confess to you giving you an information on one of two players.\nThe confessed player will also reveal their alignment to everyone if you die.\nFinally, you can also use your bless ability on someone, making them immune to being voted out the next meeting.");
                }
                if (pc.Is(RoleEnum.Seer))
                {
                    HudManager.Instance.ShowPopUp("You are the <color=#FFCC80FF>Seer</color>, you can check if someone is evil or not.\nUse this ability on suspicious players!");
                }
                if (pc.Is(RoleEnum.Snitch))
                {
                    HudManager.Instance.ShowPopUp("You are the <color=#D4AF37FF>Snitch</color>, you can see all <color=#FF0000FF>Impostors</color> after completing all your tasks.\nBe careful because <color=#FF0000FF>Impostors</color> will also know who you are after completing a certain amount of tasks.");
                }
                if (pc.Is(RoleEnum.Spy))
                {
                    HudManager.Instance.ShowPopUp("You are the <color=#CCA3CCFF>Spy</color>, you can see anyone's location on the admin map.");
                }
                if (pc.Is(RoleEnum.Tracker))
                {
                    HudManager.Instance.ShowPopUp("You are the <color=#009900FF>Tracker</color>, you can track anyone's location in real time.");
                }
                if (pc.Is(RoleEnum.Trapper))
                {
                    HudManager.Instance.ShowPopUp("You are the <color=#A7D1B3FF>Trapper</color>, place traps to gain informations about other player's roles.");
                }
                if (pc.Is(RoleEnum.Fighter))
                {
                    HudManager.Instance.ShowPopUp("You are the <color=#9358e6>Fighter</color>, you can kill anyone but if you kill a Crewmate, you will turn into a Madmate.");
                }
                if (pc.Is(RoleEnum.Hunter))
                {
                    HudManager.Instance.ShowPopUp("You are the <color=#29AB87FF>Hunter</color>, you can execute any suspicious player if they have interacted with someone else.");
                }
                if (pc.Is(RoleEnum.Jailor))
                {
                    HudManager.Instance.ShowPopUp("You are the <color=#A6A6A6>Jailor</color>, you can use your ability to Jail someone.\nDuring the meeting, you can talk to the imprisoned player in a special chat window called Jailor Chat.\nIf this person turns out to be evil, you can kill them by clicking the “Execute” button located under the “Skip” button (you must confirm the execution).\nIf the player turns out to be a Crewmate, you lose the ability to jailed players and, depending on the settings, the Jailor misfires and dies.");
                }
                if (pc.Is(RoleEnum.Deputy))
                {
                    HudManager.Instance.ShowPopUp("You are the <color=#FFCC00FF>Deputy</color>, you can camp on a player during the round.\nThen, if that player dies, you can try to shoot the killer next meeting.\nIf you shoot correctly, you will kill the killer, else, nothing happens.");
                }
                if (pc.Is(RoleEnum.Knight))
                {
                    HudManager.Instance.ShowPopUp("You are the <color=#ffd082>Knight</color>, you can kill anyone but only once.\nUse this ability with responsibility.");
                }
                if (pc.Is(RoleEnum.Sheriff))
                {
                    HudManager.Instance.ShowPopUp("You are the <color=#FFFF00FF>Sheriff</color>, you can kill the <color=#FF0000FF>Impostors</color> but if you try to kill a <color=#00FFFF>Crewmate</color>, you will die instead.");
                }
                if (pc.Is(RoleEnum.VampireHunter))
                {
                    HudManager.Instance.ShowPopUp("You are the <color=#B3B3E6FF>Vampire Hunter</color>, you can cure converted <color=#262626FF>Vampires</color> from the infections making them recover their old role.\nIf you try to cure the original <color=#262626FF>Vampire</color>, you will kill them!\nIf all <color=#262626FF>Vampires</color> are dead, you will turn into another Crewmate role.");
                }
                if (pc.Is(RoleEnum.Veteran))
                {
                    HudManager.Instance.ShowPopUp("You are the <color=#998040FF>Veteran</color>, you can alert to kill anyone that interacts with you (including non killers).\nUse this ability to protect yourself against attacks.");
                }
                if (pc.Is(RoleEnum.Astral))
                {
                    HudManager.Instance.ShowPopUp("You are the <color=#a703ae>Astral</color>, you can use your ability to temporarily become a ghost and leave your body.\nBe careful though because you can still be killed if someone attacks your body!");
                }
                if (pc.Is(RoleEnum.Lookout))
                {
                    HudManager.Instance.ShowPopUp("You are the <color=#33FF66FF>Lookout</color>, you can use your ability to watch any player in real time.\nBut the player may be alerted when you do so.");
                }
                if (pc.Is(RoleEnum.Watcher))
                {
                    HudManager.Instance.ShowPopUp("You are the <color=#33FF66FF>Watcher</color>, You can use your ability on players and find out who interacted with him at the next meeting.");
                }
                if (pc.Is(RoleEnum.Vigilante))
                {
                    HudManager.Instance.ShowPopUp("You are the <color=#FFFF99FF>Vigilante</color>, you can guess <color=#FF0000FF>Impostor</color>'s roles in meeting.\nHowever, if you guess a role incorreclty, you will die instead.");
                }
                if (pc.Is(RoleEnum.Altruist))
                {
                    HudManager.Instance.ShowPopUp("You are the <color=#660000FF>Altruist</color>, you can sacrifice yourself to revive someone else.\nHowever, any evil player will be alerted that you revived someone.");
                }
                if (pc.Is(RoleEnum.Medic))
                {
                    HudManager.Instance.ShowPopUp("You are the <color=#006600FF>Medic</color>, you can give a shield to someone which will make him invincible to murder attempts.\nIf the shield break, you may be noticed.\nThe shield will also break if you die.\nYou also gain informations by reporting dead bodies.");
                }
                if (pc.Is(RoleEnum.Cleric))
                {
                    HudManager.Instance.ShowPopUp("You are the <color=#00FFB3FF>Cleric</color>, you have two abilities, the first one gives a shield to a player making him protected from murder attempts for a short period of time and the second one lets you cleanse a player removing any bad effect from them (douse, infection, hysteria...).");
                }
                if (pc.Is(RoleEnum.Plumber))
                {
                    HudManager.Instance.ShowPopUp("You are the <color=#CC6600FF>Plumber</color>, your first ability allows you to eject all the players that are using the vents, you can also block a vent making it unusable for the rest of the game.");
                }
                if (pc.Is(RoleEnum.Doctor))
                {
                    HudManager.Instance.ShowPopUp("You are the <color=#00EE3C>Doctor</color>, use your abilities to resurect other players!\nYou can resurect anyone so be careful while using your powers.\nYou may also be only able to resurect someone in MedBay / Laboratory depending on settings. If so, you have a drag button to drag the body to a medical area.\nNote that revived players can't chat nor vote in meetings.");
                }
                if (pc.Is(RoleEnum.Crusader))
                {
                    HudManager.Instance.ShowPopUp("You are the <color=#815230>Crusader</color>, you can Crusade someone resulting in killing anyone that interacts with that player.\nYou can only Crusade one player at the same time.\nCrusade will be reset every meeting.");
                }
                if (pc.Is(RoleEnum.Bodyguard))
                {
                    HudManager.Instance.ShowPopUp("You are the <color=#255bc9>Bodyguard</color>, if a near player tries to kill someone else, you will kill them and die too.\nNote that this only applies to players that use the regular kill method.");
                }
                if (pc.Is(RoleEnum.Paranoïac))
                {
                    HudManager.Instance.ShowPopUp("You are the <color=#0a7eae>Paranoïac</color>, you can call a Meeting at anytime and hide in vents if you feel scared.");
                }
                if (pc.Is(RoleEnum.Engineer))
                {
                    HudManager.Instance.ShowPopUp("You are the <color=#FFA60AFF>Engineer</color>, you can use vents to catch <color=#FF0000FF>Impostors</color> and you can also fix sabotages.");
                }
                if (pc.Is(RoleEnum.TimeLord))
                {
                    HudManager.Instance.ShowPopUp("You are the <color=#0f00b8>Time Lord</color>, you can rewind the time making players go back in time and also reviving latest dead players if their bodies didn't get eaten / cleaned.");
                }
                if (pc.Is(RoleEnum.Imitator))
                {
                    HudManager.Instance.ShowPopUp("You are the <color=#B3D94DFF>Imitator</color>, you can imitate the role of dead Crewmates.\nNote that not all roles are imitables.");
                }
                if (pc.Is(RoleEnum.Warden))
                {
                    HudManager.Instance.ShowPopUp("You are the <color=#9900FFFF>Warden</color>, you can use your button on someone to fortify them, they will be immune to interactions and if someone does interact with them, you and your target will get notified.\nNote that this doesn't protect from kills.");
                }
                if (pc.Is(RoleEnum.President))
                {
                    HudManager.Instance.ShowPopUp("You are the <color=#704FA8FF>President</color>, you can abstain your vote for the next round");
                }
                if (pc.Is(RoleEnum.Medium))
                {
                    HudManager.Instance.ShowPopUp("You are the <color=#A680FFFF>Medium</color>, you can see the ghosts of recent dead players by pressing the Mediate button.\nGhosts might lead you to their killer but be careful because some might try to trick you!");
                }
                if (pc.Is(RoleEnum.Prosecutor))
                {
                    HudManager.Instance.ShowPopUp("You are the <color=#B38000FF>Prosecutor</color>, you can choose to control the vote and vote someone out once.\nHowever, you may also die by executing a Crewmate.\nYou can not prosecute on the 10 last seconds of a meeting.");
                }
                if (pc.Is(RoleEnum.Swapper))
                {
                    HudManager.Instance.ShowPopUp("You are the <color=#66E666FF>Swapper</color>, you can swap the votes of two players in meeting.");
                }
                if (pc.Is(RoleEnum.Transporter))
                {
                    HudManager.Instance.ShowPopUp("You are the <color=#00EEFFFF>Transporter</color>, you can swap the location of two players.");
                }
                if (pc.Is(RoleEnum.Informant))
                {
                    HudManager.Instance.ShowPopUp("You are the <color=#4CFAAB>Informant</color>, you can access vitals and admin from anywhere on the map.");
                }
                
                // Neutrals Roles infos
                if (pc.Is(RoleEnum.Amnesiac))
                {
                    HudManager.Instance.ShowPopUp("You are the <color=#80B2FFFF>Amnesiac</color>, you forgot your role so remember a dead player's one!");
                }
                if (pc.Is(RoleEnum.Shifter))
                {
                    HudManager.Instance.ShowPopUp("You are the <color=#AAAAAA>Shifter</color>, you can shift your role with anyone and you are not guessable!");
                }
                else if (pc.Is(RoleEnum.Mercenary))
                {
                    HudManager.Instance.ShowPopUp("You are the <color=#8C6699FF>Mercenary</color>, you can guard player and get rewarded with gold if someone interacts with your target.\nUse then what you've earned to bribe other players.\nIf one of the bribed players wins, you will win too, so make sure to get as much bribed players as possible!.");
                }
                if (pc.Is(RoleEnum.GuardianAngel))
                {
                    HudManager.Instance.ShowPopUp("You are the <color=#B3FFFFFF>Guardian Angel</color>, you have to protect your target at any cost!\nYou can use your ability to give a shield to your target.\nIf your target dies before you, you role will change depending on the settings.");
                }
                if (pc.Is(RoleEnum.Survivor))
                {
                    HudManager.Instance.ShowPopUp("You are the <color=#FFE64DFF>Survivor</color>, you can use your vest ability to give you a temporarily shield and become immune to attacks.\nYour goal is to stay alive until the end!");
                }
                if (pc.Is(RoleEnum.Doomsayer))
                {
                    HudManager.Instance.ShowPopUp("You are the <color=#00FF80FF>Doomsayer</color>, you can guess the role of other players in meeting. Guessing a certain amount of roles will result in making you win!");
                }
                if (pc.Is(RoleEnum.Executioner))
                {
                    HudManager.Instance.ShowPopUp("You are the <color=#8C4005FF>Executioner</color>, if you manage to eject your target, you win.");
                }
                if (pc.Is(RoleEnum.Jester))
                {
                    HudManager.Instance.ShowPopUp("You are the <color=#FFBFCCFF>Jester</color>, get voted out to win!\nYou may be able to vent to act more suspicious.");
                }
                if (pc.Is(RoleEnum.SoulCollector))
                {
                    HudManager.Instance.ShowPopUp($"You are the <color=#99FFCCFF>Soul Collector</color>, you can reap players, instantly killing them and leaving a soul instead of a body on the ground.\nReap everyone and be the last one standing to win!");
                }
                if (pc.Is(RoleEnum.Vulture))
                {
                    HudManager.Instance.ShowPopUp($"You are the <color=#916e6e>Vulture</color>, eat {CustomGameOptions.VultureBodies} Dead Bodies to win!");
                }
                if (pc.Is(RoleEnum.Phantom))
                {
                    HudManager.Instance.ShowPopUp("You are the <color=#662962FF>Phantom</color>, finish your tasks before getting clicked to win.");
                }
                if (pc.Is(RoleEnum.Troll))
                {
                    HudManager.Instance.ShowPopUp("You are the <color=#c27d5a>Troll</color>, you can force someone to kill you, if this player gets ejected, you win.");
                }
                if (pc.Is(RoleEnum.Arsonist))
                {
                    HudManager.Instance.ShowPopUp("You are the <color=#FF4D00FF>Arsonist</color>, you can douse players and then kill all doused players by pressing the Ignite button.");
                }
                if (pc.Is(RoleEnum.Mutant))
                {
                    HudManager.Instance.ShowPopUp("You are the <color=#b6eb5e>Mutant</color>, you can kill normally but with a long cooldown and a low vison.\nYou can also decide to transform which will give you a shorter kill cooldown, an Impostor vision and also depending on settings, the ability to vent.");
                }
                if (pc.Is(RoleEnum.Infectious))
                {
                    HudManager.Instance.ShowPopUp("You are the <color=#bf9000>Infectious</color>, you can kill or infect player.\nInfecting a player will after each round give them a disadvantage until it kills them.\nOn stage 1 of the infection, nothing happens, on stage 2, they become slower, on stage 3, their vision gets reduced and on stage 4, they die.\nIf you kill an infected player, your next cooldown will be shorter.");
                }
                if (pc.Is(RoleEnum.Plaguebearer))
                {
                    HudManager.Instance.ShowPopUp("You are the <color=#E6FFB3FF>Plaguebearer</color>, infect everyone to turn into Pestilence!");
                }
                if (pc.Is(RoleEnum.Attacker))
                {
                    HudManager.Instance.ShowPopUp("You are the <color=#c73a00>Attacker</color>, finish your tasks to become <color=#8bce23>Terrorist</color>!");
                }
                if (pc.Is(RoleEnum.Terrorist))
                {
                    HudManager.Instance.ShowPopUp("You are the <color=#8bce23>Terrorist</color>, kill everyone to win!\nYou can not be guessed or voted out!");
                }
                if (pc.Is(RoleEnum.Pestilence))
                {
                    HudManager.Instance.ShowPopUp("You are the <color=#4c4c4c>Pestilence</color>, the most powerful role, no one can attack or guess you, the only way to get rid of you is by ejecting you.");
                }
                if (pc.Is(RoleEnum.SerialKiller))
                {
                    HudManager.Instance.ShowPopUp("You are the <color=#0c20d5>Serial Killer</color>, you can kill anyone and may also be able to convert someone to your side, depending on settings.");
                }
                if (pc.Is(RoleEnum.Doppelganger))
                {
                    HudManager.Instance.ShowPopUp("You are the <color=#fadd84>Doppelganger</color>, whenever you kill someone, you transform into them.\nNote that you can't transform with the reviver.\nKill everyone to win.");
                }
                if (pc.Is(RoleEnum.Glitch))
                {
                    HudManager.Instance.ShowPopUp("You are The <color=#00FF00FF>Glitch</color>, you can mimic to morph into someone else, hack a player to make them unable to use their abilities and kill anyone.");
                }
                if (pc.Is(RoleEnum.Vampire))
                {
                    HudManager.Instance.ShowPopUp("You are the <color=#262626FF>Vampire</color>, you can bite someone to convert them to your side, if the convert is unsuccessful, your cooldown will be reset.\nYou can also perform regular kills.");
                }
                if (pc.Is(RoleEnum.Werewolf))
                {
                    HudManager.Instance.ShowPopUp("You are the <color=#A86629FF>Werewolf</color>, use your Rampage ability to kill with a shorter kill cooldown and to get an Impostor vision.\nYou may also gain the ability to vent while rampaging, depending on settings.");
                }
                if (pc.Is(RoleEnum.Juggernaut))
                {
                    HudManager.Instance.ShowPopUp("You are the <color=#8c004c>Juggernaut</color>, when you use your kill ability, your next kill cooldown decreases etc until it reaches 0s.");
                }
                // Impostors Roles info
                if (pc.Is(RoleEnum.Impostor))
                {
                    HudManager.Instance.ShowPopUp("You are the <color=#FF0000FF>Impostor</color>, kill everyone and sabotage without being caught!");
                }
                if (pc.Is(RoleEnum.Wraith))
                {
                    HudManager.Instance.ShowPopUp("You are the <color=#FF0000FF>Wraith</color>, if you manage to complete all your tasks before getting clicked, you will kill a random non impostor!");
                }
                if (pc.Is(RoleEnum.Freezer))
                {
                    HudManager.Instance.ShowPopUp("You are the <color=#FF0000FF>Freezer</color>, you can use your ability to temporarily make alive players unable to move.");
                }
                if (pc.Is(RoleEnum.Blinder))
                {
                    HudManager.Instance.ShowPopUp("You are the <color=#FF0000FF>Blinder</color>, you can use your ability to temporarily make alive players unable to see.");
                }
                if (pc.Is(RoleEnum.Escapist))
                {
                    HudManager.Instance.ShowPopUp("You are the <color=#FF0000FF>Escapist</color>, you can use your Mark button to set a point on the map, and then you can teleport to that point using the Recall button.");
                }
                if (pc.Is(RoleEnum.Kamikaze))
                {
                    HudManager.Instance.ShowPopUp("You are the <color=#FF0000FF>Kamikaze</color>, sacrifice yourself for greater good");
                }
                if (pc.Is(RoleEnum.Grenadier))
                {
                    HudManager.Instance.ShowPopUp("You are the <color=#FF0000FF>Grenadier</color>, you can use your Flash button to make near Crewmates totally flash.");
                }
                if (pc.Is(RoleEnum.Morphling))
                {
                    HudManager.Instance.ShowPopUp("You are the <color=#FF0000FF>Morphling</color>, you can morph into the player you want after taking a sample of his body.");
                }
                if (pc.Is(RoleEnum.Hypnotist))
                {
                    HudManager.Instance.ShowPopUp("You are the <color=#FF0000FF>Hypnotist</color>, you can use your ability to hypnotize other players.\nOnce enough people are hypnotized, you can release Mass Hysteria making hypnotized players see all other players as either themselves, camouflaged or invisible.");
                }
                if (pc.Is(RoleEnum.Assassin))
                {
                    HudManager.Instance.ShowPopUp("You are the <color=#FF0000FF>Assassin</color>, you can guess the roles of players during the meeting to kill them.\nHowever, if your guess is wrong, you die instead.");
                }
                if (pc.Is(RoleEnum.Swooper))
                {
                    HudManager.Instance.ShowPopUp("You are the <color=#FF0000FF>Swooper</color>, you can turn invisible to kill without getting caught.");
                }
                if (pc.Is(RoleEnum.Venerer))
                {
                    HudManager.Instance.ShowPopUp("You are the <color=#FF0000FF>Venerer</color>, you unlock different abilities after each kill.");
                }
                if (pc.Is(RoleEnum.Poisoner))
                {
                    HudManager.Instance.ShowPopUp("You are the <color=#FF0000FF>Poisoner</color>, you can poison your target to make them die after a certain delay.");
                }
                if (pc.Is(RoleEnum.Shooter))
                {
                    HudManager.Instance.ShowPopUp("You are the <color=#FF0000FF>Shooter</color>, you can store bullets to kill again without cooldown after. Storing a bullet will reset your kill cooldown.");
                }
                if (pc.Is(RoleEnum.Bomber))
                {
                    HudManager.Instance.ShowPopUp("You are the <color=#FF0000FF>Bomber</color>, you can place a bomb somewhere on the map which will kill every player in its radius after exploding.\nBe careful because the bomb may also kill yourself or your teammates!");
                }
                if (pc.Is(RoleEnum.Traitor))
                {
                    HudManager.Instance.ShowPopUp("You are the <color=#FF0000FF>Traitor</color>, you betrayed the Crew and turned <color=#FF0000FF>Impostor</color>, now kill them all!");
                }
                if (pc.Is(RoleEnum.Warlock))
                {
                    HudManager.Instance.ShowPopUp("You are the <color=#FF0000FF>Warlock</color>, you can charge up your kill cooldown resulting in killing multiple players at the same time.");
                }
                if (pc.Is(RoleEnum.Mafioso))
                {
                    HudManager.Instance.ShowPopUp("You are the <color=#FF0000FF>Mafioso</color>, you can not kill, sabotage or vent until all of your teammates are dead.");
                }
                if (pc.Is(RoleEnum.Reviver))
                {
                    HudManager.Instance.ShowPopUp("You are the <color=#FF0000FF>Reviver</color>, if you die, you are able to revive inside someone else's body during the same round.\nBut you won't be able to kill unless all of your teamates are dead.\nIf the round ends and you didn't revive, then you won't be able to revive anymore.\nNote that you can't revive into all bodies (ex: bodies that switched identity with Doppelganger).");
                }
                if (pc.Is(RoleEnum.Blackmailer))
                {
                    HudManager.Instance.ShowPopUp("You are the <color=#FF0000FF>Blackmailer</color>, you can use your ability to make a player unable to talk in meetings!");
                }
                if (pc.Is(RoleEnum.Converter))
                {
                    HudManager.Instance.ShowPopUp("You are the <color=#FF0000FF>Converter</color>, you can use your ability revive a dead Crewmate and convert them to your side.");
                }
                if (pc.Is(RoleEnum.Janitor))
                {
                    HudManager.Instance.ShowPopUp("You are the <color=#FF0000FF>Janitor</color>, you can clean up dead bodies on the map.");
                }
                if (pc.Is(RoleEnum.Witch))
                {
                    HudManager.Instance.ShowPopUp("You are the <color=#FF0000FF>Witch</color>, you can cast spells on players, killing them after the next meeting.\nHowever, if you die before the end of that meeting or get ejected, the players won't die.\nYou can also perform regular kills.");
                }
                if (pc.Is(RoleEnum.Conjurer))
                {
                    HudManager.Instance.ShowPopUp("You are the <color=#FF0000FF>Conjurer</color>, you can curse a player and control him to kill with his identity.");
                }
                if (pc.Is(RoleEnum.BountyHunter))
                {
                    HudManager.Instance.ShowPopUp("You are the <color=#FF0000FF>Bounty Hunter</color>, a target is assigned to you, if you kill your target, you get a short kill cooldown, else a long.");
                }
                if (pc.Is(RoleEnum.Miner))
                {
                    HudManager.Instance.ShowPopUp("You are the <color=#FF0000FF>Miner</color>, you can place vents around the map and use them to go away after killing.");
                }
                if (pc.Is(RoleEnum.Undertaker))
                {
                    HudManager.Instance.ShowPopUp("You are the <color=#FF0000FF>Undertaker</color>, you can drag bodies around the map to hide them.");
                }
                if (pc.Is(RoleEnum.Manipulator))
                {
                    HudManager.Instance.ShowPopUp("You are the <color=#FF0000FF>Manipulator</color>, you can use your Manipulate button on a player.\nThen, you can control that player, making you able to control their movements and use their identity to kill.\nNote that the Manipulation will stop if the Manipulated gets killed, if you get killed or if a meeting is called.");
                }
                if (pc.Is(RoleEnum.Noclip))
                {
                    HudManager.Instance.ShowPopUp("You are the <color=#FF0000FF>Noclip</color>, you can use your Noclip button to walk through walls.");
                }
                //Coven Roles info
                if (pc.Is(RoleEnum.Coven))
                {
                    HudManager.Instance.ShowPopUp("You are the <color=#bf5fff>Coven</color>, you can kill & sabotage, kill all non-coven members to win!");
                }
                if (pc.Is(RoleEnum.Ritualist))
                {
                    HudManager.Instance.ShowPopUp("You are the <color=#bf5fff>Ritualist</color>, you can guess player's roles resulting in killing them.\nHowever, if you guess incorrectly, you will die instead!");
                }
                if (pc.Is(RoleEnum.HexMaster))
                {
                    HudManager.Instance.ShowPopUp("You are the <color=#bf5fff>Hex Master</color>, you can hex players and then hex bomb them all at the same time.\nHowever, everyone will know who the hexed players are.");
                }
                if (pc.Is(RoleEnum.CovenLeader))
                {
                    HudManager.Instance.ShowPopUp("You are the <color=#bf5fff>Coven Leader</color>, you have access to the vent and can convert anyone to coven once.");
                }
                if (pc.Is(RoleEnum.Spiritualist))
                {
                    HudManager.Instance.ShowPopUp("You are the <color=#bf5fff>Spiritualist</color>, you can use your ability to control a player.\nIf this player tries to attack a coven members with a direct kill, the attack will reverse.\nThis does not apply to undirect kills (veteran, bomb, poison, guess...).");
                }
                if (pc.Is(RoleEnum.VoodooMaster))
                {
                    HudManager.Instance.ShowPopUp("You are the <color=#bf5fff>Voodoo Master</color>, you can use your ability to force a player to vote someone during the meeting.\nThis player will be forced to vote the player that you are voting.");
                }
                if (pc.Is(RoleEnum.PotionMaster))
                {
                    HudManager.Instance.ShowPopUp("You are the <color=#bf5fff>Potion Master</color>, you use your ability to get a random potion effect.\nUsing it will give you one of the following potions:\nSpeed: Make your speed higher\nStrength: You are able to vent and have a shorter kill cooldown\nShield: You are temporarily immune to direct kills\nInvisibility: You are invisible to other players");
                }
                //Battle Royale Role
                if (pc.Is(RoleEnum.Player))
                {
                    HudManager.Instance.ShowPopUp("This is <color=#006aff>Battle Royale</color> Mode, as a <color=#006aff>Player</color> your goal is to kill everyone and be the last one standing!\nNo meetings and report allowed.");
                }
            }
        }

        public static string GetRoleInfos(string role)
        {
            // vanilla
            if (role == "Crewmate")
            {
                return "The <color=#00FFFF>Crewmate</color>'s goal is to either finish all of its Tasks or finding and ejecting all non-crew evil killers (Impostors, Covens, Neutral Killers).";
            }
            else if (role == "Impostor")
            {
                return "The <color=#FF0000>Impostor</color> can call Sabotages, kill and use the vents. Its goal is to kill everyone besides other Impostors.";
            }
            // crewmates
            else if (role == "Aurial")
            {
                return "The <color=#B34D99FF>Aurial</color> can sense events in its aura.\nWhenever a player uses an ability near the Aurial, an arrow pointing to where the ability was used at will appear.";
            }
            else if (role == "Captain")
            {
                return "The <color=#6B8DE1>Captain</color> can zoom out on the map to catch Impostors.";
            }
            else if (role == "Avenger")
            {
                return "The <color=#216f01>Avenger</color> has an ability usable on a dead body.\nWhenever the Avenger uses its Avenge ability, the kill button of the Avenger will activate and target the killer of the dead body.\nThe killer will have its name displayed as black to the Avenger.\nHowever, if someone reports / calls a meeting, the Avenger will no longer see who the killer is.";
            }
            else if (role == "Chameleon")
            {
                return "The <color=#068c38>Chameleon</color> can turn Invisible temporarily to gain informations.";
            }
            else if (role == "Detective")
            {
                return "The <color=#4D4DFFFF>Detective</color> can use its ability to inspect crime scenes.\nRight after that, it can examine other players, if that player was near the crime scene during the kill, they will show a red flash on the Detective's screen.";
            }
            else if (role == "Haunter")
            {
                return "The <color=#D3D3D3FF>Haunter</color> will reveal the Impostors to everyone once its tasks are finished and a meeting is called.\nHowever if the Haunter gets clicked before finishing its tasks, the Impostors wont be revealed.\nThe Haunter's position may be revealed to Impostors after a certain amount of tasks done.";
            }
            else if (role == "Helper")
            {
                return "The <color=#7cb6c2>Helper</color> is a ghost role obtainable after death.\nThe Helper can use its ability to alert a living Crewmate of a nearby danger.\nThe alerted Crewmate will have a temporary speed boost and will know that they have been alerted through a pop up on their screen.";
            }
            else if (role == "Guardian")
            {
                return "The <color=#67bb43>Guardian</color> is a ghost role obtainable after death.\nThe Guardian can use its ability to protect a Crewmate from murder attempts for a short amount of time.";
            }
            else if (role == "Investigator")
            {
                return "The <color=#00B3B3FF>Investigator</color> can see the footprints of each players.";
            }
            else if (role == "Mystic")
            {
                return "The <color=#4D99E6FF>Mystic</color> will know whenever someone dies by showing a pop up on its screen.";
            }
            else if (role == "Oracle")
            {
                return "The <color=#BF00BFFF>Oracle</color> has two abilities.\nThe first one forces a player to confess to them, telling them that one of two players is evil.\nThe confessed player will also reveal their alignment to everyone if the Oracle dies.\nThe second ability, bless, makes the player immune to being voted out the next meeting.";
            }
            else if (role == "Seer")
            {
                return "The <color=#FFCC80FF>Seer</color> can use its ability to see if a player is evil (their name will be green or red).";
            }
            else if (role == "Snitch")
            {
                return "The <color=#D4AF37FF>Snitch</color> will know who the Impostors are when finish all of its tasks.\nThe Impostors may know who the Snitch is after a certain amount of tasks completed.";
            }
            else if (role == "Spy")
            {
                return "The <color=#CCA3CCFF>Spy</color> is able to see the color of each player on the admin map.";
            }
            else if (role == "Tracker")
            {
                return "The <color=#009900FF>Tracker</color> can place a tracker on a player indicating their position in real time.";
            }
            else if (role == "Trapper")
            {
                return "The <color=#A7D1B3FF>Trapper</color> can place traps on the map.\nIf players walk in a trap for a certain amount of time, their role will be revealed to the Trapper when the meeting starts.\nBut the Trapper won't know who has what role.";
            }
            else if (role == "Watcher")
            {
                return "The <color=#33FF66FF>Watcher</color> is a crewmate who can watch other players. They will see all players who interact with each player they watch.";
            }
            else if (role == "Fighter")
            {
                return "The <color=#9358e6>Fighter</color> can kill like the Sheriff.\nHowever if he mistakenly kills a Crewmate, the Fighter will turn into a <color=#FF0000>Madmate</color> and will now be aligned with the Impostors.";
            }
            else if (role == "Hunter")
            {
                return "The <color=#29AB87FF>Hunter</color> can stalk a player.\nIf that player interacts with anyone, the Hunter will know it and will be able to kill them.\nIf the Hunter is voted out, the last player who voted for the Hunter will die.";
            }
            else if (role == "Jailor")
            {
                return "The <color=#61a37a>Jailor</color> can use its ability to Jail someone.\nThe Jailed player will instantly be notified and will be totally blocked from using any special ability during the round and the next meeting if they're still jailed.\nThe Jailor can discuss with the Jailed player through a custom chat but the Jailor's identity won't be revealed to them.\nThey can also choose to release its target at any time by pressing another button and can then jail someone else but this will stop the role block on the previous jailed player.\nDuring the meeting, the Jailor can choose to execute their target but if they mistakenly execute a Crewmate, they will lose the ability to execute for the rest of the game and might even die depending on settings.";
            }
            else if (role == "Deputy")
            {
                return "The <color=#FFCC00FF>Deputy</color> can use its camp ability on a player during the round.\nThen, if the players dies, the Deputy can try to shoot their killer during the next meeting which will either kill the killer if the Deputy is right or do nothing.";
            }
            else if (role == "Knight")
            {
                return "The <color=#ffd082>Knight</color> can kill someone once during the game without any consequence.\nBut since this can be used once, the Knight has to choose wisely!";
            }
            else if (role == "Sheriff")
            {
                return "The <color=#FFFF00FF>Sheriff</color> can shoot someone.\nIf that player was an Impostor, the player will die but if the player was a Crewmate, the Sheriff will die instead.";
            }
            else if (role == "VampireHunter")
            {
                return "The <color=#B3B3E6FF>Vampire Hunter</color> can stake players.\nIf that player is a converted Vampire, the player will get its old role back.\nBut if the player is the original Vampire, the Vampire Hunter will kill them.\nIf that player is not a Vampire, nothing happens and the Vampire Hunter loses a stake.\nIf the Vampire Hunter tries to kill a non Vampire without any stake left, it might end up dying.";
            }
            else if (role == "Veteran")
            {
                return "The <color=#998040FF>Veteran</color> can use its ability to go on alert.\nWhile alerted, any player that interacts with the Veteran will be killed.";
            }
            else if (role == "Astral")
            {
                return "The <color=#a703ae>Astral</color> can use its ability to become a ghost and leave its body for a while.\nBut the Astral will still die if anyone attacks their body.";
            }
            else if (role == "Lookout")
            {
                return "The <color=#33FF66FF>Lookout</color> can use its ability to watch other players in real time.\nDepending on the settings, the Watched player may know when they are being Watched.";
            }
            else if (role == "Vigilante")
            {
                return "The <color=#FFFF99FF>Vigilante</color> can guess the role of Impostors during the meeting to kill them.";
            }
            else if (role == "Altruist")
            {
                return "The <color=#660000FF>Altruist</color> can sacrifice himself to revive someone else.";
            }
            else if (role == "Medic")
            {
                return "The <color=#006600FF>Medic</color> can cast a shield on a player to protect them from murder attempts.\nThe Medic will know if the shield breaks.\nThe Shield will instantly break if the Medic dies.\nThe Medic also gains informations when reporting a dead body.";
            }
            else if (role == "Doctor")
            {
                return "The <color=#00EE3C>Doctor</color> can revive dead players when they find dead bodies.\nDepending on settings, the Doctor may have to be in a medical area in order to revive someone, in that case, the doctor will have a drag button to drag the body to the area.\nNote that Revived player will not be able to speak anymore nor vote during meetings.";
            }
            else if (role == "Crusader")
            {
                return "The <color=#815230>Crusader</color> can crusade a player.\nIf that player gets attacked, the Crusader will instantly teleport and kill the attacker.";
            }
            else if (role == "Bodyguard")
            {
                return "The <color=#255bc9>Bodyguard</color> will protect players from attacks in a certain radius.\nIf someone gets attacked near the Bodyguard, the Bodyguard will kill the attacker and self-sacrifice instead.";
            }
            else if (role == "Paranoïac" || role == "Paranoiac")
            {
                return "The <color=#0a7eae>Paranoïac</color> can hide in vents.\nThe Paranoïac can also call a meeting from anywhere on the map.";
            }
            else if (role == "Cleric")
            {
                return "the <color=#00FFB3FF>Cleric</color> can protect a player from murder attempts for some seconds and cleanse them to remove all bad effects on them (douse, infect, hysteria...).";
            }
            else if (role == "Plumber")
            {
                return "The <color=#CC6600FF>Plumber</color>, can flush vents, ejecting all players currently in vent.\nThe Plumber can also barricade a vent making it unusable for the rest of the game.";
            }
            else if (role == "Engineer")
            {
                return "The <color=#FFA60AFF>Engineer</color> can use the vents and fix sabotages.";
            }
            else if (role == "TimeLord")
            {
                return "The <color=#0f00b8>Time Lord</color> can rewind the time, making you undo your previous movements while also reviving players that died at that moment.";
            }
            else if (role == "Imitator")
            {
                return "The <color=#B3D94DFF>Imitator</color> can imitate the role of dead Crewmates each meeting.";
            }
            else if (role == "Warden")
            {
                return "The <color=#9900FFFF>Warden</color> can fortify a player, making the Warden and them noticed when someone interacts with them.";
            }
            else if (role == "President")
            {
                return "The <color=#704FA8FF>President</color> can abstain its vote for the next round.";
            }
            else if (role == "Medium")
            {
                return "The <color=#A680FFFF>Medium</color> can use its ability to see the ghosts of latest deads.\nDeads will be noticed when the Medium sees them.";
            }
            else if (role == "Prosecutor")
            {
                return "The <color=#B38000FF>Prosecutor</color> can choose to exile someone whenever they want to resulting in instantly ejecting them.";
            }
            else if (role == "Swapper")
            {
                return "The <color=#66E666FF>Swapper</color> can swap the votes of two players.";
            }
            else if (role == "Transporter")
            {
                return "The <color=#00EEFFFF>Transporter</color> can transport two players, making them switch place with each other.";
            }
            else if (role == "Informant")
            {
                return "The <color=#4CFAAB>Informant</color> can access vitals or admin from anywhere even if the map doesn't have any vitals.";
            }
           
            // neutrals
            else if (role == "Amnesiac")
            {
                return "The <color=#80B2FFFF>Amnesiac</color> is a roleless Neutral.\nThe goal of the Amnesiac is to find a dead body and Remember its old role by taking the role of the dead player.\nThe Amnesiac may have arrows pointing to dead bodies to help.\nIf the Amnesiac doesn't find any role before the game ends, it loses.";
            }
            else if (role == "Shifter")
            {
                return "The <color=#AAAAAA>Shifter</color> is a Neutral role which can shift its role with another player.\nThe Shifter will get the shifted's role and the shifted will become a Shifter.\nShifter can't be guessed.";
            }
            else if (role == "Mercenary")
            {
                return "The <color=#8C6699FF>Mercenary</color>, can guard players and get rewarded with gold when someone interacts with one of the guarded players.\nThe currency can then be used to bribe other players.\nIf one of the bribed players then wins, the Mercenary wins too as long as they are not Lovers or Neutral Evil Roles.";
            }
            else if (role == "GuardianAngel")
            {
                return "The <color=#B3FFFFFF>Guardian Angel</color> is a Neutral role that is assigned a target that must stay alive in order to win.\nTo help its target, the Guardian Angel can temporarily shield them making them invinsible to murder attempts.";
            }
            else if (role == "Survivor")
            {
                return "The <color=#FFE64DFF>Survivor</color> is a Neutral role which wins with anyone as long as they stay alive until the game ends.\nThe Survivor has a vest ability to shield them from murder attempts temporarily.";
            }
            else if (role == "Doomsayer")
            {
                return "The <color=#00FF80FF>Doomsayer</color> is a Neutral role which wins by guessing the role of a certain amount of players during the meeting.";
            }
            else if (role == "Executioner")
            {
                return "The <color=#8C4005FF>Executioner</color> is a Neutral role which wins by ejecting a target assigned at the start of the game.";
            }
            else if (role == "Jester")
            {
                return "The <color=#FFBFCCFF>Jester</color> is a Neutral role which wins by being voted out and ejected.";
            }
            else if (role == "SoulCollector")
            {
                return "The <color=#99FFCCFF>Soul Collector</color> is a Neutral Killing role which can reap players, killing them instantly and leaving a soul on the ground.\nThe Soul Collector will win when thy are the last player standing.";
            }
            else if (role == "Vulture")
            {
                return "The <color=#916e6e>Vulture</color> is a Neutral role which wins by eating a certain amount of dead bodies.\nDepending on the settings, the vulture will have arrows pointing to dead bodies and may be able to vent.";
            }
            else if (role == "Phantom")
            {
                return "The <color=#662962FF>Phantom</color> is a Neutral role obtainable after death that will win by finishing all of its tasks without getting clicked by a player.";
            }
            else if (role == "Troll")
            {
                return "The <color=#c27d5a>Troll</color> is a Neutral role that has a troll ability.\nWhen using it, it will force the closest player to kill the Troll.\nIf that player gets ejected next meeting, the Troll will win and override any other win condition (executioner, jester...).";
            }
            else if (role == "Arsonist")
            {
                return "The <color=#FF4D00FF>Arsonist</color> is a Neutral role which can douse and then ignite players, killing all doused at the same time.\nThe Arsonist will win when they are the last player standing.";
            }
            else if (role == "Mutant")
            {
                return "The <color=#b6eb5e>Mutant</color> is a Neutral role which can kill but with a long cooldown.\nHowever, the Mutant can Transform, gaining Impostor vision, a short kill cooldown and maybe vent ability.\nBut everyone will see them as Transformed.\nThe Mutant will win when they are the last player standing.";
            }
            else if (role == "Infectious")
            {
                return "The <color=#bf9000>Infectious</color> is a Neutral role which can kill and Infect players.\nInfecting a player will slowly kill them by adding one stage of the Infection after each meeting.\nOn stage 1, nothing happens, on stage 2, the player will be slower, on stage 3, the player's vision will be much reduced, and on stage 4, the player will die.\nThe Infection will stop if the Infectious gets ejected.\nThe Infectious will also have a shofter kill cooldown when killing an infected.\nThe Infectious will win when they are the last player standing.";
            }
            else if (role == "Plaguebearer")
            {
                return "The <color=#E6FFB3FF>Plaguebearer</color> is a Neutral role which can Infect players.\nThe goal of the Plaguebearer is to infect all players to become a <color=#4c4c4c>Pestilence</color>.";
            }
            else if (role == "Attacker")
            {
                return "The <color=#c73a00>Attacker</color> is a Neutral role which has no special ability.\nThe goal of the Attacker is to finish all of its tasks to become a <color=#8bce23>Terrorist</color>.";
            }
            else if (role == "Terrorist")
            {
                return "The <color=#8bce23>Terrorist</color> is a Neutral role which can kill and may be able to vent.\nThe Terrorist is also immune to being voted out and unguessable, meaning that the only way to get rid of the Terrorist is by killing it.\nThe Terrorist wins by being the last player standing.";
            }
            else if (role == "Pestilence")
            {
                return "The <color=#4c4c4c>Pestilence</color> is the most powerful Neutral role.\nThe Pestilence can kill and is immune to murder attempts.\nIf someone tries to interact with the Pestilence, the attack will reverse and kill the attacker instead.\nThe Pestilence can't even die to lovers death nor be guessed.\nThe only way to kill the Pestilence is by ejection.\nThe Pestilence wins by being the last player standing.";
            }
            else if (role == "SerialKiller")
            {
                return "The <color=#0c20d5>Serial Killer</color> is a Neutral role which can kill and may be able to vent.\nThe Serial Killer may also be able to Convert someone to Serial Killer depending on the settings.\nThe Serial Killer team wins by being the last players standing.";
            }
            else if (role == "Doppelganger")
            {
                return "The <color=#fadd84>Doppelganger</color> is a Neutral role which can kill and may be able to vent.\nBut the Doppelganger will switch identity with whoever it kills making it harder to discover the real Doppelganger.\nThe Doppelganger wins by being the last player standing.";
            }
            else if (role == "Glitch" || role == "TheGlitch")
            {
                return "The <color=#00FF00FF>Glitch</color> is a Neutral role which can kill and may be able to vent.\nThe Glitch can also morph into other players using its mimic abiility and can hack players, making them unable to use their abilities for a while.\nThe Glitch wins by being the last player standing.";
            }
            else if (role == "Vampire")
            {
                return "The <color=#262626FF>Vampire</color> is a Neutral role which can kill and bite players.\nWhen the Vampire bites someone, it will convert them into Vampire, if successful, else it will just reset its cooldown.\nThe Vampire team wins by being the last players standing.";
            }
            else if (role == "Maul")
            {
                return "The <color=#A86629FF>Maul</color> is a Neutral role which can only kill when Rampages.\nThe Maul can press its ability to go on Rampage, and will have a short kill cooldown and may even be able to vent during this period of time.\nThe Maul wins by being the last player standing.";
            }
            else if (role == "Juggernaut")
            {
                return "The <color=#8c004c>Juggernaut</color> is a Neutral role which can kill and will have its cooldown decreased by each kill.\nThe Juggernaut wins by being the last player standing.";
            }
            // impostors
            else if (role == "Wraith")
            {
                return "The <color=#FF0000FF>Wraith</color> is an Impostor role obtainable after death.\nAs the Wraith, you will kill a random non-Impostor living player after completing all of your assigned tasks before getting clicked.";
            }
            else if (role == "Freezer")
            {
                return "The <color=#FF0000FF>Freezer</color> is an Impostor role obtainable after death.\nAs the Freezer, you can freeze alive players in place making them unable to move for a short period of time.";
            }
            else if (role == "Blinder")
            {
                return "The <color=#FF0000FF>Blinder</color> is an Impostor role obtainable after death.\nAs the Blinder, you can make players temporarily blind and unable to see anything.";
            }
            else if (role == "Escapist")
            {
                return "The <color=#FF0000FF>Escapist</color> is an Impostor role which can place a point on the map and teleport back to it after.";
            }
            else if (role == "Grenadier")
            {
                return "The <color=#FF0000FF>Grenadier</color> is an Impostor role which can temporarily blind the vision of all nearby Crewmates by throwing down a grenade.";
            }
            else if (role == "Morphling")
            {
                return "The <color=#FF0000FF>Morphling</color> is an Impostor role which can morph into another player, taking their identity.";
            }
            else if (role == "Hypnotist")
            {
                return "The <color=#FF0000FF>Hypnotist</color> is an Impostor role which can hypnotise players.\nThen, during the meeting, the Hypnotist can release mass hysteria making hypnotised players see other players as either camouflaged, invisible or themselves.";
            }
            else if (role == "Assassin")
            {
                return "The <color=#FF0000FF>Assassin</color> is an either an Impostor role or an ability for killers depending on the settings.\nThe Assassin can guess the role of the players during the meeting, instantly killing them.\nHowever, if the Assassin guesses incorrectly, it will die instead.";
            }
            else if (role == "Swooper")
            {
                return "The <color=#FF0000FF>Swooper</color> is an Impostor role which can temporarily turn Invisible.";
            }
            else if (role == "Venerer")
            {
                return "The <color=#FF0000FF>Venerer</color> is an Impostor role which unlocks abilities based on its kill count.\nThe more kills it gets, the more ability it gets too.";
            }
            else if (role == "Poisoner")
            {
                return "The <color=#FF0000FF>Poisoner</color> is an Impostor role which can Poison players instead of performing regular kills, killing them after a split seconds.";
            }
            else if (role == "Kamikaze")
            {
                return "The <color=#FF0000FF>Kamikaze</color> is an Impostor role which can use its ability to go on a kamikaze attack, killing all nearby players but also themselves.";
            }
            else if (role == "Shooter")
            {
                return "The <color=#FF0000FF>Shooter</color> is an Impostor role which can store bullets instead of instantly killing a player.\nThis will reset the Shooter's kill cooldown but will allow the Shooter to have no kill cooldown the next time it kills and potentially performing multiple kills at once.";
            }
            else if (role == "Bomber")
            {
                return "The <color=#FF0000FF>Bomber</color> is an Impostor role which can place a bomb on the map, killing all players in its radius.";
            }
            else if (role == "Traitor")
            {
                return "The <color=#FF0000FF>Traitor</color> is an Impostor role assigned to a random Crewmate if all Impostors die too quickly.\nThe Traitor has no special ability other than regular Impostors.";
            }
            else if (role == "Warlock")
            {
                return "The <color=#FF0000FF>Warlock</color> is an Impostor role which can charge up its kill cooldown.\nWhen the Warlock kills when its ability is fully charged, the Warlock will be able to perform multiple kills at once until the charge runs out.";
            }
            else if (role == "Mafioso")
            {
                return "The <color=#FF0000FF>Mafioso</color> is an Impostor role which will start without any ability other than Impostor vision.\nBut if all of the Mafioso's teammates are dead, the Mafioso will unlock all its abilities and will also have a shorter kill cooldown than regular Impostors.";
            }
            else if (role == "Reviver")
            {
                return "The <color=#FF0000FF>Reviver</color> is an Impostor role which can revive inside of someone else's dead body the round where it dies.\nIf the Reviver manages to find a body, it will take the player's identity.\nThis ability is only usable once and only the round where the Reviver has died.";
            }
            else if (role == "Blackmailer")
            {
                return "The <color=#FF0000FF>Blackmailer</color> is an Impostor role which can make a player unable to talk during the meeting.";
            }
            else if (role == "Converter")
            {
                return "The <color=#FF0000FF>Converter</color> is an Impostor role which can use its ability to revive a dead Crewmate and turn them into a <color=#FF0000>Madmate</color>.";
            }
            else if (role == "Janitor")
            {
                return "The <color=#FF0000FF>Janitor</color> is an Impostor role which can clean dead bodies, erasing any trace of crime.";
            }
            else if (role == "Witch")
            {
                return "The <color=#FF0000FF>Witch</color> is an Impostor role which can cast a spell on a player instead of regulary killing them.\nEveryone will know who the players with the spells are in the meeting, and they will all die unless the witch is ejected / killed that meeting.";
            }
            else if (role == "Conjurer")
            {
                return "The <color=#FF0000FF>Conjurer</color> is an Impostor role which can cast a curse on a player.\nThen, the Conjurer can use that player to kill the closest person near that player.\nThis means that the Conjurer will kill using the cursed player's identity.";
            }
            else if (role == "BountyHunter")
            {
                return "The <color=#FF0000FF>Bounty Hunter</color> is an Impostor role which has a target assigned at the start of the game that switchs after a certain amount of time.\nThe Bounty Hunter will have an arrow pointing to its target.\nIf the Bounty Hunter kills its target, its next cooldown will be short, else it will be long.";
            }
            else if (role == "Miner")
            {
                return "The <color=#FF0000FF>Miner</color> is an Impostor role which can use its ability to create new vents on the map.";
            }
            else if (role == "Undertaker")
            {
                return "The <color=#FF0000FF>Undertaker</color> is an Impostor role which can drag dead bodies around to hide them.";
            }
            else if (role == "Manipulator")
            {
                return "The <color=#FF0000FF>Manipulator</color> is an Impostor role which can use its ability to Manipulate a Crewmate.\nThen, the Manipulator can Control this Crewmate, allowing them to move the player and kill with their identity.\nThe Manipulation will stop if the Manipulator / Manipulated gets killed or a meeting is called.";
            }
            else if (role == "Madmate")
            {
                return "<color=#FF0000FF>Madmates</color> are old Crewmates who joined the Impostor team.\nIt could be either voluntarily (like the Fighter), or they were forced to (conversion).\nMadmates will now have Impostor vision, venting perks and may even be able to access the Impostor chat.\nMadmates may get special abilities depending on their old role (ex: Mad Engineer can sabotage instead of fixing sabotages).";
            }
            else if (role == "Noclip")
            {
                return "The <color=#FF0000FF>Noclip</color> an Impostor role which can walk trought walls.";
            }
            // coven
            else if (role == "Coven")
            {
                return "The <color=#bf5fff>Coven</color> is the regular Coven role.\nIt has no special abilities other than killing or sabotaging and Impostor vision.\nThe goal of Covens is to kill or convert every living non-Coven member.";
            }
            else if (role == "Ritualist")
            {
                return "The <color=#bf5fff>Ritualist</color> is a Coven role which can guess the role of other players during the meeting to kill them so basically, the Ritualist has the <color=#FF0000>Assassin</color> ability.";
            }
            else if (role == "HexMaster")
            {
                return "The <color=#bf5fff>Hex Master</color> is a Coven role which can hex players.\nThen, the Hex Master can choose to hex bomb them at anytime, killing all the hexed at once.\nEveryone will know who the hexed are in the meeting.";
            }
            else if (role == "CovenLeader")
            {
                return "The <color=#bf5fff>Coven Leader</color> is a Coven role which can Convert a player to the <color=#bf5fff>Coven</color> Faction.\nThe Coven Leader is also the only Coven role that it passively able to use the vents.";
            }
            else if (role == "Spiritualist")
            {
                return "The <color=#bf5fff>Spiritualist</color> is a Coven role which can use its button to Control a player.\nIf the Controlled player tries to kill any Coven member, the Spiritualist will kill them instead.";
            }
            else if (role == "VoodooMaster")
            {
                return "The <color=#bf5fff>Voodoo Master</color> is a Coven role which can use its ability on a player during the round.\nThen, the next meeeting, that player will be forced to vote whoever the Voodoo Master votes for.";
            }
            else if (role == "PotionMaster")
            {
                return "The <color=#bf5fff>Potion Master</color> is a Coven role which can get a random potion and drink it.\nThe Shield potion will grant the Potion Master a temporary shield, the invisibility potion will make the Potion Master invisible, the Strength potion will make it have a short kill cooldown and able to vent and the Speed potion will grant the Potion Master a temporary speed boost.";
            }
            else return "The role you have searched for could not be found.\nMake sure you typed the role correctly. (Do not use spaces and use majs ex: /r SerialKiller)";
        }
        public static string GetModifiersInfos(string modifiers)
        {
            if (modifiers == "Aftermath")
            {
                return "<color=#A6FFA6FF>Aftermath</color> is a Crewmate Modifier which forces the killer to use its special ability if the killer does have one when being killed.";
            }
            else if (modifiers == "Bait")
            {
                return "<color=#00B3B3FF>Bait</color> is a Crewmate Modifier which forces the killer to self report when being killed.";
            }
            else if (modifiers == "Celebrity")
            {
                return "<color=#FF9999FF>Celebrity</color> is a Crewmate Modifier that will send a message showing where, when, and how you died when the next meeting starts.";
            }
            else if (modifiers == "Diseased")
            {
                return "<color=#808080FF>Diseased</color> is a Crewmate Modifier which multiplies the kill cooldown of the killer when being killed.";
            }
            else if (modifiers == "Frosty")
            {
                return "<color=#99FFFFFF>Frosty</color> is a Crewmate Modifier which makes the killer temporarily slow when being killed.";
            }
            else if (modifiers == "Multitasker")
            {
                return "<color=#FF804DFF>Multitasker</color> is a Crewmate Modifier which makes you see through the task pop up when doing a task.";
            }
            else if (modifiers == "Torch")
            {
                return "<color=#FFFF99FF>Torch</color> is a Crewmate Modifier which gives you Impostor vision.";
            }
            else if (modifiers == "Taskmaster")
            {
                return "<color=#669966FF>Taskmaster</color> is a Crewmate Modifier which completes a random task after each meeting.";
            }
            else if (modifiers == "Vengeful")
            {
                return "<color=#8d0000>Vengeful</color> is a Crewmate Modifier which allows you to kill someone once after completing all of your tasks.";
            }
            else if (modifiers == "Buttonbarry")
            {
                return "<color=#E600FFFF>Button Barry</color> is a Global Modifier which allows you to call a meeting from anywhere once.";
            }
            else if (modifiers == "Drunk")
            {
                return "<color=#758000FF>Drunk</color> is a Global Modifier which you have reversed control.";
            }
            else if (modifiers == "Flash")
            {
                return "<color=#FF8080FF>Flash</color> is a Global Modifier which makes you faster than everyone else.";
            }
            else if (modifiers == "Satellite")
            {
                return "<color=#0099CCFF>Satellite</color> is a Global Modifier which allows you to locate the position of all the dead bodies once.";
            }
            else if (modifiers == "Giant")
            {
                return "<color=#FFB34DFF>Giant</color> is a Global Modifier which makes you bigger and maybe slower than other players.";
            }
            else if (modifiers == "Mini")
            {
                return "<color=#CCFFE6FF>Mini</color> is a Global Modifier which makes you much shorter than other players.";
            }
            else if (modifiers == "Lovers")
            {
                return "<color=#FF66CCFF>Lovers</color> is a Global Modifier which makes two players fall in lover.\nThe Lovers will now both win together, whatever their side is and may also both die if one of them dies.";
            }
            else if (modifiers == "Radar")
            {
                return "<color=#FF0080FF>Radar</color> is a Global Modifier which makes you always have an arrow pointing to the nearest player.";
            }
            else if (modifiers == "Scientist")
            {
                return "<color=#5441b3>Scientist</color> is a Global Modifier which makes you see the players death reasons.";
            }
            else if (modifiers == "Shy")
            {
                return "<color=#FFB3CCFF>Shy</color> is a Global Modifier which makes you slowly turn invisible when not moving.";
            }
            else if (modifiers == "Sixthsense")
            {
                return "<color=#D9FF8CFF>Sixth Sense</color> is a Global Modifier which makes you know when anyone interacts with you.";
            }
            else if (modifiers == "Sleuth")
            {
                return "<color=#803333FF>Sleuth</color> is a Global Modifier which makes you know the role of dead players you report.";
            }
            else if (modifiers == "Superstar")
            {
                return "<color=#ffca00>Superstar</color> is a Global Modifier which will alert everyone when dying and show an arrow pointing to its dead bodies.";
            }
            else if (modifiers == "Motionless")
            {
                return "<color=#008591>Motionless</color> is a Global Modifier which makes you not move when a meeting is called.\nThis means that you will respawn where you were before the meeting when the meeting ends.";
            }
            else if (modifiers == "Tiebreaker")
            {
                return "<color=#99E699FF>Tiebreaker</color> is a Global Modifier which makes your vote count twice whenever there's a tie.";
            }
            else if (modifiers == "Disperser")
            {
                return "<color=#FF0000>Disperser</color> is an Impostor Modifier which makes you able to Teleport all players to a random vent once in the game.";
            }
            else if (modifiers == "Bloodlust")
            {
                return "<color=#FF0000>Bloodlust</color> is an Impostor Modifier which makes your kill cooldown divided by two after killing two players in the same round.";
            }
            else if (modifiers == "Doubleshot")
            {
                return "<color=#FF0000>Double Shot</color> is an Impostor Modifier which makes you able to guess the role of a player wrong as Assassin once in the game without any consequence.";
            }
            else if (modifiers == "Lucky")
            {
                return "<color=#FF0000>Lucky</color> is an Impostor Modifier which makes your kill cooldowns completely random.";
            }
            else if (modifiers == "Saboteur")
            {
                return "<color=#FF0000>Saboteur</color> is an Impostor Modifier which reduces the Sabotage cooldown of the Impostor.";
            }
            else if (modifiers == "Tasker")
            {
                return "<color=#FF0000>Tasker</color> is an Impostor Modifier which you can do tasks.";
            }
            else if (modifiers == "Underdog")
            {
                return "<color=#FF0000>Underdog</color> is an Impostor Modifier which makes your kill cooldowns be long when there's more than 1 Impostor alive, but short when your the only Impostor alive.";
            }
            else return "The modifier you have searched for could not be found.\nMake sure you typed the modifier correctly. (Do not use spaces and use majs ex: /m SixthSense)";
        }
    }
}