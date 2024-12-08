using Project.DTOs;
using Project.Models;

namespace Project.Services
{
    public interface IPolicyAccountService
    {
        public Guid Add(PolicyAccountDto policyAccountDto);
        public bool Delete(Guid customerId, Guid policyId);
        public void Update(PolicyAccountDto policyAccountDto);

        public PageList<PolicyAccountDto> GetAll(PageParameter pageParameter);
        public PageList<PolicyAccountDto> GetAccountByCustomer(Guid id, PageParameter pageParameters);
    }
}
