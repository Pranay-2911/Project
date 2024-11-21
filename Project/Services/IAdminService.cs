using Project.DTOs;

namespace Project.Services
{
    public interface IAdminService
    {
        public Guid Add(AdminDto adminDto);
        public bool Update(AdminDto adminDto);
        public List<AdminDto> GetAll();
        public AdminDto Get(Guid id);
        public bool Delete(Guid id);
    }
}
