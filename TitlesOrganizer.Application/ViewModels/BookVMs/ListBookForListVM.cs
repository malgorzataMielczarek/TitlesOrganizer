namespace TitlesOrganizer.Application.ViewModels.BookVMs
{
    public class ListBookForListVM
    {
        public List<BookForListVM> Books { get; set; } = new List<BookForListVM>();

        public int Count { get; set; }
    }
}