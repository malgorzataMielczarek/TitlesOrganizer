namespace TitlesOrganizer.Application.ViewModels.Helpers
{
    public class Paging
    {
        public int Count { get; set; }
        public int PageSize { get; set; } = 5;
        public int CurrentPage { get; set; }

        public Paging()
        {
        }

        public Paging(int pageSize)
        {
            Count = 0;
            PageSize = pageSize;
            CurrentPage = 1;
        }
    }
}