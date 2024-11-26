using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project.Models
{
    public class PolicyAccount
    {
        [Key]
        public Guid Id { get; set; }
        public string BankName { get; set; }
        public string IFSC { get; set; }
        public long AccountNumber { get; set; }

        public Customer Customer { get; set; }
        [ForeignKey("Customer")]
        public Guid CustomerId { get; set; }
    }
}
