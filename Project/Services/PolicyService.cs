using Project.Models;
using Project.Repositories;
using AutoMapper;
using Project.DTOs;

namespace Project.Services
{
    public class PolicyService : IPolicyService
    {
        private readonly IRepository<Policy> _policyRepository;
        private readonly IRepository<Customer> _customerRepository;
        private readonly IRepository<Premium> _premiumRepository;
        private readonly IMapper _mapper;

        public PolicyService(IRepository<Policy> repository, IMapper mapper, IRepository<Customer> customerRepository, IRepository<Premium> premiumRepository)
        {
            _policyRepository = repository;
            _customerRepository = customerRepository;
            _premiumRepository = premiumRepository;
            _mapper = mapper;
        }
        public Guid Add(PolicyDto policydto)
        {
            var policy = _mapper.Map<Policy>(policydto);
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

        public List<PolicyDto> GetAll()
        {
            var policies = _policyRepository.GetAll().ToList();
            var policydtos = _mapper.Map<List<PolicyDto>>(policies);
            return policydtos;
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

        public bool PurchasePolicy(Guid customerId, Guid policyId, double totalAmount, int durationInMonths)
        {
            // 1. Link customer to the policy (add an entry in the policy-customer table if required)
            var policy = _policyRepository.Get(policyId);
            var customer = _customerRepository.Get(customerId);
            customer.Policies.Add(policy);


            // 2. Generate the premium schedule
            var premiums = GeneratePremiumSchedule(customerId, policyId, totalAmount, durationInMonths);

            // 3. Insert premiums into the database
            foreach (var premium in premiums)
            {
                _premiumRepository.Add(premium);
            }
            return true;
        }

        private List<Premium> GeneratePremiumSchedule(Guid customerId, Guid policyId, double totalAmount, int durationInMonths)
        {
            var premiums = new List<Premium>();
            var monthlyAmount = totalAmount / durationInMonths; // Split total amount equally

            for (int i = 0; i < durationInMonths; i++)
            {
                premiums.Add(new Premium
                {
                    CustomerId = customerId,
                    PolicyId = policyId,
                    Amount = monthlyAmount,
                    DueDate = DateTime.Now.AddMonths(i + 1), // Due date is next month onwards
                    Status = "Unpaid"
                });
            }

            return premiums;
        }

    }
}
