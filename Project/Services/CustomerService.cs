using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Project.DTOs;
using Project.Exceptions;
using Project.Models;
using Project.Repositories;
using Project.Types;
using Serilog;

namespace Project.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly IRepository<Customer> _repository;
        private readonly IRepository<Role> _roleRepository;
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<PolicyAccount> _policyAccountRepository;
        private readonly IMapper _mapper;
        public CustomerService(IRepository<Customer> cutomerRepository, IMapper mapper, IRepository<Role> roleRepository, IRepository<User> userRepository, IRepository<PolicyAccount> policyAccountRepository)
        {
            _mapper = mapper;
            _repository = cutomerRepository;
            _roleRepository = roleRepository;
            _userRepository = userRepository;
            _policyAccountRepository = policyAccountRepository;
        }

        public Guid AddCustomer(CustomerRegisterDto customerRegisterDto)
        {

            var roleName = _roleRepository.GetAll().Where(r => r.RoleName == Roles.CUSTOMER).FirstOrDefault();
            User user = _mapper.Map<User>(customerRegisterDto);
            user.RoleId = roleName.Id;
            user.Status = true;
            _userRepository.Add(user);

            var role = _roleRepository.Get(roleName.Id);
            role.Users.Add(user);
            _roleRepository.Update(role);
            var customer = _mapper.Map<Customer>(customerRegisterDto);

            customer.UserId = user.Id;
            customer.IsKYC = false;


            _repository.Add(customer);
            Log.Information("New customer record added: " + customer.CustomerId);
            return customer.CustomerId;
        }



        public bool ChangePassword(ChangePasswordDto passwordDto)
        {
            var customer = _repository.GetAll().AsNoTracking().Include(a => a.User).Where(a => a.User.UserName == passwordDto.UserName).FirstOrDefault();
            if (customer != null)
            {
                if (BCrypt.Net.BCrypt.Verify(passwordDto.Password, customer.User.PasswordHash))
                {
                    customer.User.PasswordHash = BCrypt.Net.BCrypt.HashPassword(passwordDto.NewPassword);
                    _repository.Update(customer);
                    Log.Information("customer password updated: " + customer.CustomerId);
                    return true;
                }

            }
            return false;
        }

        public bool DeleteCustomer(Guid id)
        {
            var customer = _repository.Get(id);
            if (customer != null)
            {
                _repository.Delete(customer);
                Log.Information("customer record deleted: " + customer.CustomerId);
                return true;
            }
            throw new CustomerNotFoundException("Customer Does Not Exist");
        }

        public Customer GetById(Guid id)
        {
            var customer = _repository.Get(id);
            if (customer != null)
            {
                return customer;
            }
            throw new CustomerNotFoundException("Customer Does Not Exist");
        }

        public PageList<CustomerDto> GetCustomers(PageParameter pageParameter, ref int count, string? searchQuery)
        {
            var customer = _repository.GetAll().AsNoTracking().Include(c => c.Accounts).ToList();
            count = customer.Count;
            List<CustomerDto> customerDtos = _mapper.Map<List<CustomerDto>>(customer);
            if (!string.IsNullOrEmpty(searchQuery))
            {
                searchQuery = searchQuery.ToLower();
                customerDtos = customerDtos
                    .Where(d => d.FirstName.ToLower().Contains(searchQuery))
                    .ToList();
            }
            count = customerDtos.Count;
            return PageList<CustomerDto>.ToPagedList(customerDtos, pageParameter.PageNumber, pageParameter.PageSize);
        }

        public bool UpdateCustomer(UpdateCustomerDto customerDto)
        {
            var existingCustomer = _repository.GetAll().AsNoTracking().Where(u => u.CustomerId == customerDto.CustomerId).FirstOrDefault();
            if (existingCustomer != null)
            {
                existingCustomer.FirstName = customerDto.FirstName;
                existingCustomer.LastName = customerDto.LastName;
                existingCustomer.DateOfBirth = customerDto.DateOfBirth;
                existingCustomer.Email = customerDto.Email;
                existingCustomer.City = customerDto.City;
                existingCustomer.State = customerDto.State;
                existingCustomer.MobileNumber = customerDto.MobileNumber;
                _repository.Update(existingCustomer);
                Log.Information("customer record updated: " + existingCustomer.CustomerId);
                return true;
            }
            throw new CustomerNotFoundException("Customer Does Not Exist");
        }

        public CustomerNameMobDto GetCustomerNameMobDto(Guid id)
        {
            var customer = _repository.Get(id);
            var dto = new CustomerNameMobDto()
            {
                Name = $"{customer.FirstName} {customer.LastName}",
                MobileNumber = customer.MobileNumber
            };
            return dto;
        }
    }
}
