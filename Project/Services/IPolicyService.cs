using Project.Models;

namespace Project.Services
{
    public interface IPolicyService
    {
        public Guid Add(Policy policy);
        public bool Delete(Guid id);
    }
}
