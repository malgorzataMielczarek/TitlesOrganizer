namespace TitlesOrganizer.Application.ViewModels.Helpers
{
    public class PartialList<T> where T : class
    {
        public List<T> Values { get; set; } = new List<T>();

        public Paging Paging { get; set; } = new Paging();
    }
}