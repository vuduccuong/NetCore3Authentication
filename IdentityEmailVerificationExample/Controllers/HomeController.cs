using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityEmailVerificationExample.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NETCore.MailKit.Core;

namespace IdentityEmailVerificationExample.Controllers
{
    public class HomeController : Controller
    {
        private readonly IEmailService _emailService;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public HomeController( 
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            IEmailService emailService)
        {
            _emailService = emailService;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [Authorize]
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user != null)
            {
               var result = await _signInManager.PasswordSignInAsync(user, password, false, false);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
            }
            return RedirectToAction("Login");
        }
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(string username, string password)
        {
            var user = new IdentityUser
            {
                UserName = username,
                Email = "cuongvd9"
            };
            var resultRegister = await _userManager.CreateAsync(user, password);
            if (resultRegister.Succeeded)
            {
                //Nếu đăng ký thành công thì gen token và gửi email
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var link = Url.Action(nameof(EmailVerify), "Home", new { userId = user.Id, code }, Request.Scheme, Request.Host.ToString());
                await _emailService.SendAsync("vuduccuong.ck48@gmail.com", "TEST confirm account", $"<a href=\"{link}\">Confirm</a>", true);
                //return
                return RedirectToAction("EmailVerifytion"); 
            }
            return RedirectToAction("Login");
        }

        public IActionResult EmailVerifytion() => View();
        public async Task<IActionResult> EmailVerify(string userId, string code)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return BadRequest("Không tìm thấy tài khoản");
            var resultConfirm = await _userManager.ConfirmEmailAsync(user, code);
            if (resultConfirm.Succeeded)
            {
                return View();
            }
          return BadRequest("Không tìm thấy tài khoản");
        }
    }
}