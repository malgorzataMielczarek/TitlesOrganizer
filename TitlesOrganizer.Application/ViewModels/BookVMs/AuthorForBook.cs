namespace TitlesOrganizer.Application.ViewModels.BookVMs
{
    public class AuthorForBook
    {
        public int Id { get; set; }
        public bool IsForBook { get; set; }
        public string FullName { get; set; } = null!;
        public string OtherBooks { get; set; } = string.Empty;
    }
}