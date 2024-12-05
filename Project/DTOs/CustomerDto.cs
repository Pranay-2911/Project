using Project.Models;
using System.ComponentModel.DataAnnotations;

namespace Project.DTOs
{
    public class CustomerDto
    {
        [Key]
        [Required]
        public Guid CustomerId { get; set; }
        [Required]
        [StringLength(15, ErrorMessage = "First name should not greater than 15")]
        public string FirstName { get; set; }
        [Required]
        [StringLength(15, ErrorMessage = "First name should not greater than 15")]
        public string LastName { get; set; }
        [Required]
        [EmailAddress(ErrorMessage ="Email must be in correct format")]
        public string Email { get; set; }
        [Required]
        [Phone(ErrorMessage ="Mobile Number must be in correct format")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Mobile number must be exactly 10 digits.")]
        public long MobileNumber { get; set; }
        [Required]
        public string State { get; set; }
        [Required]
        public string City { get; set; }
        //public string Nominee { get; set; }
        //public string NomineeRelation { get; set; }
        [Required]
        public Guid UserId { get; set; }
        public Guid? AgentId { get; set; }
        public int TotalPolicies { get; set; }
        public bool IsKYC { get; set; }
        public DateOnly DateOfBirth { get; set; }

    }
}
