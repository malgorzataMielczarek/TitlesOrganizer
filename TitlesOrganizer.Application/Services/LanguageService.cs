using TitlesOrganizer.Application.Interfaces;
using TitlesOrganizer.Application.ViewModels.Concrete.LanguageVMs;
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

        public ListLanguageForListVM GetAllLanguagesForList() => _languageRepository.GetAllLanguages().MapToList();
    }
}