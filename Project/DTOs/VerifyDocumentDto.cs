using Project.Models;
using System.Reflection.Metadata;

namespace Project.DTOs
{
    public class VerifyDocumentDto
    {
        public Guid PolicyAccountId { get; set; }
        public string PolicyName { get; set; }
        public string CustomerName { get; set; }
        public List<Models.Document> Documents { get; set; }

    }
}
