using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace SoccerSite2.Models
{
    public class TodayBets
    {
        [Key]
        public int Id { get; set; }
        public DateTime CreatedDateTime { get; set; } = DateTime.Now;
        /*public ICollection<Bet> Bets { get; } = new List<Bet>();*/
    }
}
