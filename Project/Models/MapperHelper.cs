namespace Project.Models
{
    public class MapperHelper
    {
        public static List<string> SplitDocuments(string documents)
        {
            return string.IsNullOrEmpty(documents) ? new List<string>() : documents.Split(',').ToList();
        }
    }
}
