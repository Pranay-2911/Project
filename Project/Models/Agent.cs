using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project.Models
{
    public class Agent
    {
        [Key]
        [Required]
        public Guid Id { get; set; }
        [Required]
        [StringLength(20, MinimumLength = 2, ErrorMessage="First name must be in 2 to 20 characters.")]
        public string FirstName { get; set; }
        [StringLength(20, MinimumLength = 2, ErrorMessage="First name must be in 2 to 20 characters.")]
        public string LastName { get; set; }
        [Required]
        public string Qualification { get; set; }
        [Required]
        [EmailAddress(ErrorMessage ="Email must be in correct format")]
        public string Email { get; set; }
        [Required]
        [Phone(ErrorMessage = "Mobile Number must be in correct format")]
        public long MobileNumber { get; set; }
        public User User { get; set; }
        [ForeignKey("User")]
        public Guid UserId { get; set; }

        public bool IsVerified { get; set; }

        public double CurrentCommisionBalance { get; set; }
        public double TotalCommissionEarned { get; set; }

    }
}
