using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using TitlesOrganizer.Application.Interfaces;
using TitlesOrganizer.Application.Services;

namespace TitlesOrganizer.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddTransient<IBookService, BookService>()
                .AddTransient<ICountryService, CountryService>()
                .AddTransient<ILanguageService, LanguageService>()
                .AddTransient<IMovieService, MovieService>()
                .AddTransient<ITvSeriesService, TvSeriesService>()
                .AddTransient<IVideoService, VideoService>();
            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            return services;
        }
    }
}