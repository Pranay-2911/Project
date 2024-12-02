﻿using Project.Models;
using Project.Repositories;
using AutoMapper;
using Project.DTOs;
using Project.Types;
using Microsoft.EntityFrameworkCore;

namespace Project.Services
{
    public class PolicyService : IPolicyService
    {
        private readonly IRepository<Policy> _policyRepository;
        private readonly IRepository<Customer> _customerRepository;
        private readonly IRepository<Premium> _premiumRepository;
        private readonly IRepository<PolicyAccount> _policyAccountRepository;
        private readonly IRepository<Commission> _commisionRepository;
        private readonly IRepository<Plan> _planRepository;
        private readonly IMapper _mapper;

        public PolicyService(IRepository<Policy> repository, IMapper mapper, IRepository<Customer> customerRepository, IRepository<Premium> premiumRepository, IRepository<PolicyAccount> policyaccountRepository, IRepository<Commission> commisionRepository, IRepository<Plan> planRepository)
        {
            _policyRepository = repository;
            _customerRepository = customerRepository;
            _premiumRepository = premiumRepository;
            _policyAccountRepository = policyaccountRepository;
            _commisionRepository = commisionRepository;
            _planRepository = planRepository;
            _mapper = mapper;
        }
        public Guid AddSchema(PolicyDto policydto)
        {
            var policy = _mapper.Map<Policy>(policydto);
            _policyRepository.Add(policy);
            return policy.Id;
        }
        public Guid AddPlan(PlanDto plandto) 
        { 
            var plan = _mapper.Map<Plan>(plandto);
            _planRepository.Add(plan);
            return plan.Id;
        }

        public bool Delete(Guid id)
        {
            var policy = _policyRepository.Get(id);
            if (policy != null)
            {
                _policyRepository.Delete(policy);
                return true;
            }
            return false;
        }

        public PolicyDto Get(Guid id)
        {
            var policy = _policyRepository.Get(id);
            if(policy != null)
            {
                var policydto = _mapper.Map<PolicyDto>(policy);
                return policydto;
            }
            throw new Exception("No such policy exist");
        }

        public List<PolicyDto> GetAllSchema()
        {
            var policies = _policyRepository.GetAll().ToList();
            var policydtos = _mapper.Map<List<PolicyDto>>(policies);
            return policydtos;
        }
        public List<Plan> GetAllPlan()
        {
            var plan = _planRepository.GetAll().Include(p => p.Schemes).ToList();
            //var policydtos = _mapper.Map<List<PlanDto>>(plan);
            return plan;
        }

        public bool Update(PolicyDto policydto)
        {
            var existingPolicy = _policyRepository.Get(policydto.Id);
            if (existingPolicy != null)
            {
                var policy = _mapper.Map<Policy>(policydto);
                _policyRepository.Update(policy);
                return true;
            }
            return false;
        }

        public bool PurchasePolicy(Guid customerId, PurchasePolicyRequestDto requestdto)
        {
            // 1. Link customer to the policy (add an entry in the policy-customer table if required)
            var policy = _policyRepository.Get(requestdto.PolicyId);
            var customer = _customerRepository.Get(customerId);
            var account = new PolicyAccount() { 
                CustomerId = customer.CustomerId, 
                PolicyID = policy.Id, 
                Nominee = requestdto.Nominee, 
                NomineeRelation = requestdto.NomineeRelation, 
                AgentId = requestdto.AgentId, 
                PurchasedDate = DateTime.UtcNow
            };
            _policyAccountRepository.Add(account);



            // 2. Generate the premium schedule
            var premiums = GeneratePremiumSchedule(customerId, requestdto);

            // 3. Insert premiums into the database
            foreach (var premium in premiums)
            {
                _premiumRepository.Add(premium);
            }

            if (requestdto.AgentId != null)
            {
                var registrationCommission = policy.RegistrationCommisionAmount;
                var agentCommission = new Commission
                {
                    AgentId = (Guid)requestdto.AgentId,
                    PolicyId = policy.Id,
                    CommissionAmount = registrationCommission,
                    EarnedDate = DateTime.UtcNow,
                    CommissionType = CommissionType.REGISTRATION
                };

                _commisionRepository.Add(agentCommission);
            }
        
            return true;
        }

        private List<Premium> GeneratePremiumSchedule(Guid customerId, PurchasePolicyRequestDto requestdto)
        {
            var premiums = new List<Premium>();
            var monthlyAmount = requestdto.TotalAmount / requestdto.DurationInMonths; // Split total amount equally

            for (int i = 0; i < requestdto.DurationInMonths; i++)
            {
                premiums.Add(new Premium
                {
                    CustomerId = customerId,
                    PolicyId = requestdto.PolicyId,
                    Amount = monthlyAmount,
                    DueDate = DateTime.Now.AddMonths(i + 1), // Due date is next month onwards
                    Status = "Unpaid",
                    AgentId = requestdto.AgentId
                });
            }

            return premiums;
        }
        
        public List<PolicyDto> GetPolicyByCustomer(Guid Id)
        {
            var customer = _customerRepository.Get(Id);
            var accounts = _policyAccountRepository.GetAll().Where(c => c.CustomerId ==  Id).ToList();

            List<Policy> policies = new List<Policy>();
            foreach (var account in accounts)
            {
                var policy = _policyRepository.Get(account.PolicyID);
                policies.Add(policy);
            }
            var policyDto = _mapper.Map<List<PolicyDto>>(policies);
            return policyDto;

        }

    }
}
