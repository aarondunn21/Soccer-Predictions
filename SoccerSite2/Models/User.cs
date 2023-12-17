using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace SoccerSite2.Models
{
    public class User
    {
        [Key]
        public string Id { get; set; }

        public string UserMatch { get; set; }
        public double Balance { get; set; }



/*        public ICollection<UserBet> BetList { get; } = new List<UserBet>();*/
    }
}
