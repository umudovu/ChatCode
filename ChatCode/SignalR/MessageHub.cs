using ChatCode.Data;
using ChatCode.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ChatCode.SignalR
{
    public class MessageHub:Hub
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly AppDataContext _context;

        public MessageHub(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<IdentityRole> roleManager, AppDataContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _context = context;
        }

        public RoleManager<IdentityRole> RoleManager => _roleManager;

        public override async Task OnConnectedAsync()
        {
            var user = _userManager.FindByNameAsync(Context.User.Identity.Name).Result;
            user.ConnectionId = Context.ConnectionId;
            _context.SaveChanges();

            var users = _context.Users.Where(x => x.ConnectionId != null).Where(x => x.Id != user.Id).ToList();

            await Clients.Client(user.ConnectionId).SendAsync("OnlineUsers", users);
        }
        

        public async Task SendMessage(string message)
        {
            var currentuser = _userManager.FindByNameAsync(Context.User.Identity.Name).Result;
            Message newMessage = new Message()
            {
                SenderId = currentuser.Id,
                Content = message,
                SenderUsername = currentuser.UserName
            };
            await _context.Messages.AddAsync(newMessage);
            await _context.SaveChangesAsync();

            await Clients.All.SendAsync("ReceiveMessage", currentuser, newMessage.Content);
        }
    }
}
