namespace Project.DTOs
{
    public class ShowPaymentDto
    {
        public double Amount { get; set; }
        public DateTime PaymentDate {  get; set; }
        public string Status { get; set; }
        public string PolicyName {  get; set; }
        public string CustomerName {  get; set; }
    }
}
