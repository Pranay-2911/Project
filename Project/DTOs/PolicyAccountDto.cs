using Project.Types;
using System.ComponentModel.DataAnnotations;

namespace Project.DTOs
{
    public class PolicyAccountDto
    {
        public Guid Id { get; set; }
        [Required]
        public string CustomerName { get; set; }
        [Required]
        public string PolicyName { get; set; }
        public string AgentName {get; set;}
        [Required]
        public DateTime PurchasedDate { get; set;}
        [Required]
        public string Nominee { get; set; }
        [Required]
        public NomineeRelation NomineeRelation { get; set; }
        [Required]
        public double PolicyAmount { get; set; }
        [Required]
        public int PolicyDuration {  get; set; }
        public WithdrawStatus IsVerified { get; set; }
        public Guid PolicyId {  get; set; }
        public MatureStatus IsMatured {  get; set; }
    }
}
