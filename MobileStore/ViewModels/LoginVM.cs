using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace MobileStore.ViewModels
{
    public class LoginVM
    {
        [Display(Name = "ایمیل")]
        [Required(ErrorMessage = "فیلد {0} اجباری است")]
        [DataType(DataType.EmailAddress, ErrorMessage = "لطقا ایمیل صحیح وارد کنید")]
        public string Email { get; set; } = string.Empty;

        [Display(Name = "کلمه عبور")]
        [Required(ErrorMessage = "فیلد {0} اجباری است")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        public bool IsRememberMe { get; set; }
    }
}
