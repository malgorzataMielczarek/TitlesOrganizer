namespace TitlesOrganizer.Domain.Models.Abstract
{
    public abstract class WorkModel : BaseModel
    {
        //public required string Title { get; set; }
        //public string? OriginalTitle { get; set; }
        //public string? OriginalLanguageCode { get; set; }

        public string? Description { get; set; }

        //public virtual Language? OriginalLanguage { get; set; }
        public virtual ICollection<WorkTitle> Titles { get; set; } = [];
    }
}