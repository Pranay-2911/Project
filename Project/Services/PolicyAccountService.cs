﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Project.DTOs;
using Project.Models;
using Project.Repositories;
using Serilog;
using Stripe;

namespace Project.Services
{
    public class PolicyAccountService : IPolicyAccountService
    {
        private readonly IRepository<PolicyAccount> _repository;
        private readonly IRepository<Models.Customer> _customerRepository;
        private readonly IRepository<Policy> _policyRepository;
        private readonly IRepository<Agent> _agentRepository;
        private readonly IRepository<Premium> _premiumRepository;
        private readonly IMapper _mapper;

        public PolicyAccountService(IRepository<PolicyAccount> repository, IMapper mapper, IRepository<Premium> premiumRepository, IRepository<Models.Customer> customerRepository, IRepository<Policy> policyRepository, IRepository<Agent> agentRepository)
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
            Log.Information("New Policy added: " + policyAccount.Id);
            return policyAccount.Id;
        }

        public bool Delete(Guid customerId, Guid policyId)
        {
            var account = _repository.GetAll().Where(a => a.CustomerId == customerId).Where(a => a.PolicyID == policyId).FirstOrDefault();
            Log.Information("PolicyAccount Deleted: " + account.Id);
            _repository.Delete(account);
            var premiumList = _premiumRepository.GetAll().Where(p => p.CustomerId == customerId).Where(p => p.PolicyId == policyId).ToList();
            foreach (var premium in premiumList)
            {
                _premiumRepository.Delete(premium);
            }
            return true;

        }

        public PageList<PolicyAccountDto> GetAll(PageParameter pageParameter, ref int count, string? searchQuery, string? searchQuery1)
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
            if (!string.IsNullOrEmpty(searchQuery))
            {
                searchQuery = searchQuery.ToLower();
                policyAccountDtos = policyAccountDtos
                    .Where(d => d.CustomerName.ToLower().Contains(searchQuery))
                    .ToList();
            }
            if (!string.IsNullOrEmpty(searchQuery1))
            {
                searchQuery1 = searchQuery1.ToLower();
                policyAccountDtos = policyAccountDtos
                    .Where(d => d.PolicyName.ToLower().Contains(searchQuery1))
                    .ToList();
            }

            count = policyAccountDtos.Count;
            policyAccountDtos = policyAccountDtos.OrderByDescending(p => p.PurchasedDate).ToList();
            return PageList<PolicyAccountDto>.ToPagedList(policyAccountDtos, pageParameter.PageNumber, pageParameter.PageSize);
        }

        public PageList<PolicyAccountDto> GetAccountByCustomer(Guid id, PageParameter pageParameters, ref int count, string? searchQuery)
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
                    IsVerified = account.IsVerified,
                    PolicyId= policy.Id,
                    IsMatured = account.IsMatured
                    
                    
                    

                };
                if (account.AgentId != null)
                {
                    var agent = _agentRepository.Get((Guid)account.AgentId);
                    accountDto.AgentName = $"{agent.FirstName} {agent.LastName}";
                }
                policyAccountDtos.Add(accountDto);


            }
            if (!string.IsNullOrEmpty(searchQuery))
            {
                searchQuery = searchQuery.ToLower();
                policyAccountDtos = policyAccountDtos
                    .Where(d => d.PolicyName.ToLower().Contains(searchQuery))
                    .ToList();
            }
            count = policyAccountDtos.Count;
            return PageList<PolicyAccountDto>.ToPagedList(policyAccountDtos, pageParameters.PageNumber, pageParameters.PageSize);
        }

        public void Update(PolicyAccountDto policyAccountDto)
        {
            throw new NotImplementedException();
        }

        public bool ReUpload(Guid id)
        {
            var account = _repository.Get(id);
            if (account != null)
            {
                account.IsVerified = Types.WithdrawStatus.PENDING;
                _repository.Update(account);

                return true;
            }
            return false;
        }
        public  PageList<PolicyAccountDto> GetAllClaims(PageParameter pageParameter, ref int count, string? searchQuery)
        {
            var accountList = _repository.GetAll().Where(a => a.IsMatured == Types.MatureStatus.UNDER_PROCESS).ToList();
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
                    IsVerified = account.IsVerified,
                    PolicyId = policy.Id,
                    IsMatured = account.IsMatured




                };
                if (account.AgentId != null)
                {
                    var agent = _agentRepository.Get((Guid)account.AgentId);
                    accountDto.AgentName = $"{agent.FirstName} {agent.LastName}";
                }

                policyAccountDtos.Add(accountDto);


            }
            if (!string.IsNullOrEmpty(searchQuery))
            {
                searchQuery = searchQuery.ToLower();
                policyAccountDtos = policyAccountDtos
                    .Where(d => d.CustomerName.ToLower().Contains(searchQuery))
                    .ToList();
            }

            count = policyAccountDtos.Count;
            return PageList<PolicyAccountDto>.ToPagedList(policyAccountDtos, pageParameter.PageNumber, pageParameter.PageSize);
        }

        public bool ApproveClaims(Guid id)
        {
            var account = _repository.Get(id);
            if (account != null)
            {
                account.IsMatured = Types.MatureStatus.CLAIMED;
                _repository.Update(account);
                return true;
            }
            return false;
        }
    }
}
