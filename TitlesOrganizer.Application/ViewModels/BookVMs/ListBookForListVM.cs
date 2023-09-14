using TitlesOrganizer.Application.ViewModels.Common;

namespace TitlesOrganizer.Application.ViewModels.BookVMs
{
    public class ListBookForListVM
    {
        public List<BookForListVM> Books { get; set; } = new List<BookForListVM>();
        public int Count { get; set; }

        public SortByEnum SortBy { get; set; }
        public int PageSize { get; set; }
        public int PageNo { get; set; }
        public string SearchString { get; set; } = string.Empty;
    }
}