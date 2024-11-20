using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project.Models
{
    public class Document
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Location { get; set; }
        
        public Customer Customer { get; set; }  
        [ForeignKey("Customer")]
        public Guid CustomerId { get; set; }
    }
}
