using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Project.Models
{
    public class Payment
    {
        [Key]
        public Guid Id { get; set; }
        public double AmountPaid { get; set; }
        public DateTime PaymentDate { get; set; }
        public string PaymentStatus { get; set; }

        public Premium Premium { get; set; }
        [ForeignKey("Premium")]
        public Guid PremiumId { get; set; }
    }
}
