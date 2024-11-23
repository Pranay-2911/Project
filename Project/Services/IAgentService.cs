using Project.DTOs;

namespace Project.Services
{
    public interface IAgentService
    {
        public Guid Add(AgentDto agentDto);
        public bool Update(AgentDto agentDto);
        public List<AgentDto> GetAll();
        public AgentDto Get(Guid id);
        public bool Delete(Guid id);
    }
}
