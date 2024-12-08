using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Project.DTOs;
using Project.Models;
using Project.Repositories;

namespace Project.Services
{
    public class PolicyAccountService : IPolicyAccountService
    {
        private readonly IRepository<PolicyAccount> _repository;
        private readonly IRepository<Customer> _customerRepository;
        private readonly IRepository<Policy> _policyRepository;
        private readonly IRepository<Agent> _agentRepository;
        private readonly IRepository<Premium> _premiumRepository;
        private readonly IMapper _mapper;

        public PolicyAccountService(IRepository<PolicyAccount> repository, IMapper mapper, IRepository<Premium> premiumRepository, IRepository<Customer> customerRepository, IRepository<Policy> policyRepository, IRepository<Agent> agentRepository)
        {
            _repository = repository;
            _mapper = mapper;  
            _premiumRepository = premiumRepository;
            _customerRepository = customerRepository;
            _policyRepository = policyRepository;
            _agentRepository = agentRepository;
        }
        public Guid Add(PolicyAccountDto policyAccountDto)
        {
            var policyAccount = _mapper.Map<PolicyAccount>(policyAccountDto);
            _repository.Add(policyAccount);
            return policyAccount.Id;
        }

        public bool Delete(Guid customerId, Guid policyId)
        {
            var account = _repository.GetAll().Where(a => a.CustomerId == customerId).Where(a => a.PolicyID == policyId).FirstOrDefault();
            _repository.Delete(account);
            var premiumList = _premiumRepository.GetAll().Where(p => p.CustomerId == customerId).Where(p => p.PolicyId == policyId).ToList();
            foreach (var premium in premiumList)
            {
                _premiumRepository.Delete(premium);
            }
            return true;

        }

        public PageList<PolicyAccountDto> GetAll(PageParameter pageParameter)
        {
            var accountList = _repository.GetAll().ToList();
            List<PolicyAccountDto> policyAccountDtos = new List<PolicyAccountDto>();
            foreach (var account in accountList)
            {

                var custommer = _customerRepository.Get(account.CustomerId);
                var policy = _policyRepository.Get(account.PolicyID);
                
                var accountDto = new PolicyAccountDto()
                {
                    Id = account.Id,
                    CustomerName = $"{custommer.FirstName} {custommer.LastName}",
                    PolicyName = policy.Title,
                    NomineeRelation = account.NomineeRelation,
                    Nominee = account.Nominee,
                    PolicyAmount = account.PolicyAmount,
                    PolicyDuration = account.PolicyDuration,
                    PurchasedDate = account.PurchasedDate,
                    AgentName = "NA"

                };
                if(account.AgentId != null)
                {
                    var agent = _agentRepository.Get((Guid)account.AgentId);
                    accountDto.AgentName = $"{agent.FirstName} {agent.LastName}";
                }
                policyAccountDtos.Add(accountDto);
                
               
            }
            return PageList<PolicyAccountDto>.ToPagedList(policyAccountDtos, pageParameter.PageNumber, pageParameter.PageSize);
        }

        public PageList<PolicyAccountDto> GetAccountByCustomer(Guid id, PageParameter pageParameters)
        {
            var accountList = _repository.GetAll().Where(e => e.CustomerId == id).ToList();
            List<PolicyAccountDto> policyAccountDtos = new List<PolicyAccountDto>();
            foreach (var account in accountList)
            {

                var custommer = _customerRepository.Get(account.CustomerId);
                var policy = _policyRepository.Get(account.PolicyID);

                var accountDto = new PolicyAccountDto()
                {
                    Id = account.Id,
                    CustomerName = $"{custommer.FirstName} {custommer.LastName}",
                    PolicyName = policy.Title,
                    NomineeRelation = account.NomineeRelation,
                    Nominee = account.Nominee,
                    PolicyAmount = account.PolicyAmount,
                    PolicyDuration = account.PolicyDuration,
                    PurchasedDate = account.PurchasedDate,
                    AgentName = "NA",
                    IsVerified = account.IsVerified
                    

                };
                if (account.AgentId != null)
                {
                    var agent = _agentRepository.Get((Guid)account.AgentId);
                    accountDto.AgentName = $"{agent.FirstName} {agent.LastName}";
                }
                policyAccountDtos.Add(accountDto);


            }
            return PageList<PolicyAccountDto>.ToPagedList(policyAccountDtos, pageParameters.PageNumber, pageParameters.PageSize);
        }

        public void Update(PolicyAccountDto policyAccountDto)
        {
            throw new NotImplementedException();
        }
    }
}
