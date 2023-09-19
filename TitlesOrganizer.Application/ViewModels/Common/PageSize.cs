namespace TitlesOrganizer.Application.ViewModels.Common
{
    public static class PageSize
    {
        public static string DataTextField = "Value";
        public static string DataValueField = "Key";

        public static Dictionary<int, string> List = new Dictionary<int, string>()
        {
            {5, "5 books on page" },
            {10, "10 books on page" },
            {20, "20 books on page" },
            {50, "50 books on page" },
            {100, "100 books on page" },
            {150, "150 books on page" },
            {200, "200 books on page" },
            {250, "250 books on page" },
            {500, "500 books on page" }
        };
    }
}