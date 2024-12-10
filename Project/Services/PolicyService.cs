using Project.Models;
using Project.Repositories;
using AutoMapper;
using Project.DTOs;
using Project.Types;
using Microsoft.EntityFrameworkCore;
using MailKit.Search;

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
                policy.PolicyStatus =false;
                _policyRepository.Update(policy);
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
                PolicyDuration = requestdto.DurationInMonths
            };
            _policyAccountRepository.Add(account);
            policyAcctId = account.Id;

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
                var agent = _agentRepository.Get((Guid)requestdto.AgentId);
                agent.CurrentCommisionBalance += policy.RegistrationCommisionAmount;
                agent.TotalCommissionEarned += policy.RegistrationCommisionAmount;
                _agentRepository.Update(agent);
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

        public PageList<ViewCommissionDto> GetCommission(PageParameter pageParameter, ref int count, string? searchQuery, string? commissionType) 
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

    }
}
