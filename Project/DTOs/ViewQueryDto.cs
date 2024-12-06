using Project.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Project.DTOs
{
    public class ViewQueryDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public string? Reply { get; set; }
        public Guid CustomerId { get; set; }
    }
}
