using Project.Models;

namespace Project.Services
{
    public interface IUserService
    {
        public List<User> GetRoles();
        public User GetById(Guid id);
        public Guid AddRole(User user);
        public bool DeleteRole(Guid id);
        public bool UpdateRole(User user);
    }
}
