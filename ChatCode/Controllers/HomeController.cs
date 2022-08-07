using ChatCode.Data;
using ChatCode.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ChatCode.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly AppDataContext _context;
        //private readonly IHubContext<MessageHub> _hubContext;
        public HomeController(ILogger<HomeController> logger, UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, AppDataContext context)
        {
            _logger = logger;
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }

        public async Task<IActionResult> Chat()
        {
            var user =await _userManager.FindByNameAsync(User.Identity.Name);


            return View(user);
        }


    }
}
