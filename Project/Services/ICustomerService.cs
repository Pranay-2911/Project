﻿using Project.DTOs;
using Project.Models;

namespace Project.Services
{
    public interface ICustomerService
    {
        public List<CustomerDto> GetCustomers();
        public Customer GetById(Guid id);
        public Guid AddCustomer(CustomerDto customerDto);
        public bool DeleteCustomer(Guid id);
        public bool UpdateCustomer(CustomerDto customerDto);
    }
}
