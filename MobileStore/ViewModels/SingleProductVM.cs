using MobileStore.Models;

namespace MobileStore.ViewModels
{
    public class SingleProductVM
    {
        public Product Product { get; set; } = null!;
        public CommentVM Comment { get; set; } = null!;
    }
}
