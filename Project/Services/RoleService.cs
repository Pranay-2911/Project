using Microsoft.EntityFrameworkCore;
using Project.Models;
using Project.Repositories;

namespace Project.Services
{
    public class RoleService : IRoleService
    {
        private readonly IRepository<Role> _repository;

        public RoleService(IRepository<Role> repository)
        {
            _repository = repository;
        }
        public Guid AddRole(Role role)
        {
            _repository.Add(role);
            return role.Id;
        }

        public bool DeleteRole(Guid id)
        {
            var role = _repository.Get(id);
            if (role != null)
            {
                _repository.Delete(role);
                return true;
            }
            return false;

        }

        public Role GetById(Guid id)
        {
            return _repository.Get(id);
        }

        public List<Role> GetRoles()
        {
            return _repository.GetAll().ToList();
        }

        public bool UpdateRole(Role role)
        {
             var existingRole = _repository.GetAll().AsNoTracking().Where(r => r.Id == role.Id);
            if (existingRole != null)
            {
                _repository.Update(role);
                return true;
            }
            return false;

        }
    }
}
