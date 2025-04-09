using TitlesOrganizer.Domain.Models.Abstract;

namespace TitlesOrganizer.Domain.Models
{
    public class Season : WorkModel
    {
        public int Number { get; set; }
        public int? ExpectedLength { get; set; }
        public int SeriesId { get; set; }

        public virtual ICollection<Episode> Episodes { get; set; } = [];
        public virtual TvSeries Series { get; set; } = null!;
    }
}