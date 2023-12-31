﻿using TitlesOrganizer.Domain.Models.Abstract;

namespace TitlesOrganizer.Domain.Models
{
    public class Season : IBaseModel
    {
        public int Id { get; set; }
        public int No { get; set; }
        public string? Title { get; set; }
        public string? OriginalTitle { get; set; }
        public int? ExpectedLength { get; set; }
        public string? Description { get; set; }
        public int SeriesId { get; set; }

        public virtual ICollection<Episode> Episodes { get; set; } = new List<Episode>();
        public TvSeries Series { get; set; } = null!;
    }
}