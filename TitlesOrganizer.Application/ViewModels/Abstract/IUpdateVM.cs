using System.ComponentModel.DataAnnotations;
using TitlesOrganizer.Domain.Models.Abstract;

namespace TitlesOrganizer.Application.ViewModels.Abstract
{
    public interface IUpdateVM<T> where T : class, IBaseModel
    {
        [ScaffoldColumn(false)]
        int Id { get; set; }
    }
}