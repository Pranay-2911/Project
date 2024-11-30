using Project.DTOs;

namespace Project.Services
{
    public interface IPremiumService
    {
        public List<PremiumDto> GetPremiumsByPolicy(Guid policyId);
        public PaymentDto PayPremium(Guid premiumId, PaymentDto paymentDto);
        public List<PremiumDto> GetPremiumStatuses(Guid policyId);
    }
}
