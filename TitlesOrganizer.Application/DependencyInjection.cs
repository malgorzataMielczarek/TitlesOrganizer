using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using TitlesOrganizer.Application.Interfaces;
using TitlesOrganizer.Application.Services;
using TitlesOrganizer.Application.ViewModels.BookVMs;

namespace TitlesOrganizer.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            // Services
            services.AddTransient<IBookService, BookService>()
                .AddTransient<ICountryService, CountryService>()
                .AddTransient<ILanguageService, LanguageService>()
                .AddTransient<IMovieService, MovieService>()
                .AddTransient<ITvSeriesService, TvSeriesService>();
            // Mapping
            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            // Validation
            services.AddTransient<IValidator<BookVM>, BookValidator>();

            return services;
        }
    }
}