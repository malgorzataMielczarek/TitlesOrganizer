using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using TitlesOrganizer.Application.ViewModels.Abstract;

namespace TitlesOrganizer.Application.ViewModels.Common
{
    public class ForItemVM : IForItemVM
    {
        public virtual string Description { get; set; } = string.Empty;

        [ScaffoldColumn(false)]
        public int Id { get; set; }

        [DisplayName("")]
        public virtual bool IsForItem { get; set; } = false;
    }
}