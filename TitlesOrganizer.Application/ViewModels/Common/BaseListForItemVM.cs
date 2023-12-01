using System.ComponentModel.DataAnnotations;
using TitlesOrganizer.Application.ViewModels.Abstract;
using TitlesOrganizer.Application.ViewModels.Helpers;

namespace TitlesOrganizer.Application.ViewModels.Common
{
    public class BaseListForItemVM : IListForItemVM
    {
        public BaseListForItemVM()
        {
            Filtering = new();
            Item = new BaseForListVM();
            Paging = new();
            Values = new();
        }

        public BaseListForItemVM(List<IForItemVM> values, IForListVM item, Paging paging, Filtering filtering)
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