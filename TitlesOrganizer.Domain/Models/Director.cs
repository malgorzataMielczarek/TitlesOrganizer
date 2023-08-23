namespace TitlesOrganizer.Domain.Models
{
    public class Director
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? LastName { get; set; }

        public virtual ICollection<Movie>? Movies { get; set; }
        public virtual ICollection<Episode>? Episodes { get; set; }
    }
}