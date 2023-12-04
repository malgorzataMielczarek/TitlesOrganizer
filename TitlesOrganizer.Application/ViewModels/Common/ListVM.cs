using System.ComponentModel.DataAnnotations;
using TitlesOrganizer.Application.ViewModels.Abstract;
using TitlesOrganizer.Application.ViewModels.Helpers;

namespace TitlesOrganizer.Application.ViewModels.Common
{
    public class ListVM : IListVM
    {
        public ListVM(List<IForListVM> values, Paging paging, Filtering filtering)
        {
            Filtering = filtering;
            Paging = paging;
            Values = values;
        }

        public ListVM()
        {
            Filtering = new();
            Paging = new();
            Values = new();
        }

        [ScaffoldColumn(false)]
        public Filtering Filtering { get; set; }

        [ScaffoldColumn(false)]
        public Paging Paging { get; set; }

        public virtual List<IForListVM> Values { get; set; }
    }
}