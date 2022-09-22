using FantasyPremierLeague.Data;
using FantasyPremierLeague.Models;
using FantasyPremierLeague.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace FantasyPremierLeague.Repository
{
    public class RepoTeam : Repo<Team>, IRepoTeam
    {
        public RepoTeam(AppDBContext context) : base(context)
        {
        }

        public Team GetTeam(int id)
        {
            return _context.Teams.Include(x => x.Players).ThenInclude(x => x.Statistics).SingleOrDefault(x => x.Id == id);
        }
        public bool HasTeam(int id)
        {
            return _context.Teams.Any(x => x.Id == id);
        }
        public ICollection<Team> GetTeamsSortedbyNames(Expression<Func<Team, bool>> filter = null)
        {
            IQueryable<Team> query = _dbSet;
            if (filter is not null)
                query = query.Where(filter);
            query = query.Include(x => x.Players).ThenInclude(x => x.Statistics);
            return query.OrderBy(x => x.Name).ToList();
        }
    }
}
