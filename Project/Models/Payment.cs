using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Project.Models
{
    public class Payment
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public double AmountPaid { get; set; }
        [Required]
        public DateTime PaymentDate { get; set; }
        [Required]
        public string PaymentStatus { get; set; }

        public Premium Premium { get; set; }
        [ForeignKey("Premium")]
        public Guid PremiumId { get; set; }
    }
}
