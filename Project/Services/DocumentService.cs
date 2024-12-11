using AutoMapper;
using Project.DTOs;
using Project.Models;
using Project.Repositories;
using Serilog;

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
            Log.Information("New document record added: " + document.Id);
            return document.Id;
        }

        public bool Delete(Guid id)
        {
            var document = _repository.Get(id);
            if (document != null)
            {
                _repository.Delete(document);
                Log.Information("Document record deleted: " + document.Id);
                return true;
            }
            return false;
        }
    }
}
