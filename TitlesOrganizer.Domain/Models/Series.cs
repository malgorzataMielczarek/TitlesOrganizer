using TitlesOrganizer.Domain.Models.Abstract;

namespace TitlesOrganizer.Domain.Models
{
    public class Series : WorkModel
    {
        public virtual ICollection<Book> Books { get; set; } = [];
        public virtual ICollection<Movie> Movies { get; set; } = [];
    }
}