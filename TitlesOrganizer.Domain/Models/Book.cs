namespace TitlesOrganizer.Domain.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? OriginalTitle { get; set; }
        public string? OriginalLanguage { get; set; }
        public int? Year { get; set; }
        public string? Edition { get; set; }
        public string? Description { get; set; }
        public int? SeriesId { get; set; }
        public Series? Series { get; set; }

        public virtual ICollection<BookAuthor>? BookAuthors { get; set; }
        public virtual ICollection<BookGenre>? BookGenres { get; set; }
    }
}