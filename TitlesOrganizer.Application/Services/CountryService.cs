using TitlesOrganizer.Application.Interfaces;
using TitlesOrganizer.Domain.Interfaces;

namespace TitlesOrganizer.Application.Services
{
    public class CountryService : ICountryService
    {
        private readonly ICountryRepository _countryRepository;

        public CountryService(ICountryRepository countryRepository)
        {
            _countryRepository = countryRepository;
        }
    }
}