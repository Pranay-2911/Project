using AutoMapper;
using Project.DTOs;
using Project.Models;
using Project.Repositories;

namespace Project.Services
{
    public class QueryService : IQueryService
    {
        private readonly IRepository<Query> _queryRepository;
        private readonly IMapper _mapper;
        public QueryService(IRepository<Query> queryRepository, IMapper mapper)
        {
            _queryRepository = queryRepository;
            _mapper = mapper;
        }

        public Guid AddQuery(AddQueryDto dto)
        {
            var query = _mapper.Map<Query>(dto);    
            _queryRepository.Add(query);
            return query.Id;
        }
        public List<ViewQueryDto> GetAllQuery()
        {
            var query = _queryRepository.GetAll().ToList();
            var queryDto = _mapper.Map<List<ViewQueryDto>>(query);
            return queryDto;
        }
    }
}
