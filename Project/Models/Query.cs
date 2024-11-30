namespace Project.Models
{
    public class Query
    {
        public Guid Id { get; set; }    
        public string Title {get; set;}
        public string Message {get; set;}
        public string? Reply { get; set;}
    }
}
