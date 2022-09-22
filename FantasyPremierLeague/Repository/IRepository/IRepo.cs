using System.Linq.Expressions;

namespace FantasyPremierLeague.Repository.IRepository
{
    public interface IRepo<T> where T : class
    {
        ICollection<T> GetAll(Expression<Func<T,bool>> filter = null);
        bool CreateEntity(T entity);
        bool UpdateEntity(T entity);
        bool DeleteEntity(T entity);
        bool Save();
    }
}
