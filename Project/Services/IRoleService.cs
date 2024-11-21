using Project.DTOs;
using Project.Models;

namespace Project.Services
{
    public interface IRoleService
    {
        public List<RoleDto> GetRoles();
        public Role GetById(Guid id);
        public Guid AddRole(RoleDto roleDto);
        public bool DeleteRole(Guid id);
        public bool UpdateRole(RoleDto roleDto);
    }
}
