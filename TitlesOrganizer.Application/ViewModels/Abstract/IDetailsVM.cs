using System.ComponentModel.DataAnnotations;
using TitlesOrganizer.Domain.Models.Abstract;

namespace TitlesOrganizer.Application.ViewModels.Abstract
{
    public interface IDetailsVM<T> where T : class, IBaseModel
    {
        [ScaffoldColumn(false)]
        int Id { get; set; }

        [ScaffoldColumn(false)]
        string Title { get; set; }
    }
}