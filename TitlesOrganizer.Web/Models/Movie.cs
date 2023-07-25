using TitlesOrganizer.Web.Models.Common;

namespace TitlesOrganizer.Web.Models
{
    public class Movie : Item
    {
        public List<string> Directors { get; set; }
        public List<string> Countries { get; set; }

        public Movie()
        {
            Countries = new List<string>();
            Directors = new List<string>();
        }
    }
}