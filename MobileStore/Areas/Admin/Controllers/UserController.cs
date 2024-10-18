using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using MobileStore.Services;

namespace MobileStore.Areas.Admin.Controllers
{
    [Area("admin")]
    [Route("admin")]
    [Authorize(Roles = "admin")]
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UserController(ApplicationDbContext context)
        {
            this._context = context;
        }

        [Route("users")]
        public IActionResult GetAllUser()
        {

            var users = _context.Users.OrderByDescending(b => b.Id).ToList();
            return View(users);
        }
    }
}
