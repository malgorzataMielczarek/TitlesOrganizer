using TitlesOrganizer.Domain.Models.Abstract;

namespace TitlesOrganizer.Domain.Models
{
    public class TvSeries : WorkModel
    {
        public int? ExpectedLength { get; set; }

        public virtual ICollection<Season> Seasons { get; set; } = [];
        public virtual ICollection<Genre> Genres { get; set; } = [];
        public virtual ICollection<Creator> Creators { get; set; } = [];
        public virtual ICollection<Country> Countries { get; set; } = [];
    }
}