using Project.Types;
using System.ComponentModel.DataAnnotations;

namespace Project.DTOs
{
    public class RoleDto
    {
        public Guid Id { get; set; }
        public string RoleName { get; set; }
        public int TotalUser { get; set; }
    }
}
