using Project.DTOs;

namespace Project.Services
{
    public interface IPremiumService
    { 
        public PaymentDto PayPremium(Guid premiumId, PaymentDto paymentDto);
        public List<PremiumDto> GetPremiumStatuses(Guid policyId);
        public List<PremiumDto> GetPremiumByPolicyAccount(Guid id);
    }
}
