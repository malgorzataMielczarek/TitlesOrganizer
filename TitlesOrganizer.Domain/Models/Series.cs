namespace TitlesOrganizer.Domain.Models
{
    public class Series
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public int? ExpectedLength { get; set; }

        public ICollection<Episode>? Episodes { get; set; }
        public ICollection<Season>? Seasons { get; set; }
        public ICollection<Movie>? Movies { get; set; }
        public ICollection<Book>? Books { get; set; }
    }
}