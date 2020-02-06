using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Basics.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public IActionResult Secret()
        {
            return View();
        }

        public IActionResult Authentication()
        {
            var grandmaClaim = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, "Vu Duc Cuong"),
                new Claim(ClaimTypes.Email, "cuongvd9@fpt.com.vn"),
                new Claim("Cuong.Say", "Hello, World")
            };

            var lincenseClaim = new List<Claim>()
            {
                new Claim(ClaimTypes.Email, "vuduccuong.ck48@gmail.com"),
                new Claim("DrivingLincense", "A+")
            };

            var grandmaIdentity = new ClaimsIdentity(grandmaClaim, "Grandma Identity");
            var lincenseIdentity = new ClaimsIdentity(lincenseClaim, "Lincense Identity");

            var userPrincipal = new ClaimsPrincipal(new[] { grandmaIdentity, lincenseIdentity });

            HttpContext.SignInAsync(userPrincipal);

            return RedirectToAction("Index");
        }
    }
}