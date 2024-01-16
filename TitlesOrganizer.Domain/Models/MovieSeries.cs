using TitlesOrganizer.Domain.Models.Abstract;

namespace TitlesOrganizer.Domain.Models
{
    public class MovieSeries : BaseTitleModel
    {
        public ICollection<Movie> Movies { get; set; } = new List<Movie>();
    }
}