using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project.Models
{
    public class Document
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public Customer Customer { get; set; }  
        [ForeignKey("Customer")]
        public Guid CustomerId { get; set; }
        public bool Satus { get; set; }
    }
}
