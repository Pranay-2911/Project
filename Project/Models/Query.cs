using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Project.Models
{
    public class Query
    {
        [Key]
        [Required]
        public Guid Id { get; set; }
        [Required]
        [MaxLength(100, ErrorMessage ="Query must be in 100 characters")]
        public string Title {get; set;}
        [Required]
        [MaxLength(800, ErrorMessage ="Message must be in 800 characters")]
        public string Message {get; set;}
        public string? Reply { get; set;}
        public Customer Customer { get; set;}
        [ForeignKey("Customer")]
        public Guid CustomerId { get; set;}
    }
}
