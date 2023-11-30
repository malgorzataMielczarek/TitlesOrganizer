using System.ComponentModel.DataAnnotations;
using TitlesOrganizer.Application.ViewModels.Abstract;
using TitlesOrganizer.Application.ViewModels.Helpers;

namespace TitlesOrganizer.Application.ViewModels.Base
{
    public class PartialList : IPartialList
    {
        public PartialList()
        {
            Values = new List<IForListVM>();
            Paging = new Paging();
        }

        public PartialList(int pageSize)
        {
            Values = new List<IForListVM>();
            Paging = new Paging(pageSize);
        }

        [ScaffoldColumn(false)]
        public Paging Paging { get; set; }

        public virtual List<IForListVM> Values { get; set; }
    }
}