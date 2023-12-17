using System.ComponentModel.DataAnnotations;

namespace SoccerSite2.Models
{
    public class Bet
    {
        [Key]
        public int BetId { get; set; }


        public TodayBets TodayBets { get; set; }

        public string BetName { get; set; }

        public double HomeOdd { get; set; }

        public double AwayOdd { get; set; }

        public double DrawOdd { get; set; }
        public int Fixture { get; set; }
        public DateTime date { get; set; }

    }
}
