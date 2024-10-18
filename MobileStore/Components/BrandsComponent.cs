using Microsoft.AspNetCore.Mvc;
using MobileStore.Services;

namespace MobileStore.Components
{
    public class BrandsComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;

        public BrandsComponent(ApplicationDbContext context)
        {
            this._context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var brands = _context.Brands.ToList();

            return await Task.FromResult(View("/Views/Components/_BrandsComponent.cshtml", brands));
        }
    }


}
