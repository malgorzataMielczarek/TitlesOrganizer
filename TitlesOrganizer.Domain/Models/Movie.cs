namespace TitlesOrganizer.Domain.Models
{
    public class Movie : Work
    {
        public virtual ICollection<Country> Countries { get; set; } = [];
    }
}