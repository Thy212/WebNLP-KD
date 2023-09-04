using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebLabAI.Data;
using WebLabAI.Models;
using WebLabAI.ViewModels;
using Microsoft.Extensions.Hosting;
using WebLabAI.Utilies;

namespace WebLabAI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class PageController : Controller
    {
        private readonly ApplicationDbContext _context;
        private INotyfService _notification { get; }
        private readonly IWebHostEnvironment _webHostEnvironment;

        public PageController(ApplicationDbContext context,
                                INotyfService notification,
                                IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _notification = notification;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        public async Task<IActionResult> About()
        {
            var page = await _context.Pages!.FirstOrDefaultAsync(x => x.Slug == "about");
            var vm = new PageVM()
            {
                Id = page!.Id,
                Title = page.Title,
                ShortDescription = page.ShortDescription,
                Description = page.Description,
                ThumbnailUrl = page.ThumbnailUrl,
            };
            return View(vm);
        }

        [HttpPost]
		public async Task<IActionResult> About(PageVM vm)
		{
			if (!ModelState.IsValid) { return View(vm); }
			var page = await _context.Pages!.FirstOrDefaultAsync(x => x.Slug == "about");
			if (page == null)
			{
				_notification.Error("Page not found");
				return View();
			}
			page.Title = vm.Title;
			page.ShortDescription = vm.ShortDescription;
			page.Description = vm.Description;

			if (vm.Thumbnail != null)
			{
				page.ThumbnailUrl = UploadImage(vm.Thumbnail);
			}

			await _context.SaveChangesAsync();
			_notification.Success("About page updated succesfully");
			return RedirectToAction("About", "Page", new { area = "Admin" });
		}

		[HttpGet]
		public async Task<IActionResult> Contact()
		{
			var page = await _context.Pages!.FirstOrDefaultAsync(x => x.Slug == "contact");
			var vm = new PageVM()
			{
				Id = page!.Id,
				Title = page.Title,
				ShortDescription = page.ShortDescription,
				Description = page.Description,
				ThumbnailUrl = page.ThumbnailUrl,
				Phone = page.Phone,
				Email = page.Email,
				Address = page.Address,
			};

            
            return View(vm);
		}

		[HttpPost]
		public async Task<IActionResult> Contact(PageVM vm)
		{
			if (!ModelState.IsValid) { return View(vm); }
			var page = await _context.Pages!.FirstOrDefaultAsync(x => x.Slug == "contact");
			if (page == null)
			{
				_notification.Error("Page not found");
				return View();
			}
			page.Title = vm.Title;
			page.ShortDescription = vm.ShortDescription;
			page.Description = vm.Description;
			page.Phone = vm.Phone;
			page.Email = vm.Email;
			page.Address = vm.Address;

			if (vm.Thumbnail != null)
			{
				page.ThumbnailUrl = UploadImage(vm.Thumbnail);
			}

			await _context.SaveChangesAsync();
			_notification.Success("Contact page updated succesfully");
			return RedirectToAction("Contact", "Page", new { area = "Admin" });
		}


        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var send = await _context.ContactForms!.FirstOrDefaultAsync(x => x.Id == id);

            _context.ContactForms!.Remove(send!);
            await _context.SaveChangesAsync();

            _notification.Success("Contact Deleted");
            return RedirectToAction("Contact", "Page", new { area = "Admin" });

        }

        [HttpGet]
		public async Task<IActionResult> Research()
		{
			var page = await _context.Pages!.FirstOrDefaultAsync(x => x.Slug == "research");
			var vm = new PageVM()
			{
				Id = page!.Id,
				Title = page.Title,
				ShortDescription = page.ShortDescription,
				Description = page.Description,
				ThumbnailUrl = page.ThumbnailUrl,
			};
			return View(vm);
		}

		[HttpPost]
		public async Task<IActionResult> Research(PageVM vm)
		{
			if (!ModelState.IsValid) { return View(vm); }
			var page = await _context.Pages!.FirstOrDefaultAsync(x => x.Slug == "research");
			if (page == null)
			{
				_notification.Error("Page not found");
				return View();
			}
			page.Title = vm.Title;
			page.ShortDescription = vm.ShortDescription;
			page.Description = vm.Description;

			if (vm.Thumbnail != null)
			{
				page.ThumbnailUrl = UploadImage(vm.Thumbnail);
			}

			await _context.SaveChangesAsync();
			_notification.Success("Research page updated succesfully");
			return RedirectToAction("Research", "Page", new { area = "Admin" });
		}

        [HttpGet]
        public async Task<IActionResult> Training()
        {
            var page = await _context.Pages!.FirstOrDefaultAsync(x => x.Slug == "training");
            var vm = new PageVM()
            {
                Id = page!.Id,
                Title = page.Title,
                ShortDescription = page.ShortDescription,
                Description = page.Description,
                ThumbnailUrl = page.ThumbnailUrl,
            };
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Training(PageVM vm)
        {
            if (!ModelState.IsValid) { return View(vm); }
            var page = await _context.Pages!.FirstOrDefaultAsync(x => x.Slug == "training");
            if (page == null)
            {
                _notification.Error("Page not found");
                return View();
            }
            page.Title = vm.Title;
            page.ShortDescription = vm.ShortDescription;
            page.Description = vm.Description;

            if (vm.Thumbnail != null)
            {
                page.ThumbnailUrl = UploadImage(vm.Thumbnail);
            }

            await _context.SaveChangesAsync();
            _notification.Success("Training page updated succesfully");
            return RedirectToAction("Training", "Page", new { area = "Admin" });
        }

		[HttpGet]
		public async Task<IActionResult> Device()
		{
			var page = await _context.Pages!.FirstOrDefaultAsync(x => x.Slug == "device");
			var vm = new PageVM()
			{
				Id = page!.Id,
				Title = page.Title,
				ShortDescription = page.ShortDescription,
				Description = page.Description,
				ThumbnailUrl = page.ThumbnailUrl,
			};
			return View(vm);
		}

		[HttpPost]
		public async Task<IActionResult> Device(PageVM vm)
		{
			if (!ModelState.IsValid) { return View(vm); }
			var page = await _context.Pages!.FirstOrDefaultAsync(x => x.Slug == "device");
			if (page == null)
			{
				_notification.Error("Page not found");
				return View();
			}
			page.Title = vm.Title;
			page.ShortDescription = vm.ShortDescription;
			page.Description = vm.Description;

			if (vm.Thumbnail != null)
			{
				page.ThumbnailUrl = UploadImage(vm.Thumbnail);
			}

			await _context.SaveChangesAsync();
			_notification.Success("Device page updated succesfully");
			return RedirectToAction("Device", "Page", new { area = "Admin" });
		}

        [HttpGet]
        public async Task<IActionResult> News()
        {
            var page = await _context.Pages!.FirstOrDefaultAsync(x => x.Slug == "news");
            var vm = new PageVM()
            {
                Id = page!.Id,
                Title = page.Title,
                ShortDescription = page.ShortDescription,
                Description = page.Description,
                ThumbnailUrl = page.ThumbnailUrl,
            };
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> News(PageVM vm)
        {
            if (!ModelState.IsValid) { return View(vm); }
            var page = await _context.Pages!.FirstOrDefaultAsync(x => x.Slug == "news");
            if (page == null)
            {
                _notification.Error("Page not found");
                return View();
            }
            page.Title = vm.Title;
            page.ShortDescription = vm.ShortDescription;
            page.Description = vm.Description;

            if (vm.Thumbnail != null)
            {
                page.ThumbnailUrl = UploadImage(vm.Thumbnail);
            }

            await _context.SaveChangesAsync();
            _notification.Success("News page updated succesfully");
            return RedirectToAction("News", "Page", new { area = "Admin" });
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
