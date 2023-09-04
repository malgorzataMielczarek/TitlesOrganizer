namespace TitlesOrganizer.Application.ViewModels.BookVMs
{
    public class AuthorDetailsVM
    {
        public int Id { get; set; }
        public string FullName { get; set; } = null!;
        public ListBookForListVM Books { get; set; } = null!;
    }
}