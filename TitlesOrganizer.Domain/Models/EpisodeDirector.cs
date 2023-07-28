namespace TitlesOrganizer.Domain.Models
{
    public class EpisodeDirector
    {
        public int EpisodeId { get; set; }
        public Episode Episode { get; set; }
        public int DirectorId { get; set; }
        public Creator Director { get; set; }
    }
}