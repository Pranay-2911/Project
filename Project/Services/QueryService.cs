using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Project.DTOs;
using Project.Models;
using Project.Repositories;

namespace Project.Services
{
    public class QueryService : IQueryService
    {
        private readonly IRepository<Query> _queryRepository;
        private readonly IRepository<Customer> _customerRepository;
        private readonly IMapper _mapper;
        public QueryService(IRepository<Query> queryRepository, IMapper mapper, IRepository<Customer> customerRepository)
        {
            _queryRepository = queryRepository;
            _mapper = mapper;
            _customerRepository = customerRepository;
        }

        public Guid AddQuery(AddQueryDto dto)
        {
            var query = _mapper.Map<Query>(dto);    
            _queryRepository.Add(query);
            return query.Id;
        }
        public PageList<ViewQueryDto> GetAllQuery(PageParameter pageParameter, ref int count)
        {
            var queries = _queryRepository.GetAll().Where(q => q.Reply == null).ToList();
            List<ViewQueryDto> viewCommissionDtos = new List<ViewQueryDto>();
           foreach(var query in queries)
            {
                var custommer = _customerRepository.Get(query.CustomerId);
                var queryDto = new ViewQueryDto()
                {
                    CustomerName = $"{custommer.FirstName} {custommer.LastName}",
                    Id = query.Id,
                    Message = query.Message,
                    Title = query.Title,
                    
                };
                viewCommissionDtos.Add(queryDto);
            }
            count = viewCommissionDtos.Count;
            return PageList<ViewQueryDto>.ToPagedList(viewCommissionDtos, pageParameter.PageNumber, pageParameter.PageSize);
        }

        public PageList<ViewQueryDto> GetQueryByCustomer(Guid customerId, PageParameter pageParameter, ref int count)
        {
            var queries = _queryRepository.GetAll().Where(q => q.CustomerId == customerId).ToList();
            List<ViewQueryDto> viewCommissionDtos = new List<ViewQueryDto>();
            foreach (var query in queries)
            {
                var custommer = _customerRepository.Get(query.CustomerId);
                var queryDto = new ViewQueryDto()
                {
                    CustomerName = $"{custommer.FirstName} {custommer.LastName}",
                    Id = query.Id,
                    Message = query.Message,
                    Reply = query.Reply,
                    Title = query.Title,

                };
                viewCommissionDtos.Add(queryDto);
            }
            count = viewCommissionDtos.Count;
            return PageList<ViewQueryDto>.ToPagedList(viewCommissionDtos, pageParameter.PageNumber, pageParameter.PageSize);
        }

        public bool Response(Guid id, QueryResponseDto queryResponseDto)
        {
            var query = _queryRepository.Get(id);
            if (query != null)
            {
                query.Reply = queryResponseDto.Reply;
                _queryRepository.Update(query);
                return true;
            }
            return false;
        }
        
    }
}
