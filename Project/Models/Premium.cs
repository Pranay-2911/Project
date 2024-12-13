using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace Project.Models
{
    public class Premium
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public double Amount { get; set; }
        [Required]
        public DateTime DueDate { get; set; }
        public string Status { get; set; }
        public DateTime? PaymentDate { get; set; }

        public Policy Policy { get; set; }
        [ForeignKey("Policy")]
        public Guid PolicyId { get; set; }

        public Customer Customer { get; set; }
        [ForeignKey("Customer")]
        public Guid CustomerId { get; set; }

        public Guid? AgentId { get; set; }  

        public Guid AccountId {  get; set; }
    }
}
