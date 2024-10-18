using System.ComponentModel.DataAnnotations;

namespace MobileStore.ViewModels
{
    public class ProductVM
    {
        public int Id { get; set; }

        [Display(Name = "نام برند")]
        public int BrandId { get; set; }

        [Display(Name = "عنوان")]
        [Required(ErrorMessage = "وارد کردن {0} الزامی می باشد.")]
        [MinLength(2, ErrorMessage = "حداقل مقدار {0} {1} حرف می باشد.")]
        [MaxLength(200, ErrorMessage = "حداکثر مقدار {0} {1} حرف می باشد.")]
        public string Title { get; set; } = string.Empty;

        [Display(Name = "مقدار رم")]
        public int Ram { get; set; }

        [Display(Name = "مقدار هارد")]
        public int Hard { get; set; }

        [Display(Name = "مقدار دوربین")]
        public int Camera { get; set; }

        [Display(Name = "توضیحات")]
        public string? Description { get; set; }
        
    }
}
