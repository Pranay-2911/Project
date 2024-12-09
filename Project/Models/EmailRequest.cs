namespace Project.Models
{
    public class EmailRequest
    {
        
        public Guid Id {  get; set; }
        public string To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }
}
