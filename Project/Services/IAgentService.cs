using Project.DTOs;
using Project.Models;

namespace Project.Services
{
    public interface IAgentService
    {
        public Guid Add(AgentDto agentDto);
        public bool Update(AgentDto agentDto);
        public List<Agent> GetAll();
        public AgentDto Get(Guid id);
        public bool Delete(Guid id);
        public bool ChangePassword(ChnagePasswordDto passwordDto);
    }
}
