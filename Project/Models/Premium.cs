using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace Project.Models
{
    public class Premium
    {
        [Key]
        public Guid Id { get; set; }
        public double Amount { get; set; }
        public DateTime DueDate { get; set; }
        public string Status { get; set; }
        public DateTime? PaymentDate { get; set; }

        public Policy Policy { get; set; }
        [ForeignKey("Policy")]
        public Guid PolicyId { get; set; }

        public Customer Customer { get; set; }
        [ForeignKey("Customer")]
        public Guid CustomerId { get; set; }
    }
}
