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
            services.AddTransient<IAuthorService, AuthorService>()
                .AddTransient<IBookCommandsService, BookCommandsService>()
                .AddTransient<IBookSeriesService, BookSeriesService>()
                .AddTransient<ILiteratureGenreService, LiteratureGenreService>()
                .AddTransient<ILanguageService, LanguageService>();
            // Mapping
            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            // Validation
            services.AddTransient<IValidator<AuthorVM>, AuthorVMValidator>()
                .AddTransient<IValidator<BookVM>, BookVMValidator>()
                .AddTransient<IValidator<GenreVM>, GenreVMValidator>()
                .AddTransient<IValidator<SeriesVM>, SeriesVMValidator>();

            return services;
        }
    }
}