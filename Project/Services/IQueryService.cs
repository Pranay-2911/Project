using Project.DTOs;
using Project.Models;

namespace Project.Services
{
    public interface IQueryService
    {
        public Guid AddQuery(AddQueryDto dto);
        public PageList<ViewQueryDto> GetAllQuery(PageParameter pageParameter);
        public PageList<ViewQueryDto> GetQueryByCustomer(Guid customerId, PageParameter pageParameter);
        public bool Response(Guid id, QueryResponseDto queryResponseDto);
    }
}
