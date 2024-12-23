﻿using AutoMapper;
using Project.DTOs;
using Project.Models;
using Project.Repositories;
using Project.Types;
using Serilog;

namespace Project.Services
{
    public class PremiumService:IPremiumService
    {
        private readonly IRepository<Payment> _paymentRepository;
        private readonly IRepository<Premium> _premiumRepository;
        private readonly IRepository<Commission> _commissionRepository;
        private readonly IRepository<Policy> _policyRepository;
        private readonly IRepository<Agent> _agentRepository;
        private readonly IRepository<PolicyAccount> _policyAccountRepository;
        private readonly IMapper _mapper;

        public PremiumService(IRepository<Payment> paymentRepository, IRepository<Premium> premiumRepository, IRepository<Commission> commissionRepository, IRepository<Policy> policyRepository, IRepository<Agent> agentRepository, IRepository<PolicyAccount> policyAccountRepository, IMapper mapper)
        {
            _paymentRepository = paymentRepository;
            _premiumRepository = premiumRepository;
            _commissionRepository = commissionRepository;
            _policyRepository = policyRepository;
            _agentRepository = agentRepository;
            _policyAccountRepository = policyAccountRepository;
            _mapper = mapper;   
        }

        public PaymentDto PayPremium(Guid premiumId)
        {
            var premium = _premiumRepository.Get(premiumId);

            if (premium == null || premium.Status == "Paid")
                return new PaymentDto { Status = false, Amount = premium.Amount };

            // Save payment details
            var payment = new Payment
            {
                Id = Guid.NewGuid(),
                PremiumId = premiumId,
                AmountPaid = premium.Amount,
                PaymentDate = DateTime.UtcNow,
                PaymentStatus = "Success"
            };
            _paymentRepository.Add(payment);

            // Update premium status
            premium.Status = "Paid";
            premium.PaymentDate = DateTime.UtcNow;
            _premiumRepository.Update(premium);

            var policy = _policyRepository.Get(premium.PolicyId);
            if (premium.AgentId != null)
            {
                var agent = _agentRepository.Get((Guid)premium.AgentId);
                var premiumCommission = policy.InstallmentCommisionRatio * premium.Amount / 100.0;
                var agentCommission = new Commission
                {
                    AgentId = (Guid)premium.AgentId,
                    PolicyId = premium.PolicyId,
                    CommissionAmount = premiumCommission,
                    EarnedDate = DateTime.UtcNow,
                    CommissionType = CommissionType.PREMIUM
                };
                agent.CurrentCommisionBalance += agentCommission.CommissionAmount;
                agent.TotalCommissionEarned += agentCommission.CommissionAmount;

                _commissionRepository.Add(agentCommission);
                Log.Information("commission record updated: " + agentCommission.Id);
            }

            CheckMaturity(premium);

            Log.Information("payment record updated: " + payment.Id);
           


            return new PaymentDto { Amount = premium.Amount, Status = true };
        }
        private void CheckMaturity(Premium premium)
        {
            var premiums = _premiumRepository.GetAll().Where(p => p.AccountId == premium.AccountId).Where(p => p.Status == "Unpaid").ToList();

            if (premiums.Count == 0)
            {
                var account = _policyAccountRepository.Get(premium.AccountId);
                account.IsMatured = MatureStatus.UNDER_PROCESS;
                _policyAccountRepository.Update(account);
            }
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

        public PageList<PremiumDto> GetPremiumByPolicyAccount(Guid id, PageParameter pageParameter, ref int count)
        {
            var account =_policyAccountRepository.Get(id); 
            var premiums = _premiumRepository.GetAll().Where(a => a.CustomerId == account.CustomerId).Where(a => a.PolicyId == account.PolicyID).ToList();
            
            var premiumDto = _mapper.Map<List<PremiumDto>>(premiums);
            count = premiumDto.Count;
            return PageList<PremiumDto>.ToPagedList(premiumDto, pageParameter.PageNumber, pageParameter.PageSize); ;
        }

        public bool AddImage(string image, Guid id)
        {
            var policy = _policyRepository.Get(id);
            policy.ImageLink = image;
            _policyRepository.Update(policy);
            Log.Information("policy record updated: " + policy.Id);

            return true;
        }
    }
}
