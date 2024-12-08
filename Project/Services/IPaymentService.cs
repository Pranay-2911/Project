using Project.DTOs;
using Project.Models;

namespace Project.Services
{
    public interface IPaymentService
    {
        public PageList<ShowPaymentDto> GetAll(PageParameter pageParameter);
    }
}
