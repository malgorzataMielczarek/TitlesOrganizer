namespace TitlesOrganizer.Domain.Models
{
    public class MovieDirector
    {
        public int MovieId { get; set; }
        public Movie Movie { get; set; }
        public int DirectorId { get; set; }
        public Creator Director { get; set; }
    }
}