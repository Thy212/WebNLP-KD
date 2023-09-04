using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebLabAI.Models;

namespace WebLabAI.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<ApplicationUser>? ApplicationUser { get; set; }
        public DbSet<Post>? Posts { get; set; }
        public DbSet<Page>? Pages { get; set; }
        public DbSet<Setting>? Settings { get; set; }
        public DbSet<Research>? Researchs { get; set; }
        public DbSet<Training>? Trainings { get; set; }
        public DbSet<Device>? Devices { get; set; }
        public DbSet<ContactForm>? ContactForms { get; set; }
        public DbSet<Partner>? Partners { get; set; }

		
	}
}
