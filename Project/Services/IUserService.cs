using Project.DTOs;
using Project.Models;

namespace Project.Services
{
    public interface IUserService
    {
        public List<UserDto> GetUsers();
        public User GetById(Guid id);
        public Guid AddUser(UserDto userDto);
        public bool DeleteUser(Guid id);
        public bool UpdateUser(UserDto userDto);
    }
}
