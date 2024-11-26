﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Project.DTOs;
using Project.Models;
using Project.Repositories;

namespace Project.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IRepository<Employee> _repository;
        private readonly IRepository<Role> _repositoryRole;
        private readonly IRepository<User> _userRepository;
        private readonly IMapper _mapper;
        public EmployeeService(IRepository<Employee> employeeRepository, IMapper mapper, IRepository<Role> repositoryRole, IRepository<User> userRepository)
        {
            _repository = employeeRepository;
            _mapper = mapper;
            _repositoryRole = repositoryRole;
            _userRepository = userRepository;
        }
        public Guid AddEmployee(EmployeeRegisterDto employeeRegisterDto)
        {
            Role role = new Role() { RoleName = Types.Roles.EMPLOYEE};
            _repositoryRole.Add(role);

            User user = new User() { UserName = employeeRegisterDto.Username, Password = employeeRegisterDto.Password, RoleId = role.Id, Status = true };
            _userRepository.Add(user);

            employeeRegisterDto.UserId = user.Id;

            var employee = _mapper.Map<Employee>(employeeRegisterDto);
            _repository.Add(employee);
            return employee.Id;
        }

        public bool DeleteEmployee(Guid id)
        {
            var employee = _repository.Get(id);
            if (employee != null)
            {
                _repository.Delete(employee);
                return true;
            }
            return false;
        }

        public Employee GetById(Guid id)
        {
            return _repository.Get(id);
        }

        public List<EmployeeDto> GetEmployees()
        {
            var employee = _repository.GetAll().ToList();
            List<EmployeeDto> employeeDtos = _mapper.Map<List<EmployeeDto>>(employee);
            return employeeDtos;
        }

        public bool UpdateEmployee(EmployeeDto employeeDto)
        {
            var existingEmployee = _repository.GetAll().AsNoTracking().Where(u => u.Id == employeeDto.Id);
            if (existingEmployee != null)
            {
                var employee = _mapper.Map<Employee>(employeeDto);
                _repository.Update(employee);
                return true;
            }
            return false;
        }
    }
}
