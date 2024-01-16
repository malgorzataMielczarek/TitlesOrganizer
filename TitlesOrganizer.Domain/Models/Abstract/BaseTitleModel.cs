namespace TitlesOrganizer.Domain.Models.Abstract
{
    public abstract class BaseTitleModel : BaseModel
    {
        public required string Title { get; set; }
        public string? OriginalTitle { get; set; }
        public string? OriginalLanguageCode { get; set; }
        public string? Description { get; set; }
        
        public Language? OriginalLanguage { get; set; }
    }
}