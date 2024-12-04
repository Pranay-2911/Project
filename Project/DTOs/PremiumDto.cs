using System.ComponentModel.DataAnnotations;
namespace Project.DTOs
{
    public class PremiumDto
    {
        [Key]
        [Required]
        public Guid Id { get; set; }
        [Required]
        public DateTime DueDate { get; set; }
        [Required]
        public double Amount { get; set; }
        [Required]
        public string Status { get; set; }
        [Required]
        public DateTime? PaymentDate { get; set; }
    }
}
