using Project.DTOs;
using Project.Models;

namespace Project.Services
{
    public interface IEmployeeService
    {
        public List<EmployeeDto> GetEmployees();
        public Employee GetById(Guid id);
        public Guid AddEmployee(EmployeeRegisterDto employeeRegisterDto);
        public bool DeleteEmployee(Guid id);
        public bool UpdateEmployee(EmployeeDto employeeDto);
        public List<VerifyDocumentDto> GetDocuments();
        public bool Verify(Guid id);
    }
}
