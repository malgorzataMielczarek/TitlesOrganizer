﻿using System.ComponentModel.DataAnnotations;

namespace TitlesOrganizer.Domain.Models.Abstract
{
    public abstract class DictionaryModel
    {
        [Key]
        public required string Code { get; set; }

        public required string Name { get; set; }
    }
}