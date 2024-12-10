using Project.DTOs;
using Project.Models;

namespace Project.Services
{
    public interface ICommissionRequestService
    {
        public PageList<ViewCommissionRequestDto> GetAllPendingRequest(PageParameter pageParameter, ref int count, string? searchQuery);
        public PageList<CommissionRequest> GetRequestByAgent(Guid id, PageParameter pageParameter, ref int count);
        public bool Approve(Guid id);
        public bool Reject(Guid id);
    }
}
