using Project.Models;

namespace Project.Repositories
{
    public interface IRepository<T>
    {
        public void Add(T entity);
        public IQueryable<T> GetAll();
        public void Update(T entity);
        public T Get(Guid id);
        public int Delete(T entity);
    }
}
