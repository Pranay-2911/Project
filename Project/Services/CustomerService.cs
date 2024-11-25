using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Project.DTOs;
using Project.Models;
using Project.Repositories;
using Project.Types;

namespace Project.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly IRepository<Customer> _repository;
        private readonly IRepository<Role> _roleRepository;
        private readonly IRepository<User> _userRepository;
        private readonly IMapper _mapper;
        public CustomerService(IRepository<Customer> cutomerRepository, IMapper mapper, IRepository<Role> roleRepository, IRepository<User> userRepository)
        {
            _mapper = mapper;
            _repository = cutomerRepository;
            _roleRepository = roleRepository;
            _userRepository = userRepository;   
        }

        public Guid AddCustomer(CustomerRegisterDto customerRegisterDto)
        {

            Role role = new Role() { RoleName = Roles.CUSTOMER, Status = true};
            _roleRepository.Add(role);
         
            User user = new User() { UserName = customerRegisterDto.UserName, Password = customerRegisterDto.Password, RoleId = role.Id};
            _userRepository.Add(user);
            
            customerRegisterDto.UserId = user.Id;

            var customer = _mapper.Map<Customer>(customerRegisterDto);
            _repository.Add(customer);
            return customer.CustomerId;
        }
        public bool ChangePassword(ChnagePasswordDto passwordDto)
        {
            var agent = _repository.GetAll().AsNoTracking().Include(a => a.User).Where(a => a.User.UserName == passwordDto.UserName && a.User.Password == passwordDto.Password).FirstOrDefault();
            if (agent != null)
            {
                agent.User.Password = passwordDto.NewPassword;
                _repository.Update(agent);
                return true;
            }
            return false;
        }

        public bool DeleteCustomer(Guid id)
        {
            var customer = _repository.Get(id);
            if (customer != null)
            {
                _repository.Delete(customer);
                return true;
            }
            return false;
        }

        public Customer GetById(Guid id)
        {
            return _repository.Get(id);
        }

        public List<CustomerDto> GetCustomers()
        {
            var customer = _repository.GetAll().ToList();
            List<CustomerDto> customerDtos = _mapper.Map<List<CustomerDto>>(customer);
            return customerDtos;
        }

        public bool UpdateCustomer(CustomerDto customerDto)
        {
            var existingCustomer = _repository.GetAll().AsNoTracking().Where(u => u.CustomerId == customerDto.CustomerId);
            if (existingCustomer != null)
            {
                var customer = _mapper.Map<Customer>(customerDto);
                _repository.Update(customer);
                return true;
            }
            return false;
        }
    }
}
