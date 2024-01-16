using TitlesOrganizer.Domain.Models.Abstract;
using TitlesOrganizer.Domain.Models.Enums;

namespace TitlesOrganizer.Domain.Models
{
    public class Creator : BaseModel
    {
        public string? Name { get; set; }
        public string? LastName { get; set; }
        public Profession Profession { get; set; }

        public virtual ICollection<Book> Books { get; set; } = new List<Book>();
        public virtual ICollection<Movie> Movies { get; set; } = new List<Movie>();
        public virtual ICollection<TvSeries> TvSeries { get; set; } = new List<TvSeries>();
    }
}