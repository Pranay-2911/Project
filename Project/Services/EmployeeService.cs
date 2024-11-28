using AutoMapper;
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
        private Guid _roleId = new Guid("6046947d-04e2-4ab5-9320-08dd0ecdec34");
        public EmployeeService(IRepository<Employee> employeeRepository, IMapper mapper, IRepository<Role> repositoryRole, IRepository<User> userRepository)
        {
            _repository = employeeRepository;
            _mapper = mapper;
            _repositoryRole = repositoryRole;
            _userRepository = userRepository;
        }
        public Guid AddEmployee(EmployeeRegisterDto employeeRegisterDto)
        {
            User user = _mapper.Map<User>(employeeRegisterDto);
            user.RoleId = _roleId;
            user.Status = true;
            _userRepository.Add(user);

            var role = _repositoryRole.Get(_roleId);
            role.Users.Add(user);
            _repositoryRole.Update(role);

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
