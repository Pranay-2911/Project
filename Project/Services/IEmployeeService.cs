using Project.DTOs;
using Project.Models;

namespace Project.Services
{
    public interface IEmployeeService
    {
        public PageList<EmployeeDto> GetEmployees(PageParameter pageParameter, ref int count, string? searchQuery);
        public Employee GetById(Guid id);
        public Guid AddEmployee(EmployeeRegisterDto employeeRegisterDto);
        public bool DeleteEmployee(Guid id);
        public bool UpdateEmployee(UpdateEmployeeDto employeeDto);
        public PageList<VerifyDocumentDto> GetDocuments(PageParameter pageParameters, ref int count, string? searchQuery);
        public bool ChangePassword(ChangePasswordDto passwordDto);   
        public bool Verify(Guid id);
        public bool Reject(Guid id);
    }
}
