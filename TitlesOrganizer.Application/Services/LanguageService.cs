using TitlesOrganizer.Application.Interfaces;
using TitlesOrganizer.Application.Mapping;
using TitlesOrganizer.Application.ViewModels.LanguageVMs;
using TitlesOrganizer.Domain.Interfaces;

namespace TitlesOrganizer.Application.Services
{
    public class LanguageService : ILanguageService
    {
        private readonly ILanguageRepository _languageRepository;

        public LanguageService(ILanguageRepository languageRepository)
        {
            _languageRepository = languageRepository;
        }

        public ListLanguageForListVM GetAllLanguagesForList()
        {
            var languages = _languageRepository.GetAllLanguages().OrderBy(l => l.Name).Select(LanguageMappings.ToLanguageForListVM).ToList();

            var list = new ListLanguageForListVM()
            {
                Languages = languages,
                Count = languages.Count
            };

            return list;
        }
    }
}