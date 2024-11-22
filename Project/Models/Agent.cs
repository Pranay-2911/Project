using System.ComponentModel.DataAnnotations;

namespace Project.Models
{
    public class Agent
    {
        [Key]
        public Guid AgentId { get; set; }
        [Required]
        [StringLength(20, MinimumLength = 2, ErrorMessage="First name must be in 2 to 20 characters.")]
        public string FirstName { get; set; }
        [StringLength(20, MinimumLength = 2, ErrorMessage="First name must be in 2 to 20 characters.")]
        public string LastName { get; set; }
        [Required]
        public string Qualification { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public long MobileNumber { get; set; }
        public double CommisionEarned { get; set; }
        public User User { get; set; }
        public List<Customer> Customers { get; set; }

    }
}
