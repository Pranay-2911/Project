using Project.DTOs;
using Project.Models;

namespace Project.Services
{
    public interface ICommissionRequestService
    {
        public PageList<ViewCommissionRequestDto> GetAllPendingRequest(PageParameter pageParameter);
        public List<CommissionRequest> GetRequestByAgent(Guid id);
        public bool Approve(Guid id);
        public bool Reject(Guid id);
    }
}
