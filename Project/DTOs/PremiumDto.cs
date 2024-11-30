namespace Project.DTOs
{
    public class PremiumDto
    {
        public Guid Id { get; set; }
        public DateTime DueDate { get; set; }
        public double Amount { get; set; }
        public string Status { get; set; } 
        public DateTime? PaymentDate { get; set; }
    }
}
