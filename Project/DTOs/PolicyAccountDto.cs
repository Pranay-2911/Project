using Project.Types;
using System.ComponentModel.DataAnnotations;

namespace Project.DTOs
{
    public class PolicyAccountDto
    {
        [Required]
        public Guid CustomerId { get; set; }
        [Required]
        public Guid PolicyID { get; set; }
        public Guid? AgentId {get; set;}
        [Required]
        public DateTime PurchasedDate { get; set;}
        [Required]
        public string Nominee { get; set; }
        [Required]
        public NomineeRelation NomineeRelation { get; set; }
    }
}
