using TitlesOrganizer.Application.ViewModels.BookVMs.ForList;

namespace TitlesOrganizer.Application.ViewModels.BookVMs.Details
{
    public class GenreDetailsVM
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public ListBookForListVM Books { get; set; } = null!;
    }
}