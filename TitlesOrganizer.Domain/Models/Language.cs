using TitlesOrganizer.Domain.Models.Abstract;
using TitlesOrganizer.Domain.Models.Enums;

namespace TitlesOrganizer.Domain.Models
{
    public class Language : DictionaryModel
    {
        public LanguageSignificance Level { get; set; }
        public virtual ICollection<Work> Works { get; set; } = [];
    }
}