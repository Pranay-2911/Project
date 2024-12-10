using Project.DTOs;
using Project.Models;

namespace Project.Services
{
    public interface IQueryService
    {
        public Guid AddQuery(AddQueryDto dto);
        public PageList<ViewQueryDto> GetAllQuery(PageParameter pageParameter, ref int count, string? searchQuery);
        public PageList<ViewQueryDto> GetQueryByCustomer(Guid customerId, PageParameter pageParameter, ref int count, string? searchQuery);
        public bool Response(Guid id, QueryResponseDto queryResponseDto);
    }
}
