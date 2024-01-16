using TitlesOrganizer.Domain.Models.Abstract;

namespace TitlesOrganizer.Domain.Models
{
    public class Episode : BaseTitleModel
    {
        public int Number { get; set; }
        public int? AbsoluteNo { get; set; }
        public int SeasonId { get; set; }

        public Season Season { get; set; } = null!;
    }
}