﻿using Project.Models;
using Project.Repositories;
using AutoMapper;
using Project.DTOs;
using Project.Types;
using Microsoft.EntityFrameworkCore;
using MailKit.Search;
using Project.Exceptions;
using Serilog;

namespace Project.Services
{
    public class PolicyService : IPolicyService
    {
        private readonly IRepository<Policy> _policyRepository;
        private readonly IRepository<Customer> _customerRepository;
        private readonly IRepository<Agent> _agentRepository;
        private readonly IRepository<Premium> _premiumRepository;
        private readonly IRepository<PolicyAccount> _policyAccountRepository;
        private readonly IRepository<Commission> _commisionRepository;
        private readonly IRepository<Plan> _planRepository;
        private readonly IRepository<Document> _documentRepository;
        private readonly IMapper _mapper;

        public PolicyService(IRepository<Policy> repository, IMapper mapper, IRepository<Customer> customerRepository, IRepository<Premium> premiumRepository, IRepository<PolicyAccount> policyaccountRepository, IRepository<Commission> commisionRepository, IRepository<Plan> planRepository, IRepository<Agent> agentRepository, IRepository<Document> documentRepository)
        {
            _policyRepository = repository;
            _customerRepository = customerRepository;
            _premiumRepository = premiumRepository;
            _policyAccountRepository = policyaccountRepository;
            _commisionRepository = commisionRepository;
            _planRepository = planRepository;
            _agentRepository = agentRepository;
            _documentRepository = documentRepository;
            _mapper = mapper;
        }
        public Guid AddSchema(PolicyDto policydto)
        {
            var existingScheme = _policyRepository.GetAll().Where(p=>p.Title == policydto.Title).FirstOrDefault();
            if (existingScheme == null)
            {
                var policy = _mapper.Map<Policy>(policydto);
                _policyRepository.Add(policy);
                Log.Information("New scheme added: " + policy.Id);
                return policy.Id;
            }
            throw new Exception("Schema already exist!");
        }

        public Plan AddPlan(PlanDto plandto) 
        { 
            var existingPlan = _planRepository.GetAll().Where(p=>p.Name == plandto.Name).FirstOrDefault();
            if (existingPlan == null)
            {
                var plan = _mapper.Map<Plan>(plandto);
                _planRepository.Add(plan);
                Log.Information("New plan added: " + plan.Id);
                return plan;
            }
            throw new PlanExistException("Plan with same name already exist!");
        }

        public bool Delete(Guid id)
        {
            var policy = _policyRepository.Get(id);
            
            if (policy != null)
            {
                policy.PolicyStatus =false;
                _policyRepository.Update(policy);
                Log.Information("policy deleted: " + policy.Id);
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

        public PageList<PolicyDto> GetAllSchema(PageParameter pageParameter, ref int count,string? searchQuery)
        {
            var policies = _policyRepository.GetAll().ToList(); 
            var policydtos = _mapper.Map<List<PolicyDto>>(policies);
            if (!string.IsNullOrEmpty(searchQuery))
            {
                searchQuery = searchQuery.ToLower();
                policydtos = policydtos
                    .Where(d => d.Title.ToLower().Contains(searchQuery))
                    .ToList();
            }
           

            count = policydtos.Count;
            return PageList<PolicyDto>.ToPagedList(policydtos, pageParameter.PageNumber, pageParameter.PageSize);
        }
        public List<Plan> GetAllPlan()
        {
            var plans = _planRepository.GetAll().Include(p => p.Schemes).ToList();

            foreach (var plan in plans)
            { 
                plan.Schemes = plan.Schemes.Where(scheme => scheme.PolicyStatus == true).ToList();
            }

            return plans;
        }


        public bool Update(PolicyDto policydto)
        {
            var existingPolicy = _policyRepository.GetAll().AsNoTracking().Where( p => p.Id == policydto.Id).FirstOrDefault();
            if (existingPolicy != null)
            {
                var policy = _mapper.Map<Policy>(policydto);
                _policyRepository.Update(policy);
                Log.Information("policy record updated: " + policy.Id);
                return true;
            }
            return false;
        }

        public bool PurchasePolicy(Guid customerId, PurchasePolicyRequestDto requestdto, ref Guid policyAcctId)
        {
            // 1. Link customer to the policy (add an entry in the policy-customer table if required)
            var policy = _policyRepository.Get(requestdto.PolicyId);
            var customer = _customerRepository.Get(customerId);
            if (!Enum.TryParse<NomineeRelation>(requestdto.NomineeRelation, true, out var nomineeRelation))
            {
                throw new ArgumentException($"Invalid Nominee Relation: {requestdto.NomineeRelation}");
            }
            var account = new PolicyAccount() { 
                CustomerId = customer.CustomerId, 
                PolicyID = policy.Id, 
                Nominee = requestdto.Nominee, 
                NomineeRelation = nomineeRelation, 
                AgentId = requestdto.AgentId, 
                PurchasedDate = DateTime.UtcNow,
                PolicyAmount = requestdto.TotalAmount,
                PolicyDuration = requestdto.DurationInYears,
                IsMatured = MatureStatus.PENDING
            };
            _policyAccountRepository.Add(account);
            policyAcctId = account.Id;

            // 2. Generate the premium schedule
            var premiums = GeneratePremiumSchedule(customerId, requestdto, policyAcctId);

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
                Log.Information("agentCommission record updated: " + agentCommission.Id);

                var agent = _agentRepository.Get((Guid)requestdto.AgentId);
                agent.CurrentCommisionBalance += policy.RegistrationCommisionAmount;
                agent.TotalCommissionEarned += policy.RegistrationCommisionAmount;
                _agentRepository.Update(agent);
                Log.Information("agent record updated: " + agent.Id);

            }
            Log.Information("policyAccount record updated: " + account.Id);

            return true;
        }

        private List<Premium> GeneratePremiumSchedule(Guid customerId, PurchasePolicyRequestDto requestdto, Guid AccountId)
        {
            var premiums = new List<Premium>();
            // Split total amount equally

            var totalPremiumCount = requestdto.DurationInYears * requestdto.Divider;
            var monthlyAmount = requestdto.TotalAmount / totalPremiumCount;

            int temp = 0;
            int premiumGap = 12 / requestdto.Divider;
            for (int i = 0; i < totalPremiumCount; i++)
            {
                
                premiums.Add(new Premium
                {
                    CustomerId = customerId,
                    PolicyId = requestdto.PolicyId,
                    Amount = monthlyAmount,
                    DueDate = DateTime.Now.AddMonths(temp + premiumGap), // Due date is next month onwards
                    Status = "Unpaid",
                    AgentId = requestdto.AgentId,
                    AccountId = AccountId

                   
                });
                temp += premiumGap;
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

        public PageList<ViewCommissionDto> GetCommission(PageParameter pageParameter, ref int count, string? searchQuery, string? commissionType, DateTime? startDate, DateTime? endDate) 
        {
            var commissions = _commisionRepository.GetAll().ToList();
            List<ViewCommissionDto> viewCommissionDtos = new List<ViewCommissionDto>();
            foreach(var commission in commissions)
            {
                var agent = _agentRepository.GetAll().Where(a => a.Id == commission.AgentId).FirstOrDefault();
                var policy = _policyRepository.GetAll().Where(p => p.Id == commission.PolicyId).FirstOrDefault();
                var policyAccount = _policyAccountRepository.GetAll().Where(p => p.PolicyID == policy.Id).Where(p => p.AgentId == agent.Id).FirstOrDefault();
                var customer = _customerRepository.GetAll().Where(c => c.CustomerId == policyAccount.CustomerId).FirstOrDefault();

                var viewCommission = new ViewCommissionDto()
                {
                    AgentName = $"{agent.FirstName} {agent.LastName}",
                    SchemaName = policy.Title,
                    CustomerName = $"{customer.FirstName} {customer.LastName}",
                    CommissionAmount = commission.CommissionAmount,
                    CommssionDate = commission.EarnedDate,
                    CommissionType = commission.CommissionType

                };
                viewCommissionDtos.Add(viewCommission);
            }
            viewCommissionDtos = viewCommissionDtos.OrderByDescending(d => d.CommssionDate).ToList();
            if (!string.IsNullOrEmpty(searchQuery))
            {
                searchQuery = searchQuery.ToLower();
                viewCommissionDtos = viewCommissionDtos
                    .Where(d => d.AgentName.ToLower().Contains(searchQuery))
                    .ToList();
            }
            
            if (!string.IsNullOrEmpty(commissionType))
            {
                var type = int.Parse(commissionType);
                viewCommissionDtos = viewCommissionDtos.Where(c => c.CommissionType == (CommissionType)type).ToList();
            }

            if (startDate.HasValue)
            {
                viewCommissionDtos = viewCommissionDtos.Where(c=>c.CommssionDate >= startDate.Value).ToList();
            }

            if (endDate.HasValue)
            {
                viewCommissionDtos = viewCommissionDtos.Where(c=>c.CommssionDate <= endDate.Value.AddDays(1)).ToList();
            }
            count = viewCommissionDtos.Count;

            return PageList<ViewCommissionDto>.ToPagedList(viewCommissionDtos, pageParameter.PageNumber, pageParameter.PageSize);
        }

        public List<ViewCommissionDto> GetCommissionByCustomer(Guid id)
        {
            var commissions = _commisionRepository.GetAll().ToList();
            List<ViewCommissionDto> viewCommissionDtos = new List<ViewCommissionDto>();
            foreach (var commission in commissions)
            {
                var agent = _agentRepository.GetAll().Where(a => a.Id == commission.AgentId).FirstOrDefault();
                var policy = _policyRepository.GetAll().Where(p => p.Id == commission.PolicyId).FirstOrDefault();
                var policyAccount = _policyAccountRepository.GetAll().Where(p => p.PolicyID == policy.Id).Where(p => p.AgentId == agent.Id).FirstOrDefault();
                var customer = _customerRepository.GetAll().Where(c => c.CustomerId == policyAccount.CustomerId).FirstOrDefault();

                var viewCommission = new ViewCommissionDto()
                {
                    AgentName = $"{agent.FirstName} {agent.LastName}",
                    SchemaName = policy.Title,
                    CustomerName = $"{customer.FirstName} {customer.LastName}",
                    CommissionAmount = commission.CommissionAmount,
                    CommssionDate = commission.EarnedDate,
                    CommissionType = commission.CommissionType

                };
                viewCommissionDtos.Add(viewCommission);
            }
         

            return viewCommissionDtos;
        }

        public PageList<ViewCommissionDto> GetCommissionByAgent(Guid id, PageParameter pageParameter,ref int count, string? searchQuery)
        {
            var commissions = _commisionRepository.GetAll().Where(c=>c.AgentId == id).ToList();
            
            List<ViewCommissionDto> viewCommissionDtos = new List<ViewCommissionDto>();

            foreach (var commission in commissions)
            {
                
                var agent = _agentRepository.GetAll().Where(a => a.Id == commission.AgentId).FirstOrDefault();
                var policy = _policyRepository.GetAll().Where(p => p.Id == commission.PolicyId).FirstOrDefault();
                var policyAccount = _policyAccountRepository.GetAll().Where(p => p.PolicyID == policy.Id).Where(p => p.AgentId == agent.Id).FirstOrDefault();
                var customer = _customerRepository.GetAll().Where(c => c.CustomerId == policyAccount.CustomerId).FirstOrDefault();

                var viewCommission = new ViewCommissionDto()
                {
                    AgentName = $"{agent.FirstName} {agent.LastName}",
                    SchemaName = policy.Title,
                    CustomerName = $"{customer.FirstName} {customer.LastName}",
                    CommissionAmount = commission.CommissionAmount,
                    CommssionDate = commission.EarnedDate,
                    CommissionType = commission.CommissionType

                };
                viewCommissionDtos.Add(viewCommission);
            }
            viewCommissionDtos = viewCommissionDtos.OrderByDescending(c => c.CommssionDate).ToList();
            if (!string.IsNullOrEmpty(searchQuery))
            {
                searchQuery = searchQuery.ToLower();
                viewCommissionDtos = viewCommissionDtos
                    .Where(d => d.CustomerName.ToLower().Contains(searchQuery))
                    .ToList();
            }
            count = viewCommissionDtos.Count;
            return PageList<ViewCommissionDto>.ToPagedList(viewCommissionDtos, pageParameter.PageNumber, pageParameter.PageSize); ;
        }

        public List<Policy> GetPoliciesByPlan(SchemaRequestDto schemaRequestDto)
        {
            var plan = _planRepository.GetAll().Include(p => p.Schemes).Where(p => p.Name == schemaRequestDto.Name).FirstOrDefault();
            return plan.Schemes;
        }

    }
}
