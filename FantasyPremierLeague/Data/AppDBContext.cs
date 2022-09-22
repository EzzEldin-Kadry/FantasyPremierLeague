using FantasyPremierLeague.Models;

using Microsoft.EntityFrameworkCore;

namespace FantasyPremierLeague.Data
{
    public class AppDBContext : DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options)
        {

        }
        public DbSet<Coach> Coachs { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<Statistics> Statistics { get; set; }
        public DbSet<CoachPlayers> CoachPlayers { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Team>().HasMany(x => x.Players).WithOne(x => x.Team);
            modelBuilder.Entity<Statistics>().HasOne(x => x.Player).WithOne(x => x.Statistics).HasForeignKey<Statistics>(x => x.PlayerId);
            modelBuilder.Entity<CoachPlayers>().HasKey(x => new
            {
                x.CoachId,x.PlayerId
            });
        }
    }
}
