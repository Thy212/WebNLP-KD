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
    [Authorize]
    public class ResearchController : Controller
	{
        private readonly ApplicationDbContext _context;
        private INotyfService _notification { get; }
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly UserManager<ApplicationUser> _userManager;

        public ResearchController(ApplicationDbContext context,
                                INotyfService notyfService,
                                IWebHostEnvironment webHostEnvironment,
                                UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _notification = notyfService;
            _webHostEnvironment = webHostEnvironment;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int? page)
		{
            var listOfResearchs = new List<Research>();
            var loggedInUser = await _userManager.Users.FirstOrDefaultAsync(x => x.Email == User.Identity!.Name);
            var loggedInUserRole = await _userManager.GetRolesAsync(loggedInUser!);
            if (loggedInUserRole[0] == WebsiteRoles.WebsiteAdmin) // is Admin, sẽ thấy all post
            {
                listOfResearchs = await _context.Researchs!.Include(x => x.ApplicationUser).ToListAsync();
            }
            else // không phải Admin, chỉ thấy post của mình
            {
                listOfResearchs = await _context.Researchs!.Include(x => x.ApplicationUser).Where(x => x.ApplicationUser!.Id == loggedInUser!.Id).ToListAsync();
            }

            var listOfReseachsVM = listOfResearchs.Select(x => new ResearchVM()
            {
                Id = x.Id,
                Title = x.Title,
                CreateDate = x.CreateDate,
                ThumbnailUrl = x.ThumbnailUrl,
                ShortDescription = x.ShortDescription,
                AuthorName = x.AuthorName,
                LinkWebsite = x.LinkWebsite,
            }).ToList();
            
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(await listOfReseachsVM.OrderByDescending(x => x.CreateDate).ToPagedListAsync(pageNumber, pageSize));
            
		}

        [HttpGet]
        public IActionResult Create()
        {
            return View(new CreatePostVM());
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreatePostVM vm)
        {
            if (!ModelState.IsValid) { return View(vm); }
            //get logged in user id
            var loggedInUser = await _userManager.Users.FirstOrDefaultAsync(x => x.Email == User.Identity!.Name);

            var post = new Research();

            post.Title = vm.Title;
            post.AuthorName = vm.AuthorName;
            post.CreateDate = vm.CreateDate;
            post.LinkWebsite = vm.LinkWebsite;
            post.ShortDescription = vm.ShortDescription;
            post.ApplicationUserId = loggedInUser!.Id;

            if (post.Title != null)
            {
                string slug = vm.Title!.Trim();
                slug = slug.Replace(" ", "-");
                post.Slug = slug + "-" + Guid.NewGuid();

            }

            if (vm.Thumbnail != null)
            {
                post.ThumbnailUrl = UploadImage(vm.Thumbnail);
            }
            await _context.Researchs!.AddAsync(post);
            await _context.SaveChangesAsync();
            _notification.Success("Publications Created Successfully");

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var post = await _context.Researchs!.FirstOrDefaultAsync(x => x.Id == id);

            var loggedInUser = await _userManager.Users.FirstOrDefaultAsync(x => x.Email == User.Identity!.Name);
            var loggedInUserRole = await _userManager.GetRolesAsync(loggedInUser!);

            if (loggedInUserRole[0] == WebsiteRoles.WebsiteAdmin || loggedInUser?.Id == post?.ApplicationUserId)
            {
                _context.Researchs!.Remove(post!);
                await _context.SaveChangesAsync();

                _notification.Success("Publications Deleted");
                return RedirectToAction("Index", "Research", new { area = "Admin" });
            }
            return View();

        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var post = await _context.Researchs!.FirstOrDefaultAsync(x => x.Id == id);
            if (post == null)
            {
                _notification.Error("Publications not found");
                return View();
            }

            var loggedInUser = await _userManager.Users.FirstOrDefaultAsync(x => x.Email == User.Identity!.Name);
            var loggedInUserRole = await _userManager.GetRolesAsync(loggedInUser!);
            if (loggedInUserRole[0] != WebsiteRoles.WebsiteAdmin && loggedInUser!.Id != post.ApplicationUserId)
            {
                _notification.Error("You are not authorized");
                return RedirectToAction("Index");
            }

            var vm = new CreatePostVM()
            {
                Id = post.Id,
                Title = post.Title,
                ShortDescription = post.ShortDescription,
                ThumbnailUrl = post.ThumbnailUrl,
                AuthorName = post.AuthorName,
                CreateDate = post.CreateDate,
                LinkWebsite = post.LinkWebsite,
            };

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(CreatePostVM vm)
        {
            if (!ModelState.IsValid) { return View(vm); }
            var post = await _context.Researchs!.FirstOrDefaultAsync(x => x.Id == vm.Id);
            if (post == null)
            {
                _notification.Error("Publications not found");
                return View();
            }

            post.Title = vm.Title;
            post.ShortDescription = vm.ShortDescription;
            post.ThumbnailUrl = vm.ThumbnailUrl;
            post.AuthorName = vm.AuthorName;
            post.CreateDate = vm.CreateDate;
            post.LinkWebsite = vm.LinkWebsite;

            if (vm.Thumbnail != null)
            {
                post.ThumbnailUrl = UploadImage(vm.Thumbnail);
            }

            await _context.SaveChangesAsync();
            _notification.Success("Publications updated succesfully");
            return RedirectToAction("Index", "Research", new { area = "Admin" });
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
