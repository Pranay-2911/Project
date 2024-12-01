﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Project.DTOs;
using Project.Exceptions;
using Project.Models;
using Project.Repositories;
using Project.Types;

namespace Project.Services
{
    public class CustomerService : ICustomerService
    {
        private Guid _roleId = new Guid("ef3c2cd4-d117-41c0-931f-08dd0ecdec34");
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

            User user = _mapper.Map<User>(customerRegisterDto);
            user.RoleId = _roleId;
            user.Status = true;
            _userRepository.Add(user);

            var role = _roleRepository.Get(_roleId);
            role.Users.Add(user);
            _roleRepository.Update(role);

            customerRegisterDto.UserId = user.Id;

            var customer = _mapper.Map<Customer>(customerRegisterDto);
            _repository.Add(customer);
            return customer.CustomerId;
        }

        public Guid AddPolicyAccount(PolicyAccountDto policyAccountDto)
        {
            var policyAccont = _mapper.Map<PolicyAccount>(policyAccountDto);
            _policyAccountRepository.Add(policyAccont);
            var customer = _repository.Get(policyAccont.CustomerId);
            customer.PolicyAccount = policyAccont;
            _repository.Update(customer);
            return policyAccont.Id;
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
                return true;
            }
            throw new CustomerNotFoundException("Customer Does Not Exist");
        }

        public Customer GetById(Guid id)
        {
            var customer =  _repository.Get(id);
            if(customer != null)
            {
                return customer;
            }
            throw new CustomerNotFoundException("Customer Does Not Exist");
        }

        public List<CustomerDto> GetCustomers()
        {
            var customer = _repository.GetAll().AsNoTracking().Include(p => p.PolicyAccount).ToList();
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
            throw new CustomerNotFoundException("Customer Does Not Exist");
        }
    }
}
