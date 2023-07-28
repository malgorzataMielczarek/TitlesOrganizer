namespace TitlesOrganizer.Domain.Models
{
    public class Creator
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? LastName { get; set; }

        public virtual ICollection<BookAuthor>? AuthorBooks { get; set; }
        public virtual ICollection<MovieDirector>? DirectorMovies { get; set; }
        public virtual ICollection<EpisodeDirector>? DirectorEpisodes { get; set; }
    }
}