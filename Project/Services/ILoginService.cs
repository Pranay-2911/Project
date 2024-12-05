using Project.Models;

namespace Project.Services
{
    public interface ILoginService
    {
        public User FindByUserName(string userName);


        public object FindUser(string role, Guid userId, ref string token, ref Guid id);


    }

}
