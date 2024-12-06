using System.ComponentModel.DataAnnotations;

namespace Project.DTOs
{
    public class AddQueryDto
    {
        [Required]
        [MaxLength(100, ErrorMessage = "Query must be in 100 characters")]
        public string Title { get; set; }
        [Required]
        [MaxLength(800, ErrorMessage = "Message must be in 800 characters")]
        public string Message { get; set; }
        public Guid CustomerId { get; set; }
    }
}
