using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Project.DTOs;
using Project.Models;
using Project.Repositories;

namespace Project.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly IRepository<Customer> _repository;
        private readonly IMapper _mapper;
        public CustomerService(IRepository<Customer> cutomerRepository, IMapper mapper)
        {
            _mapper = mapper;
            _repository = cutomerRepository;
        }

        public Guid AddCustomer(CustomerDto customerDto)
        {
            var customer = _mapper.Map<Customer>(customerDto);
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
