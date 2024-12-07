using Project.Types;

namespace Project.Services
{
    public interface IEnumService
    {
        public List<string> GetCommisson();
        public List<string> GetDocument();
        public List<string> GetNominee();
        public List<string> GetWithDrawStatus();

    }
}
