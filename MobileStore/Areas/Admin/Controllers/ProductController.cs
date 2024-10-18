using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MobileStore.Models;
using MobileStore.Services;
using MobileStore.ViewModels;
using Azure;

namespace MobileStore.Areas.Admin.Controllers
{
    [Area("admin")]
    [Route("admin")]
    [Authorize(Roles = "admin")]
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment env;

        public ProductController(ApplicationDbContext context, IWebHostEnvironment env)
        {
            this._context = context;
            this.env = env;
        }

        [Route("products")]
        public IActionResult GetAllProduct(int? page)
        {
            if (page == null || page < 1)
            {
                page = 1;
            }

            int pageSize = 10;
            int totalPages = 0;
            decimal count = _context.Products.Count();
            totalPages = (int)Math.Ceiling(count / pageSize);

            var products = _context.Products
                .Include(h=>h.Brand)
                .OrderByDescending(b => b.Id)
                .Skip((int)(page! - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            ViewBag.totalPages = totalPages;
            ViewBag.page = page;

            return View(products);
        }

        [Route("admin/product/{id}")]
        public IActionResult GetProduct(int id)
        {
            var product = _context.Products.SingleOrDefault(b => b.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

       
        [Route("product/create")]
        public IActionResult CreateProduct()
        {
            var brands = _context.Brands.ToList();
            SelectList listItems = new SelectList(brands, "Id", "Name", 0);
            ViewBag.SelectedList = listItems;
            return View();
        }

        [HttpPost]
        [Route("product/create")]
        [ValidateAntiForgeryToken]
        public IActionResult CreateProduct(ProductVM productVM)
        {
            if(!ModelState.IsValid)
            {
                var brands = _context.Brands.ToList();
                SelectList listItems = new SelectList(brands, "Id", "Name", 0);
                ViewBag.SelectedList = listItems;

                return View(productVM);
            }

            Product product = new Product()
            {
                Title = productVM.Title,
                BrandId = productVM.BrandId,
                Description = productVM.Description,
                Ram = productVM.Ram,
                Hard = productVM.Hard,
                Camera = productVM.Camera,
                CreatedAt = DateTime.Now
            };

            _context.Products.Add(product);
            _context.SaveChanges();
            return RedirectToAction("GetAllProduct");
        }


        
        [Route("product/edit/{id}")]
        public IActionResult UpdateProduct(int id)
        {
            var product = _context.Products.SingleOrDefault(a => a.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            var brands = _context.Brands.ToList();
            SelectList listItems = new SelectList(brands, "Id", "Name", 0);
            ViewBag.SelectedList = listItems;

            ProductVM productVM= new ProductVM()
            {
                Id = product.Id,
                BrandId = product.BrandId,
                Title = product.Title,
                Ram = product.Ram,
                Hard = product.Hard,
                Camera = product.Camera,
                Description = product.Description
            };
            return View(productVM);
        }

        [HttpPost]
        [Route("product/edit/{id}")]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateProduct(int id, ProductVM productVM)
        {
            if (!ModelState.IsValid)
            {
                var brands = _context.Brands.ToList();
                SelectList listItems = new SelectList(brands, "Id", "Name", 0);
                ViewBag.SelectedList = listItems;

                return View(productVM);
            }
            var product = _context.Products.Find(id);
            if (product == null)
            {
                return NotFound();
            }


            product.Title = productVM.Title;
            product.BrandId = productVM.BrandId;
            product.Ram = productVM.Ram;
            product.Hard = productVM.Hard;
            product.Camera = productVM.Camera;
            product.Description = productVM.Description;

            _context.Products.Update(product);
            _context.SaveChanges();
            return RedirectToAction("GetAllProduct");
        }

        
        [Route("product/delete/{id}")]
        public IActionResult DeleteProduct(int id)
        {
            var product = _context.Products.Find(id);
            if (product == null)
            {
                return NotFound();
            }

            _context.Products.Remove(product);
            _context.SaveChanges();

            return RedirectToAction("GetAllProduct");
        }

        [Route("product/price/{id}")]
        public IActionResult ListPriceProduct(int id) 
        {
            var product = _context.Products.Find(id);
            if (product == null)
            {
                return NotFound();
            }

            var lists = _context.ProductToShops.Include(x => x.Shop).Where(x => x.ProductId == id).ToList();
            return View(lists);
        }
    }
}
