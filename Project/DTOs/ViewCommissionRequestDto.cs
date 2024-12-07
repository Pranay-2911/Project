namespace Project.DTOs
{
    public class ViewCommissionRequestDto
    {
        public Guid Id { get; set; }
        public double Amount { get; set; }
        public DateTime RequestDate { get; set; }
        public string AgentName { get; set; }
    }
}
