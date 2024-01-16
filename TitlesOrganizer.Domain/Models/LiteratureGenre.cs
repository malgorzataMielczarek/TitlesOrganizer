using TitlesOrganizer.Domain.Models.Abstract;

namespace TitlesOrganizer.Domain.Models
{
    public class LiteratureGenre : BaseNameModel
    {
        public virtual ICollection<Book>? Books { get; set; }
    }
}