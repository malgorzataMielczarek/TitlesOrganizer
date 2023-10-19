using TitlesOrganizer.Domain.Models;

namespace TitlesOrganizer.Application.ViewModels.LanguageVMs
{
    public class LanguageForListVM
    {
        public string Code { get; set; } = null!;
        public string Name { get; set; } = null!;
    }

    public class ListLanguageForListVM
    {
        public List<LanguageForListVM> Languages { get; set; } = new List<LanguageForListVM>();
        public int Count { get; set; }
    }

    public static partial class MappingExtensions
    {
        public static IQueryable<LanguageForListVM> Map(this IQueryable<Language> languages)
        {
            return languages.Select(lang => new LanguageForListVM()
            {
                Code = lang.Code,
                Name = lang.Name
            });
        }

        public static ListLanguageForListVM MapToList(this IQueryable<Language> languages)
        {
            return new ListLanguageForListVM()
            {
                Languages = languages.Map().OrderBy(l => l.Name).ToList(),
                Count = languages.Count()
            };
        }
    }
}