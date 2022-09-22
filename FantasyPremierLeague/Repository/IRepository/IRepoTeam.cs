using FantasyPremierLeague.Models;
using System.Linq.Expressions;

namespace FantasyPremierLeague.Repository.IRepository
{
    public interface IRepoTeam : IRepo<Team>
    {
        ICollection<Team> GetTeamsSortedbyNames(Expression<Func<Team, bool>> filter = null);
        Team GetTeam(int id);
        bool HasTeam(int id);
    }
}
