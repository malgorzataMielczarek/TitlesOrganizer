namespace TitlesOrganizer.Domain.Models
{
    public class LiteratureGenre
    {
        public int Id { get; set; }
        public required string Name { get; set; }

        public virtual ICollection<Book>? Books { get; set; }
    }
}