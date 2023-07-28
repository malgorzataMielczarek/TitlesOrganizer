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
        public Series? Series { get; set; }

        public virtual ICollection<MovieGenre>? MovieGenres { get; set; }
        public virtual ICollection<MovieDirector>? MovieDirectors { get; set; }
        public virtual ICollection<MovieCountry>? MovieCountries { get; set; }
    }
}