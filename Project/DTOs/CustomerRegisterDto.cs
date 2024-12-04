using System.ComponentModel.DataAnnotations;

namespace Project.DTOs
{
    public class CustomerRegisterDto
    {
        [Required]
        [StringLength(15, ErrorMessage = "First name should not greater than 15")]
        public string FirstName { get; set; }
        [Required]
        [StringLength(15, ErrorMessage = "First name should not greater than 15")]
        public string LastName { get; set; }
        [Required]
        [EmailAddress(ErrorMessage="Email must be in correct format")]
        public string Email { get; set; }
        [Required]
        [Phone (ErrorMessage = "Mobile number must be in correct format.")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Mobile number must be exactly 10 digits.")]
        public long MobileNumber { get; set; }
        [Required]
        public string State { get; set; }
        [Required]
        public string City { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 5, ErrorMessage = "username must be in 5 to 20 characters")]
        public string UserName { get; set; }
        [Required]
        [StringLength(20, MinimumLength = 7, ErrorMessage = "password must be in 7 to 20 characters")]
        public string Password { get; set; }

        public Guid? UserId { get; set; }
    }
}
