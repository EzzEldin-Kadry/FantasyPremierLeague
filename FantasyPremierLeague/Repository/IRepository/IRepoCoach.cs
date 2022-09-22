using FantasyPremierLeague.Models;
using System.Linq.Expressions;

namespace FantasyPremierLeague.Repository.IRepository
{
    public interface IRepoCoach: IRepo<Coach>
    {
        Coach GetCoach(int id);
        ICollection<Player> GetTeamOfCoach(int id);
        bool AlreadyHasPlayers(int coachId, IList<int> player_IDs);
        bool AllPlayersMatched(int coachId, IList<int> player_IDs);
        bool AddPlayersToCoach(int coachId, IList<int> player_IDs);
        bool DeletePlayersFromCoach(int coachId, IList<int> player_IDs);
        bool HasCoach(int id);
    }
}
