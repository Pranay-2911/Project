using Project.Types;

namespace Project.DTOs
{
    public class PurchasePolicyRequestDto
    {
        public Guid PolicyId { get; set; }
        public double TotalAmount { get; set; }
        public int DurationInMonths { get; set; }
        public string Nominee { get; set; }
        public NomineeRelation NomineeRelation { get; set; }
    }
}
