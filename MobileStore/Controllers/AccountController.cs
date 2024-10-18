using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Win32;
using MobileStore.Extention;
using System.Security.Claims;
using MobileStore.Models;
using MobileStore.Services;
using MobileStore.ViewModels;

namespace MobileStore.Controllers
{
    public class AccountController : Controller
    {
        ApplicationDbContext _context;
        public AccountController(ApplicationDbContext context)
        {
            _context = context;
            
        }

        [Route("register")]
        [AllowAnonymous]
        public IActionResult Register()
        {

            return View();
        }

        [Route("register")]
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Register(RegisterVM register)
        {
            if(ModelState.IsValid) 
            {
                if(_context.Users.Any(u=>u.Email == register.Email))
                {
                    ModelState.AddModelError("Email", "تکراریه");
                    return View(register);

                }

                var user = new User
                {
                    Name = register.Name,
                    Email = register.Email,
                    Role = "client",
                    Password = PasswordHelper.EncodePasswordMd5(register.Password),
                    CreatedAt = DateTime.Now
                };

                _context.Users.Add(user);
                _context.SaveChanges();
                return RedirectToAction(nameof(Login));
            }

            ModelState.AddModelError("Email", "خرابه");
            return View();
        }

        
        [Route("/login")]
        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated) 
            { 
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [Route("/login")]
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Login(LoginVM login)
        {
            if(ModelState.IsValid)
            {
                var user = _context.Users.SingleOrDefault(u => u.Email == login.Email);

                if(user != null)
                {
                    if(user.Password == PasswordHelper.EncodePasswordMd5(login.Password)) 
                    {
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
                            IsPersistent = login.IsRememberMe
                        };

                        HttpContext.SignInAsync(Principal, Properties);

                        if(user.Role != "admin")
                        {
                            return RedirectToAction("Index", "Home");
                        }
                        else
                        {
                            return Redirect("admin/products");
                        }
                        
                        
                    }
                    ModelState.AddModelError("Email", "اطلاعات وارد شده صحیح نمی باشد");
                    return View(); 
                }

                ModelState.AddModelError("Email", "ایمیل وارد شده وجود ندارد");
                return View();
            }

            ModelState.AddModelError("Email", "لطفا فیلدها رو کامل پر کنید");
            return View();
        }

        [Route("/logout")]
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("login");
        }

        [Route("/accessDenied")]
        public ActionResult AccessDenied()
        {
            return View();
        }
    }
}
