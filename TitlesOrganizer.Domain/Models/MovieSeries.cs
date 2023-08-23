namespace TitlesOrganizer.Domain.Models
{
    public class MovieSeries
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }

        public ICollection<Movie> Movies { get; set; } = new List<Movie>();
    }
}