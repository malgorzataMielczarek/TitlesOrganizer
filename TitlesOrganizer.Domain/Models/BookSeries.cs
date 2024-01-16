using TitlesOrganizer.Domain.Models.Abstract;

namespace TitlesOrganizer.Domain.Models
{
    public class BookSeries : BaseTitleModel
    {
        public ICollection<Book> Books { get; set; } = new List<Book>();
    }
}