using TitlesOrganizer.Domain.Interfaces;
using TitlesOrganizer.Domain.Models;

namespace TitlesOrganizer.Infrastructure.Repositories
{
    public class LanguageRepository : ILanguageRepository
    {
        private readonly Context _context;

        public LanguageRepository(Context context)
        {
            _context = context;
        }

        public IQueryable<Language> GetAllLanguages() => _context.Languages;
    }
}