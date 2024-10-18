using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using MobileStore.Models;
using MobileStore.Services;
using MobileStore.ViewModels;
using System;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Drawing.Drawing2D;
using Microsoft.AspNetCore.OutputCaching;
using System.Globalization;
using System.Drawing.Printing;

namespace MobileStore.Controllers
{
    public class HomeController : Controller
    {

        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            this._context = context;
                      
        }

        [Route("/")]
        public IActionResult Index()
        {
            var brands = _context.Brands.ToList();
            List<HomeVM> brandsProduct = _context.Brands
                .Include(p=> p.Products)
                .ThenInclude(g => g.Galleries.Take(1))
                .Include(p => p.Products)
                .ThenInclude(g => g.ProductToShops.Take(1))
                .Where(x => x.Products.Count() > 0)
                .Select(x => new HomeVM
                {
                    Id = x.Id,
                    Name = x.Name,
                    Slug = x.Slug,
                    ImageFileName = x.ImageFileName,
                    Products = x.Products.Where(x => x.ProductToShops.Count() != 0).OrderByDescending(x => x.Id).Take(3).Select(x => new HomeVMProduct
                    {
                        Id = x.Id,
                        Title = x.Title,
                        Price = x.ProductToShops.OrderBy(x => x.Price).Count() != 0 ? x.ProductToShops.OrderBy(x => x.Price).First().Price : 0,
                        ImageFileName = x.Galleries.Count() != 0 ? x.Galleries.First().ImageFileName : null,
                    }).ToList()
                }).ToList();

            ViewBag.Brands = brands;

            return View(brandsProduct);
        }

        [Route("brand/{slug}")]
        public IActionResult ShowProductsBrand(string slug, int? page)
        {
            var brand = _context.Brands.SingleOrDefault(x => x.Slug == slug);
            if(brand == null)
            {
                return NotFound();
            }

            if(page == null || page < 1)
            {
                page = 1;
            }

            int pageSize = 4;
            int totalPages = 0;
            decimal count = _context.Products.Where(x => x.BrandId == brand.Id).Count();
            totalPages = (int)Math.Ceiling(count / pageSize);

            List<ShowProductsVM> products = _context.Products
                .Where(x => x.BrandId == brand.Id)
                .Include(h => h.Brand)
                .Include(h => h.Galleries.Take(1))
                .Include(p => p.ProductToShops.OrderBy(x => x.Price).Take(1))
                .ThenInclude(p => p.Shop)
                .Select(x => new ShowProductsVM { Id = x.Id, Title = x.Title, Brand = x.Brand, ImageFileName = x.Galleries.First().ImageFileName, Price = x.ProductToShops.Count() != 0 ? x.ProductToShops.First().Price : 0 })
                .OrderByDescending(b => b.Id)
                .Skip((int)(page! - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            ViewBag.Products = products;
            ViewBag.totalPages = totalPages;
            ViewBag.page = page;

            ViewData["brand"] = brand.Name;
            return View(products);
        }

        [ResponseCache(Duration = 60)]
        [Route("products")]
        public IActionResult ShowProducts(int? page)
        {
            if (page == null || page < 1)
            {
                page = 1;
            }

            int pageSize = 7;
            int totalPages = 0;
            decimal count = _context.Products.Count();
            totalPages = (int)Math.Ceiling(count / pageSize);

            List<ShowProductsVM> products = _context.Products
                .Include(h => h.Brand)
                .Include(h => h.Galleries.Take(1))
                .Include(p => p.ProductToShops.OrderBy(p => p.Price).Take(1))
                .ThenInclude(p => p.Shop)
                .Select(x => new ShowProductsVM { Id = x.Id, Title = x.Title, Brand = x.Brand, ImageFileName = x.Galleries.First().ImageFileName, Price = x.ProductToShops.Count() != 0 ? x.ProductToShops.First().Price : 0 })
                .OrderByDescending(b => b.Id)
                .Skip((int)(page! - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            

            ViewBag.totalPages = totalPages;
            ViewBag.page = page;

            return View(products);
        }

        [Route("product/{id}")]
        public IActionResult SingleProduct(int id)
        {
            var product = _context.Products
                .Include(p => p.Brand)
                .Include(c => c.Comments.OrderByDescending(b => b.Id))
                .ThenInclude(p => p.User)
                .Include(h => h.Galleries)
                .Include(p => p.ProductToShops)
                .ThenInclude(p => p.Shop)
                .FirstOrDefault(p => p.Id == id);
            if(product == null)
            {
                return NotFound();
            }

            SingleProductVM singleProductVM = new SingleProductVM
            {
                Product = product
            };

            return View(singleProductVM);
        }

        [HttpPost]
        [Route("product/{id}")]
        [Authorize]
        [ValidateAntiForgeryToken]
        public IActionResult SingleProduct(int id, CommentVM commentVM)
        {
            

            var product = _context.Products.SingleOrDefault(x => x.Id == id);
            if( product == null )
            {
                return NotFound();
            }

            Comment comment = new Comment()
            {
                UserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!),
                ProductId = id,
                Content = commentVM.Content,
                CreatedAt = DateTime.Now
            };

            _context.Comments.Add(comment);
            _context.SaveChanges();

            TempData["success"] = "نظر شما با موفقیت ثبت شد بعد از تایید ادمین عمومی میشود";

            
            return RedirectToAction("SingleProduct", new { id = id });

        }

        [Route("comment/delete/{id}")]
        public IActionResult DeleteComment(int id)
        {
            var comment = _context.Comments.Find(id);
            if (comment == null)
            {
                return NotFound();
            }

            if(comment.UserId != int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!))
            {
                return NotFound();
            }

            _context.Comments.Remove(comment);
            _context.SaveChanges();

            TempData["info"] = "نظر شما با موفقیت حذف شد";

            return RedirectToAction("SingleProduct", new { id = comment.ProductId });
        }

        [Route("Error/404")]
        public IActionResult Error404()
        {
            return View("NotFound");
        }
    }
}