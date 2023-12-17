using System.ComponentModel.DataAnnotations;
namespace SoccerSite2.Models
{
    public class UserBet
    {
        [Key]
        public int BetId { get; set; }
        public string UserId { get; set; }
        public int BetSearchId { get; set; }
        public double Odds { get; set; }
        public double Amount { get; set; }
        public bool Result { get; set; }
        public bool Completed { get; set; }
        public String FixtureName { get; set; }
        public DateTime Timestamp { get; set; }
        public string TeamChoice { get; set; }
    }

}
