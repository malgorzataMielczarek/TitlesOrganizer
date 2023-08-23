namespace TitlesOrganizer.Domain.Models
{
    public class VideoGenre
    {
        public int Id { get; set; }
        public required string Name { get; set; }

        public virtual ICollection<Movie>? Movies { get; set; }
        public virtual ICollection<TvSeries>? TvSeries { get; set; }
    }
}