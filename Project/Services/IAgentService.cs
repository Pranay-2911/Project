using Project.DTOs;
using Project.Models;

namespace Project.Services
{
    public interface IAgentService
    {
        public Guid Add(AgentRegisterDto agentRegisterDto);
        public bool Update(AgentDto agentDto);
        public PageList<AgentDto> GetAll(PageParameter pageParameters);
        public AgentDto Get(Guid id);
        public bool Delete(Guid id);
        public bool ChangePassword(ChangePasswordDto passwordDto);
        public PageList<AgentDto> GetAllUnVerified(PageParameter pageParameters);
        public bool VerifyAgent(Guid id);
        public double GetBalance(Guid id);
        public Guid CommissionRequest(CommisionRequestDto commissionRequestDto);
    }
}
