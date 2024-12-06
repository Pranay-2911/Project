using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project.Models
{
    public class Employee
    {

        [Key]
        public Guid Id { get; set; }
        [Required]
        [StringLength(15, ErrorMessage = "First name should not greater than 15")]
        public string FirstName { get; set; }
        [Required]
        [StringLength(15, ErrorMessage = "Last name should not greater than 15")]
        public string LastName { get; set; }
        [Required]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Mobile number must be exactly 10 digits.")]
        [Phone(ErrorMessage ="Mobile Number must be in correct format")]
        public long MobileNumber { get; set; }
        [Required]
        [EmailAddress(ErrorMessage ="Email must be in correct format")]
        public string Email { get; set; }
        [Required]
        public double Salary { get; set; }
        public User User { get; set; }
        [ForeignKey("User")]
        public Guid UserId { get; set; }

    }
}
