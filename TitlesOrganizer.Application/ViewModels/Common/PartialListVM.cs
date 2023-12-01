using System.ComponentModel.DataAnnotations;
using TitlesOrganizer.Application.ViewModels.Abstract;
using TitlesOrganizer.Application.ViewModels.Helpers;

namespace TitlesOrganizer.Application.ViewModels.Common
{
    public class PartialListVM : IPartialListVM
    {
        public PartialListVM()
        {
            Values = new List<IForListVM>();
            Paging = new Paging();
        }

        public PartialListVM(int pageSize)
        {
            Values = new List<IForListVM>();
            Paging = new Paging(pageSize);
        }

        [ScaffoldColumn(false)]
        public Paging Paging { get; set; }

        public virtual List<IForListVM> Values { get; set; }
    }
}