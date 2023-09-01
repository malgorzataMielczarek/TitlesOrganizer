namespace TitlesOrganizer.Application.ViewModels.LanguageVMs
{
    public class ListLanguageForListVM
    {
        public List<LanguageForListVM> Languages { get; set; } = new List<LanguageForListVM>();
        public int Count { get; set; }
    }
}