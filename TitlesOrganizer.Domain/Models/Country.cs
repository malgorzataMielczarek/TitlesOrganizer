using TitlesOrganizer.Domain.Models.Abstract;

namespace TitlesOrganizer.Domain.Models
{
    public class Country : DictionaryModel
    {
        public virtual ICollection<Movie> Movies { get; set; } = [];
        public virtual ICollection<TvSeries> TvSeries { get; set; } = [];
    }
}