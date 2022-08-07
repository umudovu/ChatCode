using ChatCode.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ChatCode.Data
{
    public class AppDataContext: IdentityDbContext<AppUser>
    {
        public AppDataContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Message> Messages { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);


            builder.Entity<Message>()
               .HasOne(u => u.Recipient)
               .WithMany(m => m.MessagesReceived)
               .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Message>()
                .HasOne(u => u.Sender)
                .WithMany(m => m.MessagesSent)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
