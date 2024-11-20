using Project.Models;

namespace Project.Services
{
    public interface IRoleService
    {
        public List<Role> GetRoles();
        public Role GetById(Guid id);
        public Guid AddRole(Role role);
        public bool DeleteRole(Guid id);
        public bool UpdateRole(Role role);
    }
}
