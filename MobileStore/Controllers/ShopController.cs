using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using MobileStore.Models;
using MobileStore.Services;
using MobileStore.ViewModels;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

namespace MobileStore.Controllers
{
    public class ShopController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ShopController(ApplicationDbContext context)
        {
            this._context = context;
        }

        [Route("user/list-product")]
        public IActionResult GetProductNotMyShop()
        {
            

            var user = _context.Users.Find(Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier)));
            if (user == null)
            {
                return NotFound();

            }

            var shop = _context.Shops.FirstOrDefault(u => u.UserId == user.Id);
            if (shop == null)
            {
                return NotFound();
            }

            var productIds = _context.ProductToShops.Where(p => p.ShopId == shop.Id).Select(x => x.ProductId).ToList();


            var products = _context.Products.Include(h => h.Brand).Where(x => !productIds.Contains(x.Id)).OrderByDescending(b => b.Id).ToList();

            return View(products);

            
        }

        [Route("user/myshop")]
        public IActionResult GetProductMyShop()
        {


            var user = _context.Users.Include(h => h.Shop).FirstOrDefault(u => u.Id == Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier)));
            var shop = _context.Shops.FirstOrDefault(u => u.UserId == user.Id);
            if (shop == null)
            {
                return NotFound();
            }

            ViewBag.shopId = shop.Id;

            var productIds = _context.ProductToShops.Where(p => p.ShopId == shop.Id).Select(x => x.ProductId).ToList();
            

            var products = _context.Products.Where(x => productIds.Contains(x.Id)).Include(h => h.Brand).Include(h => h.ProductToShops).OrderByDescending(b => b.Id).ToList();
            
            return View(products);
        }



        [Route("shop/create")]
        public IActionResult CreateShop()
        {
            var shop = _context.Shops.FirstOrDefault( x => x.UserId == Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier)));
            if (shop != null)
            {
                return RedirectToAction("GetProductMyShop");

            }
            return View();
        }

        

        [HttpPost]
        [Route("shop/create")]
        [ValidateAntiForgeryToken]
        public IActionResult CreateShop(ShopVM shopVM)
        {
            var user = _context.Users.Find(Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier)));
            if (user == null)
            {
                return NotFound();

            }
            if (!ModelState.IsValid)
            {
                return View(shopVM);
            }

            var b1 = _context.Shops.FirstOrDefault(b => b.Name == shopVM.Name);
            if (b1 != null)
            {
                ModelState.AddModelError("Name", "این نام وجود دارد");
                return View(shopVM);
            }


            Shop shop = new Shop()
            {
                Name = shopVM.Name,
                UserId = user.Id,
                CreatedAt = DateTime.Now
            };

            user.Role = "seller";

            var claims = new List<Claim>()
                        {
                            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                            new Claim(ClaimTypes.Email, user.Email),
                            new Claim(ClaimTypes.Name, user.Name),
                            new Claim(ClaimTypes.Role, user.Role),
                        };

            var Identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var Principal = new ClaimsPrincipal(Identity);
            var Properties = new AuthenticationProperties()
            {
                IsPersistent = true
            };

            HttpContext.SignInAsync(Principal, Properties);

            _context.Shops.Add(shop);
            _context.Users.Update(user);
            _context.SaveChanges();
            return RedirectToAction("GetProductMyShop");
        }

        [Route("shop/delete")]
        public IActionResult DeleteShop()
        {
            var shop = _context.Shops.FirstOrDefault(x => x.UserId == Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier)));
            if (shop == null || shop.Id == 1)
            {
                return NotFound();
            }

            var user = _context.Users.Find(int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!));
            if (user == null)
            {
                return NotFound();

            }

            user.Role = "client";

            var claims = new List<Claim>()
                        {
                            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                            new Claim(ClaimTypes.Email, user.Email),
                            new Claim(ClaimTypes.Name, user.Name),
                            new Claim(ClaimTypes.Role, user.Role),
                        };

            var Identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var Principal = new ClaimsPrincipal(Identity);
            var Properties = new AuthenticationProperties()
            {
                IsPersistent = true
            };

            HttpContext.SignInAsync(Principal, Properties);

           

            _context.Shops.Remove(shop);
            _context.Users.Update(user);
            _context.SaveChanges();

            return RedirectToAction("Index", "Home");
        }

        [Route("add-product/{id}")]
        public IActionResult AddProductToShop(int id)
        {
            ViewBag.Id = id;

            var shop = _context.Shops.FirstOrDefault(u => u.UserId == Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier)));
            if (shop == null)
            {
                return NotFound();
            }

            var ps = _context.ProductToShops.FirstOrDefault(u => u.ShopId == shop.Id && u.ProductId == id);
            if (ps != null)
            {
                return RedirectToAction("GetProductMyShop");
            }

            return View();
        }

        [HttpPost]
        [Route("add-product/{id}")]
        [ValidateAntiForgeryToken]
        public IActionResult AddProductToShop(int id, int price)
        {
            ViewBag.Id = id;

            if (price == 0)
            {
                return View();
            }

            var product = _context.Products.Find(id);
            if(product == null)
            {
                return NotFound();
            }

            var shop = _context.Shops.FirstOrDefault(u => u.UserId == Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier)));
            if (shop == null)
            {
                return NotFound();
            }

            ProductToShop productToShop = new ProductToShop()
            {
                ProductId = product.Id,
                ShopId = shop.Id,
                Price = price
            };

            _context.ProductToShops.Add(productToShop);
            _context.SaveChanges();

            return RedirectToAction("GetProductMyShop");
        }

        [Route("delete-product/{id}")]
        public IActionResult DeleteProductToShop(int id)
        {
            var product = _context.Products.Find(id);
            if (product == null)
            {
                return NotFound();
            }

            var shop = _context.Shops.FirstOrDefault(u => u.UserId == Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier)));
            if (shop == null)
            {
                return NotFound();
            }

            var ps = _context.ProductToShops.FirstOrDefault(u => u.ShopId == shop.Id && u.ProductId == product.Id);
            if (ps == null)
            {
                return NotFound();
            }

            _context.ProductToShops.Remove(ps!);
            _context.SaveChanges();

            return RedirectToAction("GetProductMyShop");
        }

    }
}
