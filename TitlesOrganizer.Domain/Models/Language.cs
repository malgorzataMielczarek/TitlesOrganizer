using TitlesOrganizer.Domain.Models.Abstract;

namespace TitlesOrganizer.Domain.Models
{
    public class Language : DictionaryModel
    {
        public virtual ICollection<Book> Books { get; set; } = [];
        public virtual ICollection<Episode> Episodes { get; set; } = [];
        public virtual ICollection<Movie> Movies { get; set; } = [];
        public virtual ICollection<Season> Seasons { get; set; } = [];
        public virtual ICollection<Series> Series { get; set; } = [];
        public virtual ICollection<TvSeries> TvSeries { get; set; } = [];
    }
}