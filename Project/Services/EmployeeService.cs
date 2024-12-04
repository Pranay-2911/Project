using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Project.DTOs;
using Project.Exceptions;
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
            var roleName = _repositoryRole.GetAll().Where(r => r.RoleName == Types.Roles.EMPLOYEE).FirstOrDefault();
            User user = _mapper.Map<User>(employeeRegisterDto);
            user.RoleId = roleName.Id;
            user.Status = true;
            _userRepository.Add(user);

            var role = _repositoryRole.Get(roleName.Id);
            role.Users.Add(user);
            _repositoryRole.Update(role);

            var employee = _mapper.Map<Employee>(employeeRegisterDto);
            employee.UserId = user.Id;

            _repository.Add(employee);
            return employee.Id;
        }

        public bool DeleteEmployee(Guid id)
        {
            var employee = _repository.GetAll().Include(e => e.User).Where(e => e.Id == id).FirstOrDefault();
            if (employee != null)
            {
                var user = _userRepository.Get(employee.UserId);
                user.Status = false;
                _userRepository.Update(user);
                return true;
            }
            throw new EmployeeNotFoundException("Employee Does Not Exist");
        }

        public Employee GetById(Guid id)
        {
            var emmloyee = _repository.Get(id);
            if(emmloyee != null)
            {
                return emmloyee;
            }
            throw new EmployeeNotFoundException("Employee Does Not Exist");
        }

        public List<EmployeeDto> GetEmployees()
        {
            var employee = _repository.GetAll().Include(a => a.User).Where(a => a.User.Status == true).ToList();
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
            throw new EmployeeNotFoundException("Employee Does Not Exist");
        }
    }
}
