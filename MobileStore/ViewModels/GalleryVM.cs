using System.ComponentModel.DataAnnotations;
using MobileStore.Utility;

namespace MobileStore.ViewModels
{
    public class GalleryVM
    {
        public int? Id { get; set; }
        public int ProductId { get; set; }

        [Display(Name = "عکس برند")]
        [Required(ErrorMessage = "وارد کردن {0} الزامی می باشد.")]
        [MaxFileResolution(100)]
        [AllowedExtensions(new string[] { ".jpg", ".png", ".jpeg" })]
        public IFormFile ImageFile { get; set; }

    }
}
