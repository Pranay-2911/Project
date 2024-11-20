using System.ComponentModel.DataAnnotations;

namespace Project.Models
{
    public class Customer
    {
        [Key]
        public Guid CustomerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public int MobileNumber { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string Nominee { get; set; }
        public string NomineeRelation { get; set; }

        public Agent Agent { get; set; }
        // policies
        public List<Document> Documents { get; set; }

    }
}
