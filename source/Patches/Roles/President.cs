using System.Collections.Generic;

namespace TownOfUsEdited.Roles
{
    public class President : Role
    {
        public List<byte> ExtraVotes = new List<byte>();

        public President(PlayerControl player) : base(player)
        {
            Name = "President";
            ImpostorText = () => "Save your votes to vote multiple times";
            TaskText = () => "Save your votes to President dump someone";
            Color = Patches.Colors.Mayor;
            RoleType = RoleEnum.President;
            AddToRoleHistory(RoleType);
            VoteBank = CustomGameOptions.PresidentVoteBank;
            Alignment = Alignment.CrewmatePower;
        }

        public int VoteBank { get; set; }
        public bool SelfVote {  get; set; }
        public bool VotedOnce { get; set; }
        
        public PlayerVoteArea Abstain { get; set; }

        public bool CanVote => VoteBank > 0 && !SelfVote;
    }
}