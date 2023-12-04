using System.ComponentModel.DataAnnotations;
using TitlesOrganizer.Application.ViewModels.Abstract;

namespace TitlesOrganizer.Application.ViewModels.Common
{
    public class ForListVM : IForListVM
    {
        [ScaffoldColumn(false)]
        public int Id { get; set; }

        public virtual string Description { get; set; } = string.Empty;
    }
}