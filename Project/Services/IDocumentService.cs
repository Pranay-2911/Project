

using Project.DTOs;
using Project.Models;

namespace Project.Services
{
    public interface IDocumentService
    {
        public Guid Add(DocumentDto documentDto);
        public bool Delete(Guid id);        
    }
}
