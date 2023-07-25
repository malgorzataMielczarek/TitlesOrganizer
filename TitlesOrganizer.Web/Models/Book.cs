using TitlesOrganizer.Web.Models.Common;

namespace TitlesOrganizer.Web.Models
{
    public class Book : Item
    {
        public List<string> Authors { get; set; }
        public string? OriginalLanguage { get; set; }
        public string? Edition { get; set; }

        public Book()
        {
            Authors = new List<string>();
        }
    }
}