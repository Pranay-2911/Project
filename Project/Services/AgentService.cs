using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Project.DTOs;
using Project.Models;
using Project.Repositories;

namespace Project.Services
{
    public class AgentService : IAgentService
    {
        private readonly IRepository<Agent> _agentRepository;
        private readonly IMapper _mapper;

        public AgentService(IRepository<Agent> repository, IMapper mapper)
        {
            _agentRepository = repository;
            _mapper = mapper;
        }
        public Guid Add(AgentDto agentDto)
        {
            var agent = _mapper.Map<Agent>(agentDto);
            _agentRepository.Add(agent);
            return agent.Id;
        }

        public bool ChangePassword(ChnagePasswordDto passwordDto)
        {
            var agent = _agentRepository.GetAll().AsNoTracking().Include(a => a.User).Where(a => a.Id == passwordDto.Guid).FirstOrDefault();
            if (agent != null)
            {
                agent.User.Password = passwordDto.Password;
                _agentRepository.Update(agent);
                return true;
            }
            return false;
        }

        public bool Delete(Guid id)
        {
            var agent = _agentRepository.Get(id);
            if (agent != null)
            {
                _agentRepository.Delete(agent);
                return true;
            }
            return false;
        }

        public AgentDto Get(Guid id)
        {
            var agent = _agentRepository.Get(id);
            if (agent != null)
            {
                var agentDto = _mapper.Map<AgentDto>(agent);
                return agentDto;
            }
            throw new Exception("No such agent exist");
        }

        public List<Agent> GetAll()
        {
            var agents = _agentRepository.GetAll().Include(a => a.User).ToList();
            //var agentDtos = _mapper.Map<List<AgentDto>>(agents);
            return agents;
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
            return false;
        }
    }
}
