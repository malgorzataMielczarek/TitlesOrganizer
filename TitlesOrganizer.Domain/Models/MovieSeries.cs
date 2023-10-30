using TitlesOrganizer.Domain.Models.Abstract;

namespace TitlesOrganizer.Domain.Models
{
    public class MovieSeries : IBaseModel
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? OriginalTitle { get; set; }
        public string? Description { get; set; }

        public ICollection<Movie> Movies { get; set; } = new List<Movie>();
    }
}