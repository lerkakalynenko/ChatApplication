using System;
using System.Threading.Tasks.Dataflow;
using ChatApp.DAL.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.DAL.EF
{
    public class ApplicationDbContext : IdentityDbContext<AppUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }


       public DbSet<Chat> Chats { get; set; }
       public DbSet<Message> Messages { get; set; }
       public DbSet<ChatUser> ChatUsers { get; set; }


        
        protected override void OnModelCreating(ModelBuilder builder)
       {
           base.OnModelCreating(builder);
           
           builder.Entity<ChatUser>()
               .HasKey(c => new {c.ChatId, c.UserId});

       }
      
       
    }



}
