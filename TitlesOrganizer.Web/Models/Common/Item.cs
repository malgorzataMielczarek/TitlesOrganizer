using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;

namespace TitlesOrganizer.Web.Models.Common
{
    public class Item : BaseItem
    {
        [DisplayName("Original title")]
        public string? OriginalTitle { get; set; }

        public string? Description { get; set; }

        public int? Year { get; set; }

        public List<string> Genres { get; set; }

        public Item()
        {
            Genres = new List<string>();
        }
    }
}