using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using MobileStore.Services;

namespace MobileStore.Areas.Admin.Controllers
{
    [Area("admin")]
    [Route("admin")]
    [Authorize(Roles = "admin")]
    public class ManageShopController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ManageShopController(ApplicationDbContext context)
        {
            this._context = context;
        }

        [Route("shops")]
        public IActionResult GetAllShop()
        {

            var shops = _context.Shops.Include(h => h.User).OrderByDescending(b => b.Id).ToList();
            return View(shops);
        }

        [Route("shops/{id}")]
        public IActionResult GetAllProductShop(int id)
        {
            var shop = _context.Shops.Find(id);
            if(shop == null)
            {
                return NotFound();
            }

            ViewBag.shop = shop;

            var items = _context.ProductToShops.Where(x => x.ShopId==id).Select(x => x.ProductId ).ToList();
            var products = _context.Products.Where(x => items.Contains(x.Id)).Include(x => x.Brand).Include(x => x.ProductToShops).ToList();
            return View(products);
        }
    }
}
