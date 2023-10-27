﻿using Microsoft.Extensions.DependencyInjection;
using TitlesOrganizer.Domain.Interfaces;
using TitlesOrganizer.Infrastructure.Repositories;

namespace TitlesOrganizer.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddTransient<IBookCommandsRepository, BookCommandsRepository>()
                .AddTransient<IBookQueriesRepository, BookQueriesRepository>()
                .AddTransient<ILanguageRepository, LanguageRepository>();
            //.AddTransient<IBookRepository, BookRepository>()
            //.AddTransient<ICountryRepository, CountryRepository>()
            //.AddTransient<IMovieRepository, MovieRepository>()
            //.AddTransient<ITvSeriesRepository, TvSeriesRepository>();

            return services;
        }
    }
}