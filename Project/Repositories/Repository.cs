
using Microsoft.EntityFrameworkCore;
using Project.Data;

namespace Project.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ProjectContext _context;
        private readonly DbSet<T> _table;

        public Repository(ProjectContext context)
        {
            _context = context;
            _table = _context.Set<T>();
        }

        public void Add(T entity)
        {
            _table.Add(entity);
            _context.SaveChanges();
        }

        public int Delete(T entity)
        {
            _table.Remove(entity);
            return _context.SaveChanges();
        }

        public T Get(Guid id)
        {
            var entity = _table.Find(id);
            return entity;
        }
        public IQueryable<T> GetAll()
        {
            return _table.AsQueryable();
        }

        public void Update(T entity)
        {
            _table.Update(entity);
            _context.SaveChanges();
        }
    }
}
