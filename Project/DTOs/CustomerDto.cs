using Project.Models;
using System.ComponentModel.DataAnnotations;

namespace Project.DTOs
{
    public class CustomerDto
    {
        public Guid CustomerId { get; set; }
        [Required]
        [StringLength(15, ErrorMessage = "First name should not greater than 15")]
        public string FirstName { get; set; }
        [Required]
        [StringLength(15, ErrorMessage = "First name should not greater than 15")]
        public string LastName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Mobile number must be exactly 10 digits.")]
        public long MobileNumber { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string Nominee { get; set; }
        public string NomineeRelation { get; set; }

        public Guid UserId { get; set; }
        public Guid? AgentId { get; set; }
        public int TotalDocuments { get; set; }
        public int TotalPolicies { get; set; }
    }
}
