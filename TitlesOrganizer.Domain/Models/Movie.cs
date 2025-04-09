using TitlesOrganizer.Domain.Models.Abstract;

namespace TitlesOrganizer.Domain.Models
{
    public class Movie : WorkModel
    {
        public int? Year { get; set; }
        public int? SeriesId { get; set; }
        public int? NumberInSeries { get; set; }

        public virtual ICollection<Genre> Genres { get; set; } = [];
        public virtual ICollection<Creator> Creators { get; set; } = [];
        public virtual ICollection<Country> Countries { get; set; } = [];
        public virtual Series? Series { get; set; }
    }
}