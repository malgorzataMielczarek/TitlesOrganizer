// Ignore Spelling: Validator

namespace TitlesOrganizer.Application.ViewModels.BookVMs
{
    public class BookVM
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public string? OriginalTitle { get; set; }
        public string? OriginalLanguageCode { get; set; }
        public int? Year { get; set; }
        public string? Edition { get; set; }
        public string? Description { get; set; }
    }
}