using Project.Types;
using System.Reflection.Metadata;

namespace Project.DTOs
{
    public class ViewCommissionDto
    {
        public Guid Id { get; set; }
        public string AgentName { get; set; }
        public string SchemaName { get; set; }
        public string CustomerName { get; set; }
        public double CommissionAmount { get; set; }
        public DateTime CommssionDate { get; set; }
        public CommissionType CommissionType { get; set; }
    }
}
