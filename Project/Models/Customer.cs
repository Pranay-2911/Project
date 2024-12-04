using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project.Models
{
    public class Customer
    {
        [Key]
        public Guid CustomerId { get; set; }
        [Required]
        [StringLength(15, ErrorMessage = "First name should not greater than 15")]
        public string FirstName { get; set; }
        [Required]
        [StringLength(15, ErrorMessage = "First name should not greater than 15")]
        public string LastName { get; set; }
        [Required]
        [EmailAddress (ErrorMessage ="Email Should be at correct format")]
        public string Email { get; set; }
        [Required]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Mobile number must be exactly 10 digits.")]
        [Phone(ErrorMessage ="Mobile Number must be in correct format")]
        public long MobileNumber { get; set; }
        [Required]
        public string State { get; set; }
        [Required]
        public string City { get; set; }
        //public string Nominee { get; set; }
        //public string NomineeRelation { get; set; }
        public bool IsKYC { get; set; }
        public DateOnly DateOfBirth {  get; set; }
        public User User { get; set; }
        [ForeignKey("User")]
        public Guid UserId { get; set; }

        public Agent? Agent { get; set; }
        [ForeignKey("Agent")]
        public Guid? AgentId { get; set; }
        public List<Document> Documents { get; set; }
        public List<PolicyAccount> Accounts { get; set; }


    }
}
