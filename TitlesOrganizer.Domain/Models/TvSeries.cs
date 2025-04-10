namespace TitlesOrganizer.Domain.Models
{
    public class TvSeries : Work
    {
        public int? ExpectedLength { get; set; }

        public virtual ICollection<Season> Seasons { get; set; } = [];
        public virtual ICollection<Country> Countries { get; set; } = [];
    }
}