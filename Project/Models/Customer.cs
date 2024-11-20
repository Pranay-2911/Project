using System.ComponentModel.DataAnnotations;

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
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public int MobileNumber { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string Nominee { get; set; }
        public string NomineeRelation { get; set; }

        public Agent? Agent { get; set; }
        // policies
        public List<Document> Documents { get; set; }
        public bool Satus { get; set; }

        public List<Policy> Policies { get; set; }


    }
}
