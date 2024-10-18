using System.ComponentModel.DataAnnotations;

namespace MobileStore.ViewModels
{
    public class CommentVM
    {
        public int? Id { get; set; }
        public int ProductId { get; set; }

        [Display(Name = "متن نظر")]
        [Required(ErrorMessage = "وارد کردن {0} الزامی می باشد.")]
        [MinLength(2, ErrorMessage = "حداقل مقدار {0} {1} حرف می باشد.")]
        [MaxLength(200, ErrorMessage = "حداکثر مقدار {0} {1} حرف می باشد.")]
        public string Content { get; set; } = "";
    }
}
