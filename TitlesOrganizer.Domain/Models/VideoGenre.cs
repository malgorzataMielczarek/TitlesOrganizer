using TitlesOrganizer.Domain.Models.Abstract;

namespace TitlesOrganizer.Domain.Models
{
    public class VideoGenre : IBaseModel
    {
        public int Id { get; set; }
        public required string Name { get; set; }

        public virtual ICollection<Movie>? Movies { get; set; }
        public virtual ICollection<TvSeries>? TvSeries { get; set; }
    }
}