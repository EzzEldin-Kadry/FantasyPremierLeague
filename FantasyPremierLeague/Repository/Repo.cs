using FantasyPremierLeague.Data;
using FantasyPremierLeague.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace FantasyPremierLeague.Repository
{
    public class Repo<T> : IRepo<T> where T : class
    {
        protected readonly AppDBContext _context;
        internal DbSet<T> _dbSet;
        public Repo(AppDBContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }
        public ICollection<T> GetAll(Expression<Func<T, bool>> filter = null)
        {
            IQueryable<T> query = _dbSet;
            if (filter is not null)
                query = query.Where(filter);
            return query.ToList();
        }

        public bool Save()
        {
            int state = _context.SaveChanges();
            return state > 0 ? true : false;
        }
        public bool CreateEntity(T entity)
        {
            _context.Add(entity);
            return Save();
        }

        public bool UpdateEntity(T entity)
        {
            _context.Update(entity);
            return Save();
        }

        public bool DeleteEntity(T entity)
        {
            _context.Remove(entity);
            return Save();
        }
    }
}
