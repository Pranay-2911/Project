using Project.Types;

namespace Project.Models
{
    public class CommissionRequest
    {
        public Guid Id { get; set; }    
        public double Amount { get; set; }
        public WithdrawStatus Status { get; set; }
        public DateTime RequestDate {  get; set; }
        public Guid AgentId { get; set; }

    }
}
