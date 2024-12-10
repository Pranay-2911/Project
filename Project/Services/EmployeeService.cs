using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Project.DTOs;
using Project.Exceptions;
using Project.Models;
using Project.Repositories;
using System.Collections.Generic;

namespace Project.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IRepository<Employee> _repository;
        private readonly IRepository<Role> _repositoryRole;
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<PolicyAccount> _policyAccountRepository;
        private readonly IRepository<Customer> _customerRepository;
        private readonly IRepository<Policy> _policyRepository;
        private readonly IRepository<Document> _documentRepository;
        private readonly IMapper _mapper;

        public EmployeeService(IRepository<Employee> employeeRepository, IMapper mapper, IRepository<Role> repositoryRole, IRepository<User> userRepository, IRepository<PolicyAccount> policyAccountRepository, IRepository<Document> documentRepository, IRepository<Customer> customerRepository, IRepository<Policy> policyRepository)
        {
            _repository = employeeRepository;
            _mapper = mapper;
            _repositoryRole = repositoryRole;
            _policyAccountRepository = policyAccountRepository;
            _userRepository = userRepository;
            _documentRepository = documentRepository;
            _customerRepository = customerRepository;
            _policyRepository = policyRepository;
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

        public PageList<EmployeeDto> GetEmployees(PageParameter pageParameter, ref int count, string? searchQuery)
        {
            var employee = _repository.GetAll().Include(a => a.User).Where(a => a.User.Status == true).ToList();
            List<EmployeeDto> employeeDtos = _mapper.Map<List<EmployeeDto>>(employee);
            if (!string.IsNullOrEmpty(searchQuery))
            {
                searchQuery = searchQuery.ToLower();
                employeeDtos = employeeDtos
                    .Where(d => d.FirstName.ToLower().Contains(searchQuery))
                    .ToList();
            }
            count = employeeDtos.Count;
            return PageList<EmployeeDto>.ToPagedList(employeeDtos, pageParameter.PageNumber, pageParameter.PageSize);
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

        public PageList<VerifyDocumentDto> GetDocuments(PageParameter pageParameters, ref int count, string? searchQuery)
        {
            var accounts = _policyAccountRepository.GetAll().Where(a => a.IsVerified == Types.WithdrawStatus.PENDING).ToList();
            List<VerifyDocumentDto> verifyDocumentDtos = new List<VerifyDocumentDto>();
            foreach (var account in accounts)
            {
                List<Document> documents = _documentRepository.GetAll().Where( d => d.isActive == true).Where(d => d.PolicyAccountId == account.Id).ToList();
                var policy = _policyRepository.Get(account.PolicyID);
                var customer = _customerRepository.Get(account.CustomerId);
                var documentDto = new VerifyDocumentDto()
                {
                    PolicyAccountId = account.Id,
                    CustomerName = $"{customer.FirstName} {customer.LastName}",
                    PolicyName = policy.Title,
                    Documents = documents
                };
               

                verifyDocumentDtos.Add(documentDto);

            }
            if (!string.IsNullOrEmpty(searchQuery))
            {
                searchQuery = searchQuery.ToLower();
                verifyDocumentDtos = verifyDocumentDtos
                    .Where(d => d.CustomerName.ToLower().Contains(searchQuery))
                    .ToList();
            }
            count = verifyDocumentDtos.Count;

            return PageList<VerifyDocumentDto>.ToPagedList(verifyDocumentDtos, pageParameters.PageNumber, pageParameters.PageSize);

        }

        public bool Verify(Guid id)
        {
            var account = _policyAccountRepository.Get(id);
            if (account != null)
            {
                account.IsVerified = Types.WithdrawStatus.APPROVED;
                _policyAccountRepository.Update(account);
                return true;
            }

            return false;
        }
        public bool Reject(Guid id)
        {
            var account = _policyAccountRepository.Get(id);
            if (account != null)
            {
                account.IsVerified = Types.WithdrawStatus.REJECTED;
                _policyAccountRepository.Update(account);
                var documents = _documentRepository.GetAll().Where(d => d.PolicyAccountId == id).ToList();
                foreach (var document in documents)
                {
                    document.isActive = false;
                    _documentRepository.Update(document);
                }
                return true;
            }

            return false;
        }
        

    }
}
