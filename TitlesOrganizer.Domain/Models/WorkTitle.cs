using TitlesOrganizer.Domain.Models.Abstract;
using TitlesOrganizer.Domain.Models.Enums;

namespace TitlesOrganizer.Domain.Models
{
    public class WorkTitle : BaseModel
    {
        public required string Title { get; set; }
        public TitleSignificance Level { get; set; } = 0;
        public int? BookId { get; set; }
        public int? EpisodeId { get; set; }
        public int? MovieId { get; set; }
        public int? SeasonId { get; set; }
        public int? SeriesId { get; set; }
        public int? TvSeriesId { get; set; }

        public virtual Book? Book { get; set; }
        public virtual Episode? Episode { get; set; }
        public virtual Movie? Movie { get; set; }
        public virtual Season? Season { get; set; }
        public virtual Series? Series { get; set; }
        public virtual TvSeries? TvSeries { get; set; }
    }
}