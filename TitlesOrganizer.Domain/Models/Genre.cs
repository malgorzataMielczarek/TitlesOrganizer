namespace TitlesOrganizer.Domain.Models
{
    public class Genre
    {
        public int Id { get; set; }
        public required string Name { get; set; }

        public virtual ICollection<MovieGenre>? GenreMovies { get; set; }
        public virtual ICollection<BookGenre>? GenreBooks { get; set; }
    }
}