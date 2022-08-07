using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace ChatCode.Models
{
    public class AppUser: IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ImgUrl { get; set; }
        public string ConnectionId { get; set; }
        public ICollection<Message> MessagesSent { get; set; }
        public ICollection<Message> MessagesReceived { get; set; }
    }
}
