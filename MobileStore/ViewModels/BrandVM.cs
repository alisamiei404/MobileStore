using System.ComponentModel.DataAnnotations;
using MobileStore.Utility;

namespace MobileStore.ViewModels
{
    public class BrandVM
    {
        public int? Id { get; set; }

        [Display(Name = "نام برند")]
        [Required(ErrorMessage = "وارد کردن {0} الزامی می باشد.")]
        [MinLength(2, ErrorMessage = "حداقل مقدار {0} {1} حرف می باشد.")]
        [MaxLength(20, ErrorMessage = "حداکثر مقدار {0} {1} حرف می باشد.")]
        public string Name { get; set; } = "";

        [Display(Name = "اسلاگ برند")]
        [Required(ErrorMessage = "وارد کردن {0} الزامی می باشد.")]
        [MinLength(2, ErrorMessage = "حداقل مقدار {0} {1} حرف می باشد.")]
        [MaxLength(20, ErrorMessage = "حداکثر مقدار {0} {1} حرف می باشد.")]
        [RegularExpression("[a-zA-Z]+", ErrorMessage = "از حروف انگلیسی فقط برای اسلاگ برند استفاده کنید")]
        public string Slug { get; set; } = "";

        [Display(Name = "عکس برند")]
        [AllowedExtensions(new string[] {".jpg",".png",".jpeg"})]
        public IFormFile? ImageFile { get; set; }

        public string? ImageFileName { get; set; } = "";


        [Display(Name = "وضعیت نمایش برند")]
        public bool IsActive { get; set; } = true;
    }
}
