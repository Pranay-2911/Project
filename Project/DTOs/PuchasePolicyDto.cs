using Project.Types;
using System.ComponentModel.DataAnnotations;

namespace Project.DTOs
{
    public class PurchasePolicyDto
    {
        [Required]
        public Guid PolicyId { get; set; }
        [Required]
        public double TotalAmount { get; set; }
        [Required]
        public int DurationInYears { get; set; }
        [Required]
        public string Nominee { get; set; }
        [Required]
        public string NomineeRelation { get; set; }
        [Required]
        public int Divider { get; set; }
        public Guid CustomerId { get; set; }
    }
}
