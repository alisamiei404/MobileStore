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
    [Authorize(Roles = "admin")]
    public class GalleryController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment env;

        public GalleryController(ApplicationDbContext context, IWebHostEnvironment env)
        {
            this._context = context;
            this.env = env;
        }


       
        [Route("gallery/list/{id}")]
        public IActionResult GetAllGallery(int id)
        {
            var product = _context.Products.SingleOrDefault(a => a.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            var galleries = _context.Galleries.Where(a => a.ProductId ==  id).ToList();
            ViewBag.id = product.Id;
            
            return View(galleries);
        }

        [Route("gallery/create/{id}")]
        public IActionResult CreateGallery(int id)
        {
            var product = _context.Products.SingleOrDefault(a => a.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            ViewBag.id = product.Id;
            return View();
        }

        [HttpPost]
        [Route("gallery/create/{id}")]
        [ValidateAntiForgeryToken]
        public IActionResult CreateGallery(GalleryVM galleryVM)
        {
            if(!ModelState.IsValid)
            {
                ViewBag.id = galleryVM.ProductId;
                return View(galleryVM);
            }

            string imageFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            imageFileName += Path.GetExtension(galleryVM.ImageFile.FileName);

            string imagesFolder = env.WebRootPath + "/images/products/";

            using (var stream = System.IO.File.Create(imagesFolder + imageFileName))
            {
                galleryVM.ImageFile.CopyTo(stream);
            }

            Gallery gallery = new Gallery()
            {
                ProductId = galleryVM.ProductId,
                ImageFileName = imageFileName,
                CreatedAt = DateTime.Now
            };

            _context.Galleries.Add(gallery);
            _context.SaveChanges();
            return RedirectToAction("GetAllGallery", new {id= galleryVM.ProductId });
        }
        
        [Route("gallery/delete/{id}")]
        public IActionResult DeleteGallery(int id)
        {
            var gallery = _context.Galleries.Find(id);
            if (gallery == null)
            {
                return NotFound();
            }

            string imagesFolder = env.WebRootPath + "/images/products/";
            System.IO.File.Delete(imagesFolder + gallery.ImageFileName);

            _context.Galleries.Remove(gallery);
            _context.SaveChanges();

            return RedirectToAction("GetAllGallery",new { id = gallery.ProductId });
        }
    }
}
