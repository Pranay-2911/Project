using AutoMapper;
using Project.DTOs;
using Project.Models;
using Project.Repositories;

namespace Project.Services
{
    public class CommissionRequestService : ICommissionRequestService
    {
        private readonly IRepository<CommissionRequest> _commissionRequestRepository;
        private readonly IRepository<Agent> _agentRepository;
        private readonly IMapper _mapper;
        public CommissionRequestService(IRepository<CommissionRequest> commissionRequestRepository, IMapper mapper, IRepository<Agent> agentRepository)
        {
            _commissionRequestRepository = commissionRequestRepository;
            _agentRepository = agentRepository;
            _mapper = mapper;
        }

        public PageList<ViewCommissionRequestDto> GetAllPendingRequest(PageParameter pageParameter)
        {
            var requests = _commissionRequestRepository.GetAll().Where(r => r.Status == Types.WithdrawStatus.PENDING).ToList();
            List<ViewCommissionRequestDto> viewCommissionRequestDtos = new List<ViewCommissionRequestDto>();
            foreach (var request in requests)
            {
                var agent = _agentRepository.Get(request.AgentId);
                var requestDto = new ViewCommissionRequestDto()
                {
                    Amount = request.Amount,
                    RequestDate = request.RequestDate,
                    Id = request.Id,
                    AgentName = $"{agent.FirstName} {agent.LastName}"

                };

                viewCommissionRequestDtos.Add(requestDto);
            }


            return PageList<ViewCommissionRequestDto>.ToPagedList(viewCommissionRequestDtos, pageParameter.PageNumber, pageParameter.PageSize);
        }

        public List<CommissionRequest> GetRequestByAgent(Guid id)
        {
            var requests = _commissionRequestRepository.GetAll().Where(r => r.AgentId == id).ToList();
            return requests;
        }

        public bool Approve(Guid id)
        {
            var request = _commissionRequestRepository.Get(id);
            if (request != null)
            {
                request.Status = Types.WithdrawStatus.APPROVED;
                _commissionRequestRepository.Update(request);
                return true;
            }
            return false;
        }
        public bool Reject(Guid id)
        {
            var request = _commissionRequestRepository.Get(id);
            if (request != null)
            {
                request.Status = Types.WithdrawStatus.REJECTED;
                _commissionRequestRepository.Update(request);
                var agent = _agentRepository.Get(request.AgentId);
                agent.CurrentCommisionBalance = agent.CurrentCommisionBalance  + request.Amount;
                _agentRepository.Update(agent);
                return true;
            }
            return false;
        }
    }
}
