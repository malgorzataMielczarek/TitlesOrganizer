using TitlesOrganizer.Domain.Models.Abstract;

namespace TitlesOrganizer.Domain.Models
{
    public class Work : WorkModel
    {
        public int? Year { get; set; }
        public int? SeriesId { get; set; }
        public int? NumberInSeries { get; set; }

        public virtual ICollection<Genre> Genres { get; set; } = [];
        public virtual ICollection<Creator> Creators { get; set; } = [];
        public virtual ICollection<Language> Languages { get; set; } = [];
        public virtual Series? Series { get; set; }
    }
}