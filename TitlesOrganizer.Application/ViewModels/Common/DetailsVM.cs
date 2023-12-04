using System.ComponentModel.DataAnnotations;
using TitlesOrganizer.Application.ViewModels.Abstract;

namespace TitlesOrganizer.Application.ViewModels.Common
{
    public class DetailsVM : IDetailsVM
    {
        [ScaffoldColumn(false)]
        public int Id { get; set; }

        [ScaffoldColumn(false)]
        public virtual string Title { get; set; } = string.Empty;
    }
}