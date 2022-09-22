using FantasyPremierLeague.Data;
using FantasyPremierLeague.Models;
using FantasyPremierLeague.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace FantasyPremierLeague.Repository
{
    public class RepoCoach : Repo<Coach>, IRepoCoach
    {
        public RepoCoach(AppDBContext context) : base(context)
        {

        }


        public Coach GetCoach(int id)
        {
            return _context.Coachs.Include(x => x.CoachPlayers)
                                    .ThenInclude(x => x.Player)
                                    .ThenInclude(x => x.Team)
                                  .Include(x => x.CoachPlayers)
                                    .ThenInclude(x => x.Player)
                                    .ThenInclude(x => x.Statistics)
                                  .FirstOrDefault(x => x.Id == id);
        }
        public ICollection<Player> GetTeamOfCoach(int id)
        {
            return _context.CoachPlayers.Where(x => x.CoachId == id)
                                             .Include(x => x.Player.Team)
                                             .Include(x => x.Player.Statistics)
                                             .Select(x => x.Player)
                                             .ToList();
        }
        public bool AlreadyHasPlayers(int coachId, IList<int> player_IDs)
        {
            var coachPlayers = GetPlayer_IDsOfCoach(coachId);

            foreach (var item in player_IDs)
            {
                if (coachPlayers.Contains(item))
                    return true;
            }
            return false;
        }

        public bool AllPlayersMatched(int coachId, IList<int> player_IDs)
        {
            var coachPlayers = GetPlayer_IDsOfCoach(coachId);
            foreach (var item in player_IDs)
            {
                if (!coachPlayers.Contains(item))
                    return false;
            }
            return true;
        }
        public bool HasCoach(int id)
        {
            return _context.Coachs.Any(x => x.Id == id);
        }

        public bool AddPlayersToCoach(int coachId, IList<int> player_IDs)
        {
            _context.CoachPlayers.AddRange(PopulateCoachPlayersbyIds(coachId, player_IDs));
            return Save();
        }
        public bool DeletePlayersFromCoach(int coachId, IList<int> player_IDs)
        {
            _context.CoachPlayers.RemoveRange(PopulateCoachPlayersbyIds(coachId, player_IDs));
            return Save();
        }

        IList<CoachPlayers> PopulateCoachPlayersbyIds(int coachId,IList<int> player_IDs)
        {
            IList<CoachPlayers> list = new List<CoachPlayers>();
            foreach (var curPlayerId in player_IDs)
            {
                list.Add(new CoachPlayers()
                {
                    CoachId = coachId,
                    PlayerId = curPlayerId
                });

            }
            return list;
        }

        ISet<int> GetPlayer_IDsOfCoach(int coachId)
        {
            return _context.CoachPlayers.Where(x => x.CoachId == coachId)
                                                           .Select(x => x.PlayerId)
                                                           .ToHashSet();
        }

    }
}
