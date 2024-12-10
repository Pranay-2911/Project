using Project.DTOs;
using Project.Models;
using Project.Repositories;

namespace Project.Services
{
    public class ForgetPasswordService : IForgetPasswordService
    {

        private readonly IRepository<User> _userRepository;
        private readonly IRepository<Role> _roleRepository;
        private readonly IRepository<Employee> _employeeRepository;
        private readonly IRepository<Customer> _customerRepository;
        private readonly IRepository<Agent> _agentRepository;
        public ForgetPasswordService(IRepository<User> userReepository, IRepository<Role> roleRepository, IRepository<Employee> employeeRepository, IRepository<Customer> customerRepository, IRepository<Agent> agentRepository)
        {
            _roleRepository = roleRepository;
            _userRepository = userReepository;
            _employeeRepository = employeeRepository;
            _customerRepository = customerRepository;
            _agentRepository = agentRepository;
        }

        public Guid ChangePassword(ForgetPasswordDto forgetPasswordDto)
        {
            var user = _userRepository.GetAll().Where( u => u.UserName == forgetPasswordDto.UserName).FirstOrDefault();
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(forgetPasswordDto.NewPassword);
            _userRepository.Update(user);
            return user.Id;
        }
        public string ChnagePasswordRequest(ForgetPasswordRequestDto forgetPasswordRequestDto)
        {
            var user = _userRepository.GetAll().Where(u => u.UserName == forgetPasswordRequestDto.UserName).FirstOrDefault();
            var role = _roleRepository.GetAll().Where(r => r.Id == user.RoleId).FirstOrDefault();

            if (role.RoleName == Types.Roles.CUSTOMER)
            {
                var customer = _customerRepository.GetAll().Where(c => c.UserId == user.Id).FirstOrDefault();
                return customer.Email;
            }
            else if (role.RoleName == Types.Roles.AGENT)
            {
                var agent = _agentRepository.GetAll().Where(a => a.UserId == user.Id).FirstOrDefault();
                return agent.Email;
            }
            else if(role.RoleName == Types.Roles.EMPLOYEE)
            {
                var employee = _employeeRepository.GetAll().Where(e => e.UserId == user.Id).FirstOrDefault();
                return employee.Email;
            }

            throw new Exception("No user found");

        }
    }
}
