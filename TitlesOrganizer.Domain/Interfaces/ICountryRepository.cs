using TitlesOrganizer.Domain.Models;

namespace TitlesOrganizer.Domain.Interfaces
{
    public interface ICountryRepository
    {
        IQueryable<Country> GetAllCountries();
    }
}