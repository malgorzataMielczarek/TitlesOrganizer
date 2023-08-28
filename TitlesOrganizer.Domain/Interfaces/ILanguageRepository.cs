using TitlesOrganizer.Domain.Models;

namespace TitlesOrganizer.Domain.Interfaces
{
    public interface ILanguageRepository
    {
        IQueryable<Language> GetAllLanguages();
    }
}