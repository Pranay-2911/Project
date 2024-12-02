using Project.Types;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project.Models
{
    public class PolicyAccount
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public Guid PolicyID { get; set; }
        public Guid? AgentId { get; set; }
        public DateTime PurchasedDate { get; set; }
        public string Nominee { get; set; }
        public NomineeRelation NomineeRelation { get; set; }
    }
}
