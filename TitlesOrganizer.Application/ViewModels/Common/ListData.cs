namespace TitlesOrganizer.Application.ViewModels.Common
{
    public struct ListData<T> where T : class
    {
        public IQueryable<T> Values { get; set; }
        public Paging Paging { get; set; }
        public Filtering Filtering { get; set; }
    }
}