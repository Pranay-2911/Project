

using Project.Models;

namespace Project.Services
{
    public interface IDocumentService
    {
        public Guid Add(Document document);
        public bool Delete(Guid id);        
    }
}
