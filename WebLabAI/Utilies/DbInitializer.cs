using Microsoft.AspNetCore.Identity;
using WebLabAI.Data;
using WebLabAI.Models;

namespace WebLabAI.Utilies
{
    public class DbInitializer : IDbInitializer
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DbInitializer(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public void Initialize()
        {
            if(!_roleManager.RoleExistsAsync(WebsiteRoles.WebsiteAdmin).GetAwaiter().GetResult())
            {
                _roleManager.CreateAsync(new IdentityRole(WebsiteRoles.WebsiteAdmin)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(WebsiteRoles.WebsiteAuthor)).GetAwaiter().GetResult();
                _userManager.CreateAsync(new ApplicationUser()
                {
                    //UserName = "admin@gmail.com",
                    Email = "admin@gmail.com",
                    FirstName = "Admin",
                    LastName = "Super"
                },"Admin@0011").Wait();

                var appUser = _context.ApplicationUser!.FirstOrDefault(x => x.Email == "admin@gmail.com");
                if (appUser != null)
                {
                    _userManager.AddToRoleAsync(appUser, WebsiteRoles.WebsiteAdmin).GetAwaiter().GetResult();
                }

                var listOfPages = new List<Page>()
                {
                    new Page()
                    {
                        Title = "About",
                        Slug = "about"
                    },
                    new Page()
                    {
                        Title = "Contact",
                        Slug = "contact"
                    },
                    new Page()
                    {
                        Title = "Research",
                        Slug = "research"
                    },
                    new Page()
                    {
                        Title = "Training",
                        Slug = "training"
                    },
                    new Page()
                    {
                        Title = "News",
                        Slug = "news"
                    },
                    new Page()
                    {
                        Title = "Device",
                        Slug = "device"
                    },
                };

                _context.Pages!.AddRange(listOfPages);
                _context.SaveChanges();
            }
        }
    }
}
