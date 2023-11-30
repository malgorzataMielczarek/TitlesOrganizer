using TitlesOrganizer.Application.ViewModels.Abstract;
using TitlesOrganizer.Application.ViewModels.Helpers;

namespace TitlesOrganizer.Application.ViewModels.Base
{
    public class BaseDoubleListForItemVM : BaseListForItemVM, IDoubleListForItemVM
    {
        public BaseDoubleListForItemVM() : base()
        {
            SelectedValues = new();
        }

        public BaseDoubleListForItemVM(List<IForItemVM> values, List<IForItemVM> selectedValues, IForListVM item, Paging paging, Filtering filtering)
            : base(values, item, paging, filtering)
        {
            SelectedValues = selectedValues;
        }

        public virtual List<IForItemVM> SelectedValues { get; set; }
    }
}