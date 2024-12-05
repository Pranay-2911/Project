using Project.Types;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project.Models
{
    public class PolicyAccount
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public Guid CustomerId { get; set; }
        [Required]
        public Guid PolicyID { get; set; }

        public Guid? AgentId { get; set; }
        [Required]
        public DateTime PurchasedDate { get; set; }
        [Required]
        public double PolicyAmount {  get; set; }
        public int PolicyDuration { get; set; }
        [Required]
        public string Nominee { get; set; }
        [Required]
        public NomineeRelation NomineeRelation { get; set; }
    }
}
