using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;
using MobileStore.Models;
using MobileStore.Services;
using MobileStore.ViewModels;

namespace MobileStore.Areas.Admin.Controllers
{
    [Area("admin")]
    [Route("admin")]
    [Authorize(Roles ="admin")]
    public class BrandController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment env;

        public BrandController(ApplicationDbContext context, IWebHostEnvironment env)
        {
            this._context = context;
            this.env = env;
        }

        [Route("brands")]
        public IActionResult GetAllBrand()
        {
            
            var brands = _context.Brands.OrderByDescending(b => b.Id).ToList();
            return View(brands);
        }

        [Route("brand/{slug}")]
        public IActionResult GetBrand(string slug)
        {
            var brand = _context.Brands.SingleOrDefault(b => b.Slug == slug);
            if (brand == null)
            {
                return NotFound();
            }

            return View(brand);
        }

       
        [HttpGet("brand/create")]
        public IActionResult CreateBrand()
        {
            return View();
        }

        [HttpPost("brand/create")]
        [ValidateAntiForgeryToken]
        public IActionResult CreateBrand(BrandVM brandVM)
        {
            if(!ModelState.IsValid)
            {
                return View(brandVM);
            }
            var b1 = _context.Brands.FirstOrDefault(b => b.Name == brandVM.Name);
            if (b1 != null)
            {
                ModelState.AddModelError("Name", "این نام وجود دارد");
                return View(brandVM);
            }

            var b2 = _context.Brands.FirstOrDefault(b => b.Slug == brandVM.Slug);
            if (b2 != null)
            {
                ModelState.AddModelError("Slug", "این اسلاگ وجود دارد");
                return View(brandVM);
            }

            if (brandVM.ImageFile == null)
            {
                ModelState.AddModelError("ImageFile", "عکس برند الزامی است");
                return View(brandVM);
            }

            string imageFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            imageFileName += Path.GetExtension(brandVM.ImageFile.FileName);

            string imagesFolder = env.WebRootPath + "/images/brands/";

            using (var stream = System.IO.File.Create(imagesFolder + imageFileName))
            {
                brandVM.ImageFile.CopyTo(stream);
            }

            Brand brand = new Brand()
            {
                Name = brandVM.Name,
                Slug = brandVM.Slug,
                ImageFileName = imageFileName,
                IsActive = brandVM.IsActive,
                CreatedAt = DateTime.Now
            };

            _context.Brands.Add(brand);
            _context.SaveChanges();
            return RedirectToAction("GetAllBrand");
        }


        
        [Route("brand/edit/{slug}")]
        public IActionResult UpdateBrand(string slug)
        {
            var brand = _context.Brands.SingleOrDefault(a => a.Slug == slug);
            if (brand == null)
            {
                //return Redirect("/error404");
                return NotFound();

            }

            BrandVM brandVM = new BrandVM()
            {
                Id = brand.Id,
                Name = brand.Name,
                Slug = brand.Slug,
                ImageFileName = brand.ImageFileName,
                IsActive = brand.IsActive
            };
            return View(brandVM);
        }

        [HttpPost("brand/edit/{id}")]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateBrand(int id, BrandVM brandVM)
        {
            if (!ModelState.IsValid)
            {
                return View(brandVM);
            }
            var brand = _context.Brands.Find(id);
            if (brand == null)
            {
                return NotFound();
            }



            var b1 = _context.Brands.FirstOrDefault(b => b.Name == brandVM.Name && b.Id != id);
            if (b1 != null)
            {
                ModelState.AddModelError("Name", "این نام وجود دارد");
                return View(brandVM);
            }

            var b2 = _context.Brands.FirstOrDefault(b => b.Slug == brandVM.Slug && b.Id != id);
            if (b2 != null)
            {
                ModelState.AddModelError("Slug", "این اسلاگ وجود دارد");
                return View(brandVM);
            }

            string imageFileName = brand.ImageFileName;
            if (brandVM.ImageFile != null)
            {
                imageFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                imageFileName += Path.GetExtension(brandVM.ImageFile.FileName);

                string imagesFolder = env.WebRootPath + "/images/brands/";

                using (var stream = System.IO.File.Create(imagesFolder + imageFileName))
                {
                    brandVM.ImageFile.CopyTo(stream);
                }

                System.IO.File.Delete(imagesFolder + brand.ImageFileName);
            }
            

            brand.Name = brandVM.Name;
            brand.Slug = brandVM.Slug;
            brand.ImageFileName = imageFileName;
            brand.IsActive = brandVM.IsActive;

            _context.Brands.Update(brand);
            _context.SaveChanges();
            return RedirectToAction("GetAllBrand");
        }

        
        [Route("brand/delete/{id}")]
        public IActionResult DeleteBrand(int id)
        {
            var brand = _context.Brands.Find(id);
            if (brand == null)
            {
                return NotFound();
            }

            string imagesFolder = env.WebRootPath + "/images/brands/";
            System.IO.File.Delete(imagesFolder + brand.ImageFileName);

            _context.Brands.Remove(brand);
            _context.SaveChanges();

            return RedirectToAction("GetAllBrand");
        }
    }
}
