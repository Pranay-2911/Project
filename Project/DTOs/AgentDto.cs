using Project.Models;
using System.ComponentModel.DataAnnotations;

namespace Project.DTOs
{
    public class AgentDto
    {
        [Key]
        [Required]
        public Guid Id { get; set; }
        [Required]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "First name must be in 2 to 20 characters.")]
        public string FirstName { get; set; }
        [StringLength(20, MinimumLength = 2, ErrorMessage = "First name must be in 2 to 20 characters.")]
        public string LastName { get; set; }
        [Required]
        public string Qualification { get; set; }
        [Required]
        [EmailAddress (ErrorMessage = "Email is not in correct format.")]
        public string Email { get; set; }
        [Required]
        [Phone (ErrorMessage = "Mobile Number is not in correct format.")]
        public long MobileNumber { get; set; }
        public bool IsVerified { get; set; }
        public double CurrentCommisionBalance { get; set; }
        public Guid UserId { get; set; }
        //public int TotalCustomers { get; set; }
        public double TotalCommissionEarned { get; set; }
        public bool IsActive { get; set; }
        //public double TotalWithdrawalAmount { get; set; }
    }
}
