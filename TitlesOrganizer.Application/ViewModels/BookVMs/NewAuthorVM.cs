using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TitlesOrganizer.Application.ViewModels.BookVMs
{
    public class NewAuthorVM
    {
        public int Id { get; set; }

        [StringLength(25)]
        public string? Name { get; set; }

        [DisplayName("Last name")]
        [StringLength(25)]
        public string? LastName { get; set; }

        public int BookId { get; set; }

        public string BookTitle { get; set; } = string.Empty;
    }
}