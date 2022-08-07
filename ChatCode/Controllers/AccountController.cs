using ChatCode.Data;
using ChatCode.Models;
using ChatCode.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SignalR_Intro.Extentions;
using System;
using System.Threading.Tasks;
using static SignalR_Intro.Helpers.Helper;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace ChatCode.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly AppDataContext _context;
        private readonly IWebHostEnvironment _env;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<IdentityRole> roleManager, AppDataContext context, IWebHostEnvironment env)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _context = context;
            _env = env;
        }
        public IActionResult Login()
        {
            return View();
        }
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterVM registerVM)
        {
            if (!ModelState.IsValid) return View();

            if (ModelState["Photo"] == null)
            {
                ModelState.AddModelError("Photo", "Photo is null");
                return View();
            }
            if (!registerVM.Photo.IsImage())
            {
                ModelState.AddModelError("Photo", "Only image");
                return View();
            }
            if (registerVM.Photo.ImageSize(800))
            {
                ModelState.AddModelError("Photo", "Olcu oversize");
                return View();
            }


            AppUser appUser = new AppUser
            {
                FirstName = registerVM.FirstName,
                LastName = registerVM.LastName,
                Email = registerVM.Email,
                UserName = registerVM.UserName,
                ImgUrl = registerVM.Photo.SaveImage(_env, "img")

            };

            IdentityResult result = await _userManager.CreateAsync(appUser, registerVM.Password);

            if (!result.Succeeded)
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }
                return View(registerVM);
            }

            await _userManager.AddToRoleAsync(appUser, UserRoles.Member.ToString());
            return RedirectToAction("login");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVM loginVM, string ReturnUrl)
        {
            if (!ModelState.IsValid) return View();

            AppUser appUser = await _userManager.FindByEmailAsync(loginVM.Email);


            if (appUser == null)
            {
                ModelState.AddModelError("", "Email veya password invalid");
                return View(loginVM);
            }



            SignInResult result = await _signInManager.PasswordSignInAsync(appUser, loginVM.Password, true, true);

            if (result.IsLockedOut)
            {
                ModelState.AddModelError("", "Email veya password invalid");
                return View(loginVM);
            }
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Email veya password invalid");
                return View(loginVM);
            }

            var roles = await _userManager.GetRolesAsync(appUser);


            return RedirectToAction("chat", "home");
        }

        public async Task<IActionResult> Logout()
        {
            var user = _userManager.FindByNameAsync(User.Identity.Name).Result;
            user.ConnectionId = null;

            await _signInManager.SignOutAsync();

            return RedirectToAction("chat", "Home");
        }


        public async Task CreateRole()
        {
            foreach (var item in Enum.GetValues(typeof(UserRoles)))
            {
                if (!await _roleManager.RoleExistsAsync(item.ToString()))
                {
                    await _roleManager.CreateAsync(new IdentityRole { Name = item.ToString() });
                }
            };
        }


    }
}
