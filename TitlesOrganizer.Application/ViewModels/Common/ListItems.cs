namespace TitlesOrganizer.Application.ViewModels.Common
{
    public abstract class ListItems<T> where T : class
    {
        public List<T> List { get; set; }
        public int Count { get; set; }

        public SortByEnum SortBy { get; set; }
        public int PageSize { get; set; }
        public int CurrentPage { get; set; }
        public string SearchString { get; set; }

        public ListItems()
        {
            List = new List<T>();
            SearchString = string.Empty;
        }

        public ListItems(IQueryable<T> list, int count, SortByEnum sortBy, int pageSize, int pageNo, string searchString)
        {
            List = list.ToList();
            Count = count;
            SortBy = sortBy;
            PageSize = pageSize;
            CurrentPage = pageNo;
            SearchString = searchString;
        }
    }
}