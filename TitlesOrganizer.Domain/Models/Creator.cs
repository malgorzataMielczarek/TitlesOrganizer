using TitlesOrganizer.Domain.Models.Abstract;
using TitlesOrganizer.Domain.Models.Enums;

namespace TitlesOrganizer.Domain.Models
{
    public class Creator : BaseModel
    {
        public string? Name { get; set; }
        public string? LastName { get; set; }
        public Profession Profession { get; set; }

        public virtual ICollection<Work> Works { get; set; } = [];
    }
}