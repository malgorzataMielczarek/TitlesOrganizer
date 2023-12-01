using TitlesOrganizer.Application.ViewModels.Concrete.LanguageVMs;

namespace TitlesOrganizer.Application.Interfaces
{
    public interface ILanguageService
    {
        ListLanguageForListVM GetAllLanguagesForList();
    }
}