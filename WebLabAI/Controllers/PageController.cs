using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebLabAI.Data;
using WebLabAI.ViewModels;
using WebLabAI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Hosting;

namespace WebLabAI.Controllers
{
    public class PageController : Controller
    {
        private readonly ApplicationDbContext _context;
		public INotyfService _notification { get; }
		private readonly IWebHostEnvironment _webHostEnvironment;

		public PageController(ApplicationDbContext context,
								INotyfService notification,
								IWebHostEnvironment webHostEnvironment)
        {
			_context = context;
			_notification = notification;
			_webHostEnvironment = webHostEnvironment;
		}

		//--------- about  ---------------
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

		//-------------- contact --------------
		[HttpGet]
		public IActionResult Contact()
		{
			return View();
		}
       
        [HttpPost]
        public async Task<IActionResult> Contact(ContactFormVM vm)  // them user
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            //
            var mess = new ContactForm();

            mess.FirstName = vm.FirstName;
            mess.LastName = vm.LastName;
            mess.Email = vm.Email;
            mess.Phone = vm.Phone;
            mess.Message = vm.Message;
            mess.DateSend = DateTime.Now;

            await _context.ContactForms!.AddAsync(mess);

            await _context.SaveChangesAsync();
            _notification.Success("The information has been submitted; we will contact you as soon as possible");
            return RedirectToAction("Contact");
        }

        //------------- research -----------------------------
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

        //--------------- training --------------------------
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

        //--------------- news ------------------------------
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

        //--------------- device --------------------
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

        //-------------thumbnail ------------------
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
