using TitlesOrganizer.Domain.Models.Abstract;

namespace TitlesOrganizer.Domain.Models
{
    public class Movie : BaseTitleModel
    {
        public int? Year { get; set; }
        public int? SeriesId { get; set; }
        public int? NumberInSeries { get; set; }

        public virtual ICollection<VideoGenre> Genres { get; set; } = new List<VideoGenre>();
        public virtual ICollection<Creator> Creators { get; set; } = new List<Creator>();
        public virtual ICollection<Country> Countries { get; set; } = new List<Country>();
        public MovieSeries? Series { get; set; }
    }
}