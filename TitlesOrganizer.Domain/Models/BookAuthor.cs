namespace TitlesOrganizer.Domain.Models
{
    public class BookAuthor
    {
        public int BookId { get; set; }
        public Book Book { get; set; }
        public int AuthorId { get; set; }
        public Creator Author { get; set; }
    }
}