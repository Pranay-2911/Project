using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Project.DTOs;
using Project.Exceptions;
using Project.Models;
using Project.Repositories;
using Project.Types;

namespace Project.Services
{
    public class AgentService : IAgentService
    {
        private readonly IRepository<Agent> _agentRepository;
        private readonly IRepository<Role> _roleRepository;
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<CommissionRequest> _commissionRequestRepository;
        private readonly IMapper _mapper;

        public AgentService(IRepository<Agent> repository, IMapper mapper, IRepository<Role> roleRepository, IRepository<User> userRepository, IRepository<CommissionRequest> commissionRequestRepository)
        {
            _agentRepository = repository;
            _mapper = mapper;
            _roleRepository = roleRepository;
            _userRepository = userRepository;
            _commissionRequestRepository = commissionRequestRepository;
        }
        public Guid Add(AgentRegisterDto agentRegisterDto)
        {
            var roleName = _roleRepository.GetAll().Where(r => r.RoleName == Roles.AGENT).FirstOrDefault();
            User user = _mapper.Map<User>(agentRegisterDto);
            user.RoleId = roleName.Id;
            user.Status = true;
            _userRepository.Add(user);

            var role = _roleRepository.Get(roleName.Id);
            role.Users.Add(user);
            _roleRepository.Update(role);

            var agent = _mapper.Map<Agent>(agentRegisterDto);
            agent.UserId = user.Id;
            agent.IsVerified = false;

           
            _agentRepository.Add(agent);
            return agent.Id;
        }

        public bool ChangePassword(ChangePasswordDto passwordDto)
        {
            var agent = _agentRepository.GetAll().AsNoTracking().Include(a => a.User).Where(a => a.User.UserName == passwordDto.UserName).FirstOrDefault();
            if (agent != null)
            {
                if (BCrypt.Net.BCrypt.Verify(passwordDto.Password, agent.User.PasswordHash))
                {
                    agent.User.PasswordHash = BCrypt.Net.BCrypt.HashPassword(passwordDto.NewPassword);
                    _agentRepository.Update(agent);
                    return true;
                }
                   
            }
            return false;
        }

        public bool Delete(Guid id)
        {
            var agent = _agentRepository.GetAll().Include(a => a.User).Where(a => a.Id == id).FirstOrDefault();
            if (agent != null)
            {
                var user = _userRepository.Get(agent.UserId);
                user.Status = false;
                _userRepository.Update(user);
                return true;
            }
            throw new AgentNotFoundException("Agent Does Not Exist");
        }

        public AgentDto Get(Guid id)
        {
            var agent = _agentRepository.Get(id);
     
            if (agent != null)
            {
                var agentDto = _mapper.Map<AgentDto>(agent);
                return agentDto;
            }
            throw new AgentNotFoundException("Agent Does Not Exist");
        }

        public List<AgentDto> GetAll()
        {
            var agents = _agentRepository.GetAll().Include(a => a.User).Where(a => a.User.Status == true).Where(a => a.IsVerified == true).ToList();
            var agentDtos = _mapper.Map<List<AgentDto>>(agents);
            return agentDtos;
        }
        public List<AgentDto> GetAllUnVerified()
        {
            var agents = _agentRepository.GetAll().Include(a => a.User).Where(a => a.User.Status == true).Where(a => a.IsVerified == false).ToList();
            var agentDtos = _mapper.Map<List<AgentDto>>(agents);
            return agentDtos;
        }
        public bool VerifyAgent(Guid id)
        {
            var agent = _agentRepository.Get(id);
            if (agent != null)
            {
                agent.IsVerified = true;
                _agentRepository.Update(agent);
                return true;
            }
            return false;
        }

        public bool Update(AgentDto agentDto)
        {
            var existingAgent = _agentRepository.GetAll().AsNoTracking().Where(u => u.Id == agentDto.Id);
            if (existingAgent != null)
            {
                var agent = _mapper.Map<Agent>(agentDto);
                _agentRepository.Update(agent);
                return true;
            }
            throw new AgentNotFoundException("Agent Does Not Exist");
        }

        public double GetBalance(Guid id)
        {
            var agent = _agentRepository.Get(id);
            if (agent != null)
            {
                return agent.CurrentCommisionBalance;
            }
            throw new AgentNotFoundException("Agent Does Not Exist");
        }

        public Guid CommissionRequest(CommisionRequestDto commissionRequestDto)
        {
            var commissionRequest = _mapper.Map<CommissionRequest>(commissionRequestDto);
            commissionRequest.Status = WithdrawStatus.PENDING;
            commissionRequest.RequestDate = DateTime.UtcNow;

            var agent = _agentRepository.Get(commissionRequest.AgentId);
            agent.CurrentCommisionBalance = agent.CurrentCommisionBalance - commissionRequest.Amount;

            _agentRepository.Update(agent);

            _commissionRequestRepository.Add(commissionRequest);
            return commissionRequest.Id;

        }
    }
}
