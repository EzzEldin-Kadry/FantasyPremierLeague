using FantasyPremierLeague.Models;
using System.Linq.Expressions;

namespace FantasyPremierLeague.Repository.IRepository
{
    public interface IRepoPlayer : IRepo<Player>
    {
        ICollection<Player> GetPlayersSortedbyNames(Expression<Func<Player, bool>> filter = null);
        ICollection<Coach> GetCoachesOfPlayer(int id);
        Player GetPlayer(int id);
        Team GetTeambyPlayer(int id);
        bool HasPlayer(int id);
        Statistics GetStatistics(int playerId);
        
    }
}
