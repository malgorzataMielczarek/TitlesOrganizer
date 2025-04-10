using TitlesOrganizer.Domain.Models.Abstract;

namespace TitlesOrganizer.Domain.Models
{
    public class Series : WorkModel
    {
        public virtual ICollection<Work> Works { get; set; } = [];
    }
}