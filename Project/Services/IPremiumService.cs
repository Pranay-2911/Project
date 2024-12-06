using Project.DTOs;

namespace Project.Services
{
    public interface IPremiumService
    { 
        public PaymentDto PayPremium(Guid premiumId);
        public List<PremiumDto> GetPremiumStatuses(Guid policyId);
        public List<PremiumDto> GetPremiumByPolicyAccount(Guid id);
        public bool AddImage(string image, Guid id);
    }
}
