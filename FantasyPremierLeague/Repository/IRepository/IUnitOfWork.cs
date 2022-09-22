namespace FantasyPremierLeague.Repository.IRepository
{
    public interface IUnitOfWork
    {
        IRepoTeam Team { get; }
        IRepoPlayer Player { get; }
        IRepoCoach Coach { get; }
        void Save();
    }
}
