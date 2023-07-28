namespace TitlesOrganizer.Domain.Models
{
    public class EpisodeCountry
    {
        public int EpisodeId { get; set; }
        public Episode Episode { get; set; }
        public int CountryId { get; set; }
        public Country Country { get; set; }
    }
}