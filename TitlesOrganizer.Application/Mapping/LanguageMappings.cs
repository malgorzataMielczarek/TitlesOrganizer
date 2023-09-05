using TitlesOrganizer.Application.ViewModels.LanguageVMs;
using TitlesOrganizer.Domain.Models;

namespace TitlesOrganizer.Application.Mapping
{
    public class LanguageMappings
    {
        public static Func<Language, LanguageForListVM> ToLanguageForListVM = language => new LanguageForListVM
        {
            Code = language.Code,
            Name = language.Name
        };
    }
}