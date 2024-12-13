using AutoMapper;
using MailKit.Search;
using Project.DTOs;
using Project.Models;
using Project.Repositories;
using Serilog;
using Project.Types;

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

        public PageList<ViewCommissionRequestDto> GetAllPendingRequest(PageParameter pageParameter, ref int count, string? searchQuery)
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
            if (!string.IsNullOrEmpty(searchQuery))
            {
                searchQuery = searchQuery.ToLower();
                viewCommissionRequestDtos = viewCommissionRequestDtos
                    .Where(d => d.AgentName.ToLower().Contains(searchQuery))
                    .ToList();
            }
            count = viewCommissionRequestDtos.Count;
            viewCommissionRequestDtos = viewCommissionRequestDtos.OrderByDescending(d => d.RequestDate).ToList();
            return PageList<ViewCommissionRequestDto>.ToPagedList(viewCommissionRequestDtos, pageParameter.PageNumber, pageParameter.PageSize);
        }

        public PageList<CommissionRequest> GetRequestByAgent(Guid id, PageParameter pageParameter, ref int count, string? selectedCommissionType)
        {
            var requests = _commissionRequestRepository.GetAll().OrderByDescending( c => c.RequestDate).Where(r => r.AgentId == id).ToList();
            if(!string.IsNullOrEmpty(selectedCommissionType))
            {
                var type = int.Parse(selectedCommissionType);
                requests = requests.Where(r=>r.Status == (WithdrawStatus)type).ToList();
            }
            count = requests.Count;
            return PageList<CommissionRequest>.ToPagedList(requests, pageParameter.PageNumber, pageParameter.PageSize);
        }

        public bool Approve(Guid id)
        {
            var request = _commissionRequestRepository.Get(id);
            if (request != null)
            {
                request.Status = Types.WithdrawStatus.APPROVED;
                _commissionRequestRepository.Update(request);
                Log.Information("Commission request approved: " + request.Id);
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
                Log.Information("commission request rejected: " + request.Id);
                Log.Information("agent record updated: " + agent.Id);
                return true;
            }
            return false;
        }
    }
}
