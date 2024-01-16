namespace TitlesOrganizer.Domain.Models.Abstract
{
    public abstract class BaseNameModel : BaseModel
    {
        public required string Name { get; set; }
    }
}