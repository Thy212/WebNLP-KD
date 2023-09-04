using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebLabAI.Data;
using WebLabAI.Models;
using WebLabAI.Utilies;
using WebLabAI.ViewModels;
using X.PagedList;

namespace WebLabAI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class PartnerController : Controller
    {
        private readonly ApplicationDbContext _context;
        private INotyfService _notification { get; }
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly UserManager<ApplicationUser> _userManager;

        public PartnerController(ApplicationDbContext context,
                                INotyfService notyfService,
                                IWebHostEnvironment webHostEnvironment,
                                UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _notification = notyfService;
            _webHostEnvironment = webHostEnvironment;
            _userManager = userManager;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Index(int? page)
        {
            
            var partner = await _context.Partners!.ToListAsync();
            var vm = partner.Select(d => new PartnerVM
            {
                Id = d.Id,
                Name = d.Name,
                ThumbnailUrl = d.ThumbnailUrl,
                LinkWebsite = d.LinkWebsite,
            }).ToList();
            // assinging role


            return View(vm.OrderBy(x => x.Name).ToList());
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Create()
        {
            return View(new PartnerVM());
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create(PartnerVM vm)
        {
            if (!ModelState.IsValid) { return View(vm); }
            //get logged in user id
            var checkPartner = await _context.Partners!.FirstOrDefaultAsync(x => x.Name == vm.Name);
            if (checkPartner != null)
            {
                _notification.Error("Partner already exists");
                return View(vm);
            }
            var par = new Partner();

            par.Name = vm.Name;
            par.ThumbnailUrl = vm.ThumbnailUrl;
            par.LinkWebsite = vm.LinkWebsite;

            if (vm.Thumbnail != null)
            {
                par.ThumbnailUrl = UploadImage(vm.Thumbnail);
            }
            await _context.Partners!.AddAsync(par);
            await _context.SaveChangesAsync();
            _notification.Success("Partner Created Successfully");

            return RedirectToAction("Index");
        }

        
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var post = await _context.Partners!.FirstOrDefaultAsync(x => x.Id == id);

            var loggedInUser = await _userManager.Users.FirstOrDefaultAsync(x => x.Email == User.Identity!.Name);
            var loggedInUserRole = await _userManager.GetRolesAsync(loggedInUser!);

            if (loggedInUserRole[0] == WebsiteRoles.WebsiteAdmin)
            {
                _context.Partners!.Remove(post!);
                await _context.SaveChangesAsync();

                _notification.Success("Partner Deleted");
                return RedirectToAction("Index", "Partner", new { area = "Admin" });

            }
            return View();


        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var post = await _context.Partners!.FirstOrDefaultAsync(x => x.Id == id);
            if (post == null)
            {
                _notification.Error("Partner not found");
                return View();
            }

            var vm = new PartnerVM()
            {
                Id = post.Id,
                Name = post.Name,
                ThumbnailUrl = post.ThumbnailUrl,
                LinkWebsite = post.LinkWebsite,
            };

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(PartnerVM vm)
        {
            if (!ModelState.IsValid) { return View(vm); }
            var post = await _context.Partners!.FirstOrDefaultAsync(x => x.Id == vm.Id);
            if (post == null)
            {
                _notification.Error("Partner not found");
                return View();
            }

            post.Name = vm.Name;
            post.ThumbnailUrl = vm.ThumbnailUrl;
            post.LinkWebsite = vm.LinkWebsite;

            if (vm.Thumbnail != null)
            {
                post.ThumbnailUrl = UploadImage(vm.Thumbnail);
            }

            await _context.SaveChangesAsync();
            _notification.Success("Partner updated succesfully");
            return RedirectToAction("Index", "Partner", new { area = "Admin" });
        }

        private string UploadImage(IFormFile file)
        {
            string uniqueFileName = "";
            var folderPath = Path.Combine(_webHostEnvironment.WebRootPath, "thumbnails");
            uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
            var filePath = Path.Combine(folderPath, uniqueFileName);
            using (FileStream fileStream = System.IO.File.Create(filePath))
            {
                file.CopyTo(fileStream);
            }
            return uniqueFileName;

        }
    }
}
