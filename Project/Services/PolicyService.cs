using Project.Models;
using Project.Repositories;

namespace Project.Services
{
    public class PolicyService : IPolicyService
    {
        private readonly IRepository<Policy> _policyRepository;

        public PolicyService(IRepository<Policy> repository)
        {
            _policyRepository = repository;
        }
        public Guid Add(Policy policy)
        {
            _policyRepository.Add(policy);
            return policy.Id;
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
    }
}
