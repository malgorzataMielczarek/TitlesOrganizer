namespace TitlesOrganizer.Application.ViewModels.BookVMs
{
    public class GenreDetailsVM
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public ListBookForListVM Books { get; set; } = null!;
    }
}