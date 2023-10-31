using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using TitlesOrganizer.Domain.Models.Abstract;

namespace TitlesOrganizer.Application.ViewModels.Abstract
{
    public interface IForItemVM<T, TItem> where T : class, IBaseModel where TItem : class, IBaseModel
    {
        [DisplayName("")]
        bool IsForItem { get; set; }

        [ScaffoldColumn(false)]
        int Id { get; set; }

        string Description { get; set; }
    }
}