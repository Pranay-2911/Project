using Project.DTOs;

namespace Project.Services
{
    public interface IPolicyAccountService
    {
        public Guid Add(PolicyAccountDto policyAccountDto);
        public void Delete(Guid id);
        public void Update(PolicyAccountDto policyAccountDto);
    }
}
