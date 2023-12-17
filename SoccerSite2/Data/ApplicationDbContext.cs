using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SoccerSite2.Models;

namespace SoccerSite2.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<TodayBets> TodayBets { get; set; }

        public DbSet<UserBet> UserBets { get; set; }

        public DbSet<Bet> Bet { get; set; }

        public DbSet<User> User { get; set; }
    }
}