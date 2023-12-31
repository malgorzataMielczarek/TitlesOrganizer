﻿using TitlesOrganizer.Domain.Models.Abstract;

namespace TitlesOrganizer.Domain.Models
{
    public class TvSeries : IBaseModel
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public string? OriginalTitle { get; set; }
        public string? Description { get; set; }
        public int? ExpectedLength { get; set; }

        public ICollection<Season> Seasons { get; set; } = new List<Season>();
        public ICollection<VideoGenre> Genres { get; set; } = new List<VideoGenre>();
        public virtual ICollection<Director> Directors { get; set; } = new List<Director>();
        public virtual ICollection<Country> Countries { get; set; } = new List<Country>();
    }
}