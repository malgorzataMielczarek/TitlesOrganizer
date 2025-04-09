using TitlesOrganizer.Domain.Models.Abstract;

namespace TitlesOrganizer.Domain.Models
{
    public class Episode : WorkModel
    {
        public int Number { get; set; }
        public int? AbsoluteNumber { get; set; }
        public int SeasonId { get; set; }

        public virtual Season Season { get; set; } = null!;
    }
}