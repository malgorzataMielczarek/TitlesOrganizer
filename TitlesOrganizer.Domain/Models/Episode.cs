﻿namespace TitlesOrganizer.Domain.Models
{
    public class Episode
    {
        public int Id { get; set; }
        public int No { get; set; }
        public int AbsoluteNo { get; set; }
        public string? Title { get; set; }
        public string? OriginalTitle { get; set; }
        public string? Description { get; set; }
        public int? SeasonId { get; set; }
        public Season? Season { get; set; }
        public int SeriesId { get; set; }
        public Series Series { get; set; }

        public virtual ICollection<EpisodeDirector> EpisodeDirectors { get; set; }
        public virtual ICollection<EpisodeCountry> EpisodeCountries { get; set; }
    }
}