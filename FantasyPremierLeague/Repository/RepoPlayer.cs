using FantasyPremierLeague.Data;
using FantasyPremierLeague.Models;
using FantasyPremierLeague.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace FantasyPremierLeague.Repository
{
    public class RepoPlayer : Repo<Player>, IRepoPlayer
    {
        public RepoPlayer(AppDBContext context) : base(context)
        {

        }
        public ICollection<Player> GetPlayersSortedbyNames(Expression<Func<Player, bool>> filter = null)
        {
            IQueryable<Player> query = _dbSet;
            if (filter is not null)
                query = query.Where(filter);
            query = query.Include(x => x.Team).Include(x => x.Statistics);
            return query.OrderBy(x => x).ToList();
        }
        public Player GetPlayer(int id)
        {
            return _context.Players.Include(x => x.Team).Include(x => x.Statistics).SingleOrDefault(x => x.Id == id);
        }

        public Team GetTeambyPlayer(int id)
        {
            IQueryable<Player> query = _dbSet;
            query = query.Include(x => x.Team);
            var player = query.FirstOrDefault(x => x.Id == id);
            return player.Team;
        }

        public ICollection<Coach> GetCoachesOfPlayer(int id)
        {
           return _context.CoachPlayers.Where(x => x.PlayerId == id)
                                 .Select(x => x.Coach)
                                 .ToList();
        }

        public bool HasPlayer(int id)
        {
            return _context.Players.Any(x => x.Id == id);
        }

        public Statistics GetStatistics(int playerId)
        {
            return _context.Statistics.AsNoTracking().FirstOrDefault(x => x.PlayerId == playerId);
        }
    }
}
