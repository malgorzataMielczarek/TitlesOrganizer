using TitlesOrganizer.Application.ViewModels.Abstract;
using TitlesOrganizer.Application.ViewModels.Helpers;

namespace TitlesOrganizer.Application.ViewModels.Common
{
    public class DoubleListForItemVM : ListForItemVM, IDoubleListForItemVM
    {
        public DoubleListForItemVM() : base()
        {
            SelectedValues = new();
        }

        public DoubleListForItemVM(List<IForItemVM> values, List<IForItemVM> selectedValues, IForListVM item, Paging paging, Filtering filtering)
            : base(values, item, paging, filtering)
        {
            SelectedValues = selectedValues;
        }

        public virtual List<IForItemVM> SelectedValues { get; set; }
    }
}