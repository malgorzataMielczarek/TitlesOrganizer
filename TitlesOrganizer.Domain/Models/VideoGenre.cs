using TitlesOrganizer.Domain.Models.Abstract;

namespace TitlesOrganizer.Domain.Models
{
    public class VideoGenre : BaseNameModel
    {
        public virtual ICollection<Movie>? Movies { get; set; }
        public virtual ICollection<TvSeries>? TvSeries { get; set; }
    }
}