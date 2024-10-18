using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using MobileStore.Services;
using MobileStore.Models;

namespace MobileStore.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        

        private readonly ApplicationDbContext _context;

        public OrderController(ApplicationDbContext context)
        {
           
            this._context = context;


        }

        [Route("/list-cart")]
        public IActionResult ListItemCart()
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var items = _context.Orders.Where(x => x.UserId == userId).Include(h => h.Product.Galleries).Include(h => h.Product.ProductToShops).Include(p => p.Shop).ToList();
            ViewData["gg"] = items.Count();
            return View(items);
        }

        [Route("/add-to-cart")]
        public IActionResult AddToCart(int shopId, int productId)
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var p = _context.ProductToShops.Where(x => x.ShopId == shopId && x.ProductId == productId).FirstOrDefault();
            if(p == null)
            {
                return NotFound();
            }

            var o = _context.Orders.Where(x => x.ShopId == shopId && x.ProductId == productId && x.UserId == userId).FirstOrDefault();
            if(o == null)
            {
                Order order = new Order()
                {
                    UserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!),
                    ProductId = productId,
                    ShopId = shopId,
                    Count = 1,
                    CreatedAt = DateTime.Now
                };

                _context.Orders.Add(order);

                TempData["success"] = "با موفقیت به سبد خرید اضافه شد";
            }
            else
            {
                o.Count = o.Count + 1;
                _context.Orders.Update(o);

                TempData["success"] = "یکی دیگه اضافه شد";
            }
            
            _context.SaveChanges();
            return RedirectToAction("ListItemCart");
        }

        [Route("/remove-to-cart")]
        public IActionResult DeleteToCart(int shopId, int productId)
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            

            var o = _context.Orders.Where(x => x.ShopId == shopId && x.ProductId == productId && x.UserId == userId).FirstOrDefault();
            if (o == null)
            {
                return NotFound();
            }

            TempData["success"] = "با موفقیت از سبد خرید حذف شد";

            _context.Orders.Remove(o);
            _context.SaveChanges();
            return RedirectToAction("ListItemCart");
        }

        [Route("/plus-to-cart")]
        public IActionResult PlusToCart(int shopId, int productId)
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var o = _context.Orders.Where(x => x.ShopId == shopId && x.ProductId == productId && x.UserId == userId).FirstOrDefault();
            if (o == null)
            {
                return NotFound();
            }

            o.Count = o.Count + 1;

            TempData["success"] = "با موفقیت یکی اضافه شد";

            _context.Orders.Update(o);
            _context.SaveChanges();
            return RedirectToAction("ListItemCart");
        }

        [Route("minus-to-cart")]
        public IActionResult MinusToCart(int shopId, int productId)
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var o = _context.Orders.Where(x => x.ShopId == shopId && x.ProductId == productId && x.UserId == userId).FirstOrDefault();
            if (o == null)
            {
                return NotFound();
            }

            if(o.Count == 1)
            {
                _context.Orders.Remove(o);
                TempData["success"] = "با موفقیت حذف شد";
            }
            else
            {
                o.Count = o.Count - 1;
                _context.Orders.Update(o);
                TempData["success"] = "با موفقیت یکی کم شد";
            }
            
            _context.SaveChanges();
            return RedirectToAction("ListItemCart");
        }
    }
}
