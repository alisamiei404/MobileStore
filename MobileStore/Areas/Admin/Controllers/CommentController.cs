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
    public class CommentController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CommentController(ApplicationDbContext context)
        {
            this._context = context;
        }

        [Route("comments")]
        public IActionResult GetAllComment()
        {

            var comments = _context.Comments.Include( u => u.User).OrderByDescending(b => b.Id).ToList();
            return View(comments);
        }


        [Route("comment/edit/{id}")]
        public IActionResult UpdateComment(int id)
        {
            var comment = _context.Comments.Find(id);
            if(comment == null)
            {
                return NotFound();
            }

            comment.IsActive = !comment.IsActive;

            _context.Comments.Update(comment);
            _context.SaveChanges();

            return RedirectToAction("GetAllComment");

        }

        [Route("comment/delete/{id}")]
        public IActionResult DeleteComment(int id)
        {
            var comment= _context.Comments.Find(id);
            if (comment == null)
            {
                return NotFound();
            }

            _context.Comments.Remove(comment);
            _context.SaveChanges();

            return RedirectToAction("GetAllComment");
        }
    }
}
