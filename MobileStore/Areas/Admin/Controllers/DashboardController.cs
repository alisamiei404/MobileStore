using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace MobileStore.Areas.Admin.Controllers
{
    public class DashboardController : Controller
    {
        [Area("admin")]
        [Route("admin")]
        [Authorize(Roles = "admin")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
