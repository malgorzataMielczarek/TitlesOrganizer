namespace TitlesOrganizer.Web.Models.Common
{
    public class Item : BaseItem
    {
        public string? OriginalTitle { get; set; }
        public string? Description { get; set; }
        public int? Year { get; set; }
        public List<string> Categories { get; set; }

        public Item()
        {
            Categories = new List<string>();
        }
    }
}