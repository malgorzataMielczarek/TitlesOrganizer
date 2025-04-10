using TitlesOrganizer.Domain.Models.Abstract;
using TitlesOrganizer.Domain.Models.Enums;

namespace TitlesOrganizer.Domain.Models
{
    public class Genre : BaseModel
    {
        public required string Name { get; set; }
        public required MediaType ForMedia { get; set; }

        public virtual ICollection<Work>? Works { get; set; }
    }
}