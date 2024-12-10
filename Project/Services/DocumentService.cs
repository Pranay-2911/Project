using AutoMapper;
using Project.DTOs;
using Project.Models;
using Project.Repositories;

namespace Project.Services
{
    public class DocumentService : IDocumentService
    {
        private readonly IRepository<Document> _repository;
        private readonly IMapper _mapper;

        public DocumentService(IRepository<Document> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public Guid Add(DocumentDto documentDto)
        {
            var document = _mapper.Map<Document>(documentDto);
            document.isActive = true;
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
