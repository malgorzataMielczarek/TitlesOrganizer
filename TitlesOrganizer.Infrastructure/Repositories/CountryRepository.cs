using TitlesOrganizer.Domain.Interfaces;
using TitlesOrganizer.Domain.Models;

namespace TitlesOrganizer.Infrastructure.Repositories
{
    public class CountryRepository : ICountryRepository
    {
        private readonly Context _context;

        public CountryRepository(Context context)
        {
            _context = context;
        }

        public IQueryable<Country> GetAllCountries() => _context.Countries;
    }
}