using AutoMapper;
using Project.DTOs;
using Project.Models;
using Project.Repositories;

namespace Project.Services
{
    public class PolicyAccountService : IPolicyAccountService
    {
        private readonly IRepository<PolicyAccount> _repository;
        private readonly IRepository<Premium> _premiumRepository;
        private readonly IMapper _mapper;

        public PolicyAccountService(IRepository<PolicyAccount> repository, IMapper mapper, IRepository<Premium> premiumRepository)
        {
            _repository = repository;
            _mapper = mapper;  
            _premiumRepository = premiumRepository;
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

        public List<PolicyAccount> GetAll()
        {
            var accountList = _repository.GetAll().ToList();
            return accountList;
        }

        public void Update(PolicyAccountDto policyAccountDto)
        {
            throw new NotImplementedException();
        }
    }
}
