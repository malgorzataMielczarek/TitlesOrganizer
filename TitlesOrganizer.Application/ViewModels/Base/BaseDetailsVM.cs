using TitlesOrganizer.Application.ViewModels.Abstract;
using TitlesOrganizer.Domain.Models.Abstract;

namespace TitlesOrganizer.Application.ViewModels.Base
{
    public class BaseDetailsVM<T> : IDetailsVM<T> where T : class, IBaseModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
    }
}