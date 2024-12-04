using Project.Types;

namespace Project.Services
{
    public class EnumService : IEnumService
    {
        public List<string> GetCommisson()
        {
            List<string> commisson = new List<string>();
            foreach (CommissionType type in Enum.GetValues(typeof(CommissionType)))
            {
                commisson.Add(type.ToString());
            }
            return commisson;
        }

        public List<string> GetDocument()
        {
            List<string> documents = new List<string>();
            foreach (DocumentType type in Enum.GetValues(typeof(DocumentType)))
            {
                documents.Add(type.ToString());
            }
            return documents;
        }

        public List<string> GetNominee()
        {
            List<string> nominee = new List<string>();
            foreach (NomineeRelation type in Enum.GetValues(typeof(NomineeRelation)))
            {
                nominee.Add(type.ToString());
            }
            return nominee;
        }
    }
}
