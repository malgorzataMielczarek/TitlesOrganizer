namespace TitlesOrganizer.Domain.Models
{
    public class TvSeries
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public string? Description { get; set; }
        public int? ExpectedLength { get; set; }

        public ICollection<Season> Seasons { get; set; } = new List<Season>();
        public ICollection<VideoGenre> Genres { get; set; } = new List<VideoGenre>();
    }
}