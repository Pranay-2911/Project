using Project.DTOs;
using Project.Models;
using Project.Repositories;

namespace Project.Services
{
    public class PremiumService:IPremiumService
    {
        private readonly IRepository<Payment> _paymentRepository;
        private readonly IRepository<Premium> _premiumRepository;

        public PremiumService(IRepository<Payment> paymentRepository, IRepository<Premium> premiumRepository)
        {
            _paymentRepository = paymentRepository;
            _premiumRepository = premiumRepository;
        }

        public PaymentDto PayPremium(Guid premiumId, PaymentDto paymentDto)
        {
            var premium = _premiumRepository.Get(premiumId);

            if (premium == null || premium.Status == "Paid")
                return new PaymentDto { Status = false, Amount = paymentDto.Amount };

            // Save payment details
            var payment = new Payment
            {
                Id = Guid.NewGuid(),
                PremiumId = premiumId,
                AmountPaid = paymentDto.Amount,
                PaymentDate = DateTime.UtcNow,
                PaymentStatus = "Success"
            };
            _paymentRepository.Add(payment);

            // Update premium status
            premium.Status = "Paid";
            premium.PaymentDate = DateTime.UtcNow;
            _premiumRepository.Update(premium);

            return new PaymentDto { Amount = paymentDto.Amount, Status = true };
        }

        public List<PremiumDto> GetPremiumStatuses(Guid policyId)
        {
            return _premiumRepository.GetAll()
                .Where(p => p.PolicyId == policyId)
                .Select(p => new PremiumDto
                {
                    Id = p.Id,
                    DueDate = p.DueDate,
                    Amount = p.Amount,
                    Status = p.Status,
                    PaymentDate = p.PaymentDate
                }).ToList();
        }
    }
}
