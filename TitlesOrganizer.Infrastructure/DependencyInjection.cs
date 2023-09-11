using Microsoft.Extensions.DependencyInjection;
using TitlesOrganizer.Domain.Interfaces;
using TitlesOrganizer.Infrastructure.Repositories;

namespace TitlesOrganizer.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddTransient<IBookRepository, BookRepository>()
                .AddTransient<ICountryRepository, CountryRepository>()
                .AddTransient<ILanguageRepository, LanguageRepository>()
                .AddTransient<IMovieRepository, MovieRepository>()
                .AddTransient<ITvSeriesRepository, TvSeriesRepository>();

            return services;
        }
    }
}