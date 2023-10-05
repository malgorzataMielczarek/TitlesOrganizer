// Ignore Spelling: Validator

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TitlesOrganizer.Application.ViewModels.BookVMs
{
    public class BookVM
    {
        [Required]
        [ScaffoldColumn(false)]
        public int Id { get; set; }

        [Required]
        [StringLength(225, MinimumLength = 1)]
        public string Title { get; set; } = null!;

        public string? Authors { get; set; }

        [StringLength(225)]
        [DisplayName("Original title")]
        public string? OriginalTitle { get; set; }

        [LanguageCode]
        [DisplayName("Original language")]
        public string? OriginalLanguageCode { get; set; }

        [YearRange(1)]
        public int? Year { get; set; }

        [StringLength(25)]
        public string? Edition { get; set; }

        public string? Genres { get; set; }

        [StringLength(2000)]
        [DataType(DataType.MultilineText)]
        public string? Description { get; set; }

        public string? Series { get; set; }

        [DisplayName("Volume number")]
        public int? NumberInSeries { get; set; }
    }

    public class LanguageCodeAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            string? code = value as string;
            if (string.IsNullOrEmpty(code))
            {
                return true;
            }
            else
            {
                return code.Length == 3 && code.ToLower().All(c => c >= 'a' && c <= 'z');
            }
        }
    }

    public class YearRangeAttribute : RangeAttribute
    {
        public YearRangeAttribute(int minimum) : base(minimum, DateTime.Now.Year)
        {
        }
    }
}