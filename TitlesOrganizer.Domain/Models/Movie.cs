namespace TitlesOrganizer.Domain.Models
{
    public class Movie
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? OriginalTitle { get; set; }
        public string? Description { get; set; }
        public int? Year { get; set; }
        public int? SeriesId { get; set; }
        public int? NumberInSeries { get; set; }

        public virtual ICollection<VideoGenre> Genres { get; set; } = new List<VideoGenre>();
        public virtual ICollection<Director> Directors { get; set; } = new List<Director>();
        public virtual ICollection<Country> Countries { get; set; } = new List<Country>();
        public MovieSeries? Series { get; set; }
    }
}