using Project.DTOs;

namespace Project.Services
{
    public interface IQueryService
    {
        public Guid AddQuery(AddQueryDto dto);
        public List<ViewQueryDto> GetAllQuery();
    }
}
