using Project.Types;
using System.ComponentModel.DataAnnotations;

namespace Project.DTOs
{
    public class PolicyAccountDto
    {
        public Guid CustomerId { get; set; }
        public Guid PolicyID { get; set; }
        public Guid? AgentId {get; set;}
        public DateTime PurchasedDate { get; set;}
        public string Nominee { get; set; }
        public NomineeRelation NomineeRelation { get; set; }
    }
}
