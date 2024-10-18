using MobileStore.Models;

namespace MobileStore.ViewModels
{
    public class HomeVM
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Slug { get; set; } = "";
        public string ImageFileName { get; set; } = "";

        public List<HomeVMProduct> Products { get; set; } = new List<HomeVMProduct>();
    }

    
}
