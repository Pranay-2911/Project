using System.ComponentModel.DataAnnotations;

namespace Project.DTOs
{
    public class PolicyAccountDto
    {
        public string BankName { get; set; }
        public string IFSC { get; set; }
        public long AccountNumber { get; set; }

        public Guid CustomerId { get; set; }
    }
}
