using System.ComponentModel.DataAnnotations;
namespace Project.DTOs
{
    public class PaymentDto
    {
        [Required]
        public double Amount { get; set; }
        [Required]
        public bool Status { get; set; }
    }
}
