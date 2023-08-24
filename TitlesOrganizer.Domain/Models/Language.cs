using System.ComponentModel.DataAnnotations;

namespace TitlesOrganizer.Domain.Models
{
    public class Language
    {
        [Key]
        public required string Code { get; set; }

        public required string Name { get; set; }
    }
}