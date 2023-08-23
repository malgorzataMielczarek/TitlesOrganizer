namespace TitlesOrganizer.Domain.Models
{
    public class Episode
    {
        public int Id { get; set; }
        public int No { get; set; }
        public int? AbsoluteNo { get; set; }
        public string? Title { get; set; }
        public string? OriginalTitle { get; set; }
        public string? Description { get; set; }
        public int SeasonId { get; set; }

        public virtual ICollection<Director> Directors { get; set; } = new List<Director>();
        public virtual ICollection<Country> Countries { get; set; } = new List<Country>();
        public Season Season { get; set; } = null!;
    }
}