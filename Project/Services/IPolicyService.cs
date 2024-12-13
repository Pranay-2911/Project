using Project.Models;
using Project.DTOs;
namespace Project.Services
{
    public interface IPolicyService
    {
        public Guid AddSchema(PolicyDto policydto);
        public PolicyDto Get(Guid id);
        public PageList<PolicyDto> GetAllSchema(PageParameter pageParameter, ref int count, string? searchQuery);
        public List<Plan> GetAllPlan();
        public bool Update(PolicyDto policy);
        public bool Delete(Guid id);
        public bool PurchasePolicy(Guid customerId, PurchasePolicyRequestDto requestdto, ref Guid policyAcctId);
        public List<PolicyDto> GetPolicyByCustomer(Guid Id);
        public Plan AddPlan(PlanDto plandto);
        public PageList<ViewCommissionDto> GetCommission(PageParameter pageParameter, ref int count, string? searchQuery, string? commissionType, DateTime? startDate, DateTime? endDate);
        public PageList<ViewCommissionDto> GetCommissionByAgent(Guid id, PageParameter pageParameter, ref int count, string? searchQuery);
        public List<Policy> GetPoliciesByPlan(SchemaRequestDto schemaRequestDto);
    }
}
