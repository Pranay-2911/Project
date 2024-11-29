using Microsoft.EntityFrameworkCore;
using Project.Models;
using Project.Repositories;

namespace Project.Services
{
    public class LoginService: ILoginService
    {
        private readonly IRepository<User> _userRepo;
        private readonly IRepository<Admin> _adminRepo;
        private readonly IRepository<Employee> _employeeRepo;
        private readonly IRepository<Agent> _agentRepo;
        private readonly IRepository<Customer> _customerRepo;

        public LoginService(IRepository<User> userRepo, IRepository<Admin> adminRepo, IRepository<Employee> employeeRepo,
            IRepository<Agent> agentRepo, IRepository<Customer> customerRepo)
        {
            _adminRepo = adminRepo;
            _userRepo = userRepo;
            _employeeRepo = employeeRepo;
            _agentRepo = agentRepo;
            _customerRepo = customerRepo;

        }

        public User FindByUserName(string userName)
        {
            return _userRepo.GetAll().Include(u => u.Role).Where(u => u.UserName == userName).FirstOrDefault();       
        }

        public object FindUser(string role, Guid id)
        {
            if (role == "ADMIN")
            {
                return _adminRepo.GetAll().Where(u => u.UserId == id).FirstOrDefault();
            }
            else if (role == "AGENT")
            {
                return _agentRepo.GetAll().Where(u => u.UserId == id).FirstOrDefault();
            }
            else if (role == "Customer")
            {
                return _customerRepo.GetAll().Where(u => u.UserId == id).FirstOrDefault();
            }
            else if (role == "EMPLOYEE")
            {
                return _employeeRepo.GetAll().Where(u => u.UserId == id).FirstOrDefault();
            }
            return null;
        }

    }

}
