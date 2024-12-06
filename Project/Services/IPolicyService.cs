using Project.Models;
using Project.DTOs;
namespace Project.Services
{
    public interface IPolicyService
    {
        public Guid AddSchema(PolicyDto policydto);
        public PolicyDto Get(Guid id);
        public List<PolicyDto> GetAllSchema();
        public List<Plan> GetAllPlan();
        public bool Update(PolicyDto policy);
        public bool Delete(Guid id);
        public bool PurchasePolicy(Guid customerId, PurchasePolicyRequestDto requestdto);
        public List<PolicyDto> GetPolicyByCustomer(Guid Id);
        public Guid AddPlan(PlanDto plandto);
        public List<ViewCommissionDto> GetCommission();
        public List<ViewCommissionDto> GetCommissionByAgent(Guid id);
    }
}
