using TitlesOrganizer.Application.ViewModels.LanguageVMs;

namespace TitlesOrganizer.Application.Interfaces
{
    public interface ILanguageService
    {
        ListLanguageForListVM GetAllLanguagesForList();
    }
}