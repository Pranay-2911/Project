using Project.DTOs;
using Project.Models;

namespace Project.Services
{
    public interface IPremiumService
    { 
        public PaymentDto PayPremium(Guid premiumId);
        public List<PremiumDto> GetPremiumStatuses(Guid policyId);
        public PageList<PremiumDto> GetPremiumByPolicyAccount(Guid id, PageParameter pageParameter);
        public bool AddImage(string image, Guid id);
    }
}
