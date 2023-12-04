using System.ComponentModel.DataAnnotations;
using TitlesOrganizer.Application.ViewModels.Abstract;
using TitlesOrganizer.Application.ViewModels.Helpers;

namespace TitlesOrganizer.Application.ViewModels.Common
{
    public class ListForItemVM : IListForItemVM
    {
        public ListForItemVM()
        {
            Filtering = new();
            Item = new ForListVM();
            Paging = new();
            Values = new();
        }

        public ListForItemVM(List<IForItemVM> values, IForListVM item, Paging paging, Filtering filtering)
        {
            Filtering = filtering;
            Item = item;
            Paging = paging;
            Values = values;
        }

        [ScaffoldColumn(false)]
        public Filtering Filtering { get; set; }

        [ScaffoldColumn(false)]
        public virtual IForListVM Item { get; set; }

        [ScaffoldColumn(false)]
        public Paging Paging { get; set; }

        public virtual List<IForItemVM> Values { get; set; }
    }
}