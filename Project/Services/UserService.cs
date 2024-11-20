using Microsoft.EntityFrameworkCore;
using Project.Models;
using Project.Repositories;

namespace Project.Services
{
    public class UserService : IUserService
    {
        private readonly IRepository<User> _repository;
        public UserService(IRepository<User> repository)
        {
            _repository = repository;
        }

        public Guid AddRole(User user)
        {
            _repository.Add(user);
            return user.Id;
        }

        public bool DeleteRole(Guid id)
        {
            var user = _repository.Get(id);
            if (user != null)
            {
                _repository.Delete(user);
                return true;
            }
            return false;

        }

        public User GetById(Guid id)
        {
            return _repository.Get(id);
        }

        public List<User> GetRoles()
        {
            return _repository.GetAll().ToList();
        }

        public bool UpdateRole(User user)
        {
            var existingUser = _repository.GetAll().AsNoTracking().Where(u => u.Id == user.Id);
            if (existingUser != null)
            {
                _repository.Update(user);
                return true;
            }
            return false;

        }
    }
}
