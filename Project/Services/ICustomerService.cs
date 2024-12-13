using Project.DTOs;
using Project.Models;

namespace Project.Services
{
    public interface ICustomerService
    {
        public PageList<CustomerDto> GetCustomers(PageParameter pageParameter, ref int count, string? searchQuery);
        public Customer GetById(Guid id);
        public Guid AddCustomer(CustomerRegisterDto customerRegisterDto);
        public bool DeleteCustomer(Guid id);
        public bool UpdateCustomer(UpdateCustomerDto customerDto);
        public bool ChangePassword(ChangePasswordDto passwordDto);
        public CustomerNameMobDto GetCustomerNameMobDto(Guid id);
    }
}
