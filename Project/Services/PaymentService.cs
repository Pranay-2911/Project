using Project.DTOs;
using Project.Models;
using Project.Repositories;

namespace Project.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IRepository<Payment> _paymentRepository;
        private readonly IRepository<Premium> _premiumRepository;
        private readonly IRepository<Customer> _customerRepository;
        private readonly IRepository<Policy> _policyRepository;
        public PaymentService(IRepository<Payment> paymentRepository, IRepository<Customer> customerRepository, IRepository<Policy> policyRepository, IRepository<Premium> premiumRepository)
        {
            _paymentRepository = paymentRepository;
            _customerRepository = customerRepository;
            _policyRepository = policyRepository;
            _premiumRepository = premiumRepository;
        }

        public PageList<ShowPaymentDto> GetAll(PageParameter pageParameter, ref int count, string? searchQuery, DateTime? startDate, DateTime? endDate)
        {   
            var payments = _paymentRepository.GetAll().ToList();
            List<ShowPaymentDto> paymentDtos = new List<ShowPaymentDto>();
            foreach (var payment in payments)
            {
                var premium = _premiumRepository.Get(payment.PremiumId);
                var customer = _customerRepository.Get(premium.CustomerId);
                var policy = _policyRepository.Get(premium.PolicyId);
                var dto = new ShowPaymentDto()
                {
                    CustomerName = $"{customer.FirstName} {customer.LastName}",
                    PaymentDate = payment.PaymentDate,
                    Amount = payment.AmountPaid,
                    PolicyName = policy.Title,
                    Status = payment.PaymentStatus
                };

                paymentDtos.Add(dto);
                if (!string.IsNullOrEmpty(searchQuery))
                {
                    searchQuery = searchQuery.ToLower();
                    paymentDtos = paymentDtos
                        .Where(d => d.PolicyName.ToLower().Contains(searchQuery))
                        .ToList();
                }
                if (startDate.HasValue)
                {
                    paymentDtos = paymentDtos.Where(c => c.PaymentDate >= startDate.Value).ToList();
                }

                if (endDate.HasValue)
                {
                    paymentDtos = paymentDtos.Where(c => c.PaymentDate <= endDate.Value).ToList();
                }
                count = paymentDtos.Count;
            }
            paymentDtos = paymentDtos.OrderByDescending(d => d.PaymentDate).ToList();
            return PageList<ShowPaymentDto>.ToPagedList(paymentDtos, pageParameter.PageNumber, pageParameter.PageSize);
        }
    }
}
