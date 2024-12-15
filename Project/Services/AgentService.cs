using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Project.DTOs;
using Project.Exceptions;
using Project.Models;
using Project.Repositories;
using Project.Types;
using Serilog;

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
            var existingUser = _userRepository.GetAll().Where(u=>u.UserName == agentRegisterDto.Username).FirstOrDefault();
            var existingEmail = _agentRepository.GetAll().Where(a=>a.Email == agentRegisterDto.Email).FirstOrDefault();
            var existingNumber = _agentRepository.GetAll().Where(a=>a.MobileNumber == agentRegisterDto.MobileNumber).FirstOrDefault();
            if (existingUser == null && existingEmail == null && existingNumber == null)
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
                Log.Information("New record added: " + agent.Id);
                return agent.Id;
            }
            throw new Exception("UserName already exist");
        }

        public bool ChangePassword(ChangePasswordDto passwordDto)
        {
            var agent = _agentRepository.GetAll().AsNoTracking().Include(a => a.User).Where(a => a.Id == passwordDto.Id).Where(a => a.User.UserName == passwordDto.UserName).FirstOrDefault();
            if (agent != null)
            {
                if (BCrypt.Net.BCrypt.Verify(passwordDto.Password, agent.User.PasswordHash))
                {
                    agent.User.PasswordHash = BCrypt.Net.BCrypt.HashPassword(passwordDto.NewPassword);
                    _agentRepository.Update(agent);
                    Log.Information("password updated " + agent.Id);
                    return true;
                }
                   
            }
            throw new Exception("You Username is not as per details");
        }

        public bool Delete(Guid id)
        {
            var agent = _agentRepository.GetAll().Include(a => a.User).Where(a => a.Id == id).FirstOrDefault();
            if (agent != null)
            {
                var user = _userRepository.Get(agent.UserId);
                user.Status = false;
                _userRepository.Update(user);
                Log.Information("record deleted: " + agent.Id);
                return true;
            }
            throw new AgentNotFoundException("Agent Does Not Exist");
        }

        public bool Active(Guid id)
        {
            var agent = _agentRepository.GetAll().Include(a => a.User).Where(a => a.Id == id).FirstOrDefault();
            if (agent != null)
            {
                var user = _userRepository.Get(agent.UserId);
                user.Status = true;
                _userRepository.Update(user);
                Log.Information("agent record activated: " + agent.Id);
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

        public PageList<AgentDto> GetAll(PageParameter pageParameters, ref int count, string? searchQuery)
        {
            var agents = _agentRepository.GetAll().Include(a => a.User).Where(a => a.IsVerified == true).ToList();
           
            var agentDtos = _mapper.Map<List<AgentDto>>(agents);
            if (!string.IsNullOrEmpty(searchQuery))
            {
                searchQuery = searchQuery.ToLower();
                agentDtos = agentDtos
                    .Where(d => d.FirstName.ToLower().Contains(searchQuery))
                    .ToList();
            }
            count = agentDtos.Count;
            return PageList<AgentDto>.ToPagedList(agentDtos, pageParameters.PageNumber, pageParameters.PageSize);
        }

        public PageList<AgentDto> GetAllUnVerified(PageParameter pageParameters, ref int count, string? searchQuery)
        {
            var agents = _agentRepository.GetAll().Include(a => a.User).Where(a => a.User.Status == true).Where(a => a.IsVerified == false).ToList();
            
            var agentDtos = _mapper.Map<List<AgentDto>>(agents);

            if (!string.IsNullOrEmpty(searchQuery))
            {
                searchQuery = searchQuery.ToLower();
                agentDtos = agentDtos
                    .Where(d => d.FirstName.ToLower().Contains(searchQuery))
                    .ToList();
            }
            count = agentDtos.Count;
            return PageList<AgentDto>.ToPagedList(agentDtos, pageParameters.PageNumber, pageParameters.PageSize);
        }

        public bool VerifyAgent(Guid id)
        {
            var agent = _agentRepository.Get(id);
            if (agent != null)
            {
                agent.IsVerified = true;
                _agentRepository.Update(agent);
                Log.Information("agent verified successfully: " + agent.Id);
                return true;
            }
            return false;
        }

        public bool Update(UpdateAgentDto agentDto)
        {
            var existingAgent = _agentRepository.GetAll().AsNoTracking().Where(u => u.Id == agentDto.Id).FirstOrDefault();
            existingAgent.MobileNumber = 0;
            existingAgent.Email = "";
            _agentRepository.Update(existingAgent);

            var existingEmail = _agentRepository.GetAll().Where(n => n.Email == agentDto.Email).FirstOrDefault();
            var existingNumber = _agentRepository.GetAll().Where(n=>n.MobileNumber == agentDto.MobileNumber).FirstOrDefault();
            if (existingAgent != null && existingEmail == null && existingNumber == null)
            {
               existingAgent.FirstName = agentDto.FirstName;
                existingAgent.LastName = agentDto.LastName;
                existingAgent.Email = agentDto.Email;
                existingAgent.Qualification = agentDto.Qualification;
                existingAgent.MobileNumber = agentDto.MobileNumber;
                
                _agentRepository.Update(existingAgent);
                
                Log.Information("agent record updated: " + existingAgent.Id);
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
            Log.Information("Agent record updated: " + agent.Id);
            Log.Information("commmision added: " + agent.Id);
            return commissionRequest.Id;

        }
    }
}
