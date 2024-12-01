using Project.Models;
using Project.DTOs;
namespace Project.Services
{
    public interface IPolicyService
    {
        public Guid Add(PolicyDto policy);
        public PolicyDto Get(Guid id);
        public List<PolicyDto> GetAll();
        public bool Update(PolicyDto policy);
        public bool Delete(Guid id);
        public bool PurchasePolicy(Guid customerId, Guid policyId, double totalAmount, int durationInMonths);
    }
}
