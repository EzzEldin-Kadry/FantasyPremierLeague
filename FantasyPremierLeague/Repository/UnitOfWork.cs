using FantasyPremierLeague.Data;
using FantasyPremierLeague.Repository.IRepository;

namespace FantasyPremierLeague.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDBContext _context;
        public UnitOfWork(AppDBContext context)
        {
            _context = context;
            Team = new RepoTeam(_context);
            Player = new RepoPlayer(_context);
            Coach = new RepoCoach(_context);
        }

        public IRepoTeam Team { get; set; }

        public IRepoPlayer Player { get; set; }

        public IRepoCoach Coach { get; set; }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
