namespace SoccerSite2.Models
{
    public class UserBetsPageModel
    {
        public IEnumerable<UserBet> Bets { get; set; }
        public User userAccount { get; set; }
    }
}
