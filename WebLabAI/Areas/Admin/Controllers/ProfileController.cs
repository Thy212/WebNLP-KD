using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebLabAI.Data;
using WebLabAI.Models;
using WebLabAI.Utilies;
using WebLabAI.ViewModels;

namespace WebLabAI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class ProfileController : Controller
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly IWebHostEnvironment _webHostEnvironment;
		private INotyfService _notification { get; }

		public ProfileController(UserManager<ApplicationUser> userManager, IWebHostEnvironment webHostEnvironment,
									INotyfService notyfService)
		{
			_userManager = userManager;
			_notification = notyfService;
			_webHostEnvironment = webHostEnvironment;
		}

		

		[HttpGet]
		public async Task<IActionResult> Index(string id)
		{
			var loggedInUser = await _userManager.Users.FirstOrDefaultAsync(x => x.Email == User.Identity!.Name); // check who loging
			var userEdit = await _userManager.FindByIdAsync(loggedInUser!.Id);
			//var userEdit = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == user.UserName);
			if (userEdit == null)
			{
				_notification.Error("User Not Found");
				return View();
			}

			var vm = new EditStaff()
			{
				FisrtName = userEdit.FirstName,
				LastName = userEdit.LastName,
				Email = userEdit.Email,
				Birthday = userEdit.Birthday,
				Education = userEdit.Education,
				LinkedinUrl = userEdit.LinkedinUrl,
				GithubUrl = userEdit.GithubUrl,
				//IsAdmin = userEdit.isAdmin,
				ThumbnailUrl = userEdit.ThumbnailUrl,
				Position = userEdit.Position,
			};

            return View(vm);
		}

		[HttpPost]
		public async Task<IActionResult> Index(EditStaff vm)
		{
			if (!ModelState.IsValid)
			{
				return View(vm);
			}

			var userEdit = await _userManager.Users.FirstOrDefaultAsync(x => x.Email == vm.Email);

			if (userEdit == null)
			{
				_notification.Error("User Not Found");
				return View();
			}

			userEdit.FirstName = vm.FisrtName;
			userEdit.LastName = vm.LastName;
			userEdit.Email = vm.Email;
			userEdit.Birthday = vm.Birthday;
			userEdit.Education = vm.Education;
			userEdit.LinkedinUrl = vm.LinkedinUrl;
			userEdit.GithubUrl = vm.GithubUrl;

            if (vm.Thumbnail != null)
            {
                userEdit.ThumbnailUrl = UploadImage(vm.Thumbnail);
            }


            await _userManager.UpdateAsync(userEdit);
			_notification.Success("User updated succesfully");
			return RedirectToAction("Index");
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
