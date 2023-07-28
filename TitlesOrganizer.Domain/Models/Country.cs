using System.ComponentModel.DataAnnotations;

namespace TitlesOrganizer.Domain.Models
{
    public class Country
    {
        [Key]
        public string Code { get; set; }

        public string Name { get; set; }

        public virtual ICollection<MovieCountry>? CountryMovies { get; set; }
        public virtual ICollection<EpisodeCountry>? CountryEpisodes { get; set; }
    }
}