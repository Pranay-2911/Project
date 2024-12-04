using System.ComponentModel.DataAnnotations;

namespace Project.DTOs
{
    public class AgentRegisterDto
    {
        [Required]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "First name must be in 2 to 20 characters.")]
        public string FirstName { get; set; }
        [StringLength(20, MinimumLength = 2, ErrorMessage = "First name must be in 2 to 20 characters.")]
        public string LastName { get; set; }
        [Required]
        public string Qualification { get; set; }
        [Required]
        [EmailAddress (ErrorMessage = "Email must be in correct format")]
        public string Email { get; set; }
        [Required]
        [Phone (ErrorMessage = "Phone Number must be in correct format")]
        public long MobileNumber { get; set; }
        [Required]
        [StringLength(20, MinimumLength = 5, ErrorMessage = "username must be in 5 to 20 characters")]
        public string Username { get; set; }
        [Required]
        [StringLength(20, MinimumLength = 7, ErrorMessage = "password must be in 7 to 20 characters")]
        public string Password { get; set; }
    }
}
