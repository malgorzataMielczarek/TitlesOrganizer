using TitlesOrganizer.Domain.Models.Abstract;

namespace TitlesOrganizer.Domain.Models
{
    public class Book : BaseTitleModel
    {
        public int? Year { get; set; }
        public string? Edition { get; set; }
        public int? SeriesId { get; set; }
        public int? NumberInSeries { get; set; }

        public virtual ICollection<Creator> Creators { get; set; } = new List<Creator>();
        public virtual ICollection<LiteratureGenre> Genres { get; set; } = new List<LiteratureGenre>();
        public BookSeries? Series { get; set; }
    }
}