using System.ComponentModel.DataAnnotations;

namespace Project.DTOs
{
    public class PolicyAccountDto
    {
        [Key]
        public Guid Id { get; set; }
        public string BankName { get; set; }
        public string IFSC { get; set; }
        public long AccountNumber { get; set; }

        public Guid CustomerId { get; set; }
    }
}
