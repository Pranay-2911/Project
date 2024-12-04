using Project.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Project.DTOs
{
    public class AdminDto
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        [StringLength(15, ErrorMessage = "First name should not greater than 15")]
        public string FirstName { get; set; }
        [Required]
        [StringLength(15, ErrorMessage = "Last name should not greater than 15")]
        public string LastName { get; set; }

        public int TotalEmployees { get; set; }
        public int TotalCustomers { get; set; }
        public int TotalAgents { get; set; }
        public int TotalPolicies { get; set; }
    }
}
