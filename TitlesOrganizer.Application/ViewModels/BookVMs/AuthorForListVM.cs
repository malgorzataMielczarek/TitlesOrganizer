namespace TitlesOrganizer.Application.ViewModels.BookVMs
{
    public class AuthorForListVM
    {
        public int Id { get; set; }
        public string FullName { get; set; } = null!;
        public List<string> Books { get; set; } = new List<string>();
    }
}