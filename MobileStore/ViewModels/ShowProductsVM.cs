using System.ComponentModel.DataAnnotations.Schema;
using MobileStore.Models;

namespace MobileStore.ViewModels
{
    public class ShowProductsVM
    {
        public int Id { get; set; }
        public Brand Brand { get; set; } = null!;
        public string Title { get; set; } = string.Empty;
        public string ImageFileName { get; set; } = "";
        public int Price { get; set; }
    }
}
