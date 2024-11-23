using Project.Models;
using Project.Repositories;

namespace Project.Services
{
    public class DocumentService : IDocumentService
    {
        private readonly IRepository<Document> _repository;

        public DocumentService(IRepository<Document> repository)
        {
            _repository = repository;
        }
        public Guid Add(Document document)
        {
            _repository.Add(document);
            return document.Id;
        }

        public bool Delete(Guid id)
        {
            var document = _repository.Get(id);
            if (document != null)
            {
                _repository.Delete(document);
                return true;
            }
            return false;
        }
    }
}
