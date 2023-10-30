using TitlesOrganizer.Domain.Models.Abstract;

namespace TitlesOrganizer.Domain.Models
{
    public class Director : IBaseModel
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? LastName { get; set; }

        public virtual ICollection<Movie>? Movies { get; set; }
        public virtual ICollection<TvSeries>? TvSeries { get; set; }
    }
}