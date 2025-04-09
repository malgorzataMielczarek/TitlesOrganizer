using TitlesOrganizer.Domain.Models.Abstract;
using TitlesOrganizer.Domain.Models.Enums;

namespace TitlesOrganizer.Domain.Models
{
    public class Genre : BaseModel
    {
        public required string Name { get; set; }
        public required MediaType ForMedia { get; set; }

        public virtual ICollection<Book>? Books { get; set; }
        public virtual ICollection<Movie>? Movies { get; set; }
        public virtual ICollection<TvSeries>? TvSeries { get; set; }
    }
}