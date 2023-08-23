﻿using System.ComponentModel.DataAnnotations;

namespace TitlesOrganizer.Domain.Models
{
    public class Country
    {
        [Key]
        public required string Code { get; set; }

        public required string Name { get; set; }

        public virtual ICollection<Movie>? Movies { get; set; }
        public virtual ICollection<Episode>? Episodes { get; set; }
    }
}