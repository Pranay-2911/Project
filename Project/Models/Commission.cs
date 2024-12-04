using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Project.Types;
namespace Project.Models
{
    public class Commission
    {
        [Key]
        [Required]
        public Guid Id {  get; set; }
        [Required]
        public Guid PolicyId { get; set; }
        public Agent Agent { get; set; }
        [ForeignKey("Agent")]
        public Guid AgentId { get; set; }

        public double CommissionAmount { get; set; }
        public DateTime EarnedDate { get; set; }
        public CommissionType CommissionType { get; set; }
    }
}
