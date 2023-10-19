namespace TitlesOrganizer.Application.ViewModels.Helpers
{
    public struct ListData<T> where T : class
    {
        public IQueryable<T> Values { get; set; }
        public Paging Paging { get; set; }
    }
}