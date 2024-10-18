using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace MobileStore.ViewModels
{
    public class RegisterVM
    {
        [Display(Name = "نام")]
        [Required(ErrorMessage = "فیلد {0} اجباری است")]
        public string Name { get; set; }

        [Display(Name = "ایمیل")]
        [Required(ErrorMessage = "فیلد {0} اجباری است")]
        [DataType(DataType.EmailAddress, ErrorMessage = "لطقا ایمیل صحیح وارد کنید")]
        public string Email { get; set; }

        [Display(Name = "کلمه عبور")]
        [Required(ErrorMessage = "فیلد {0} اجباری است")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "تکرار کلمه عبور")]
        [Required(ErrorMessage = "فیلد {0} اجباری است")]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "تکرار کلمه عبور همخوانی ندارد")]
        public string RePassword { get; set; }
    }
}
