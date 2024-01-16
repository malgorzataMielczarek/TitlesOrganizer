using TitlesOrganizer.Domain.Models.Abstract;

namespace TitlesOrganizer.Domain.Models
{
    public class TvSeries : BaseTitleModel
    {
        public int? ExpectedLength { get; set; }

        public ICollection<Season> Seasons { get; set; } = new List<Season>();
        public ICollection<VideoGenre> Genres { get; set; } = new List<VideoGenre>();
        public virtual ICollection<Creator> Creators { get; set; } = new List<Creator>();
        public virtual ICollection<Country> Countries { get; set; } = new List<Country>();
    }
}