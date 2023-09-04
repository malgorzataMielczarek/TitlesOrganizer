namespace TitlesOrganizer.Application.ViewModels.BookVMs
{
    public class NewAuthorVM
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? LastName { get; set; }

        public int BookId { get; set; }
    }
}