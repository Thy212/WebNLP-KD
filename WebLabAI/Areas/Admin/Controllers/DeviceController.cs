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
	public class DeviceController : Controller
	{
		private readonly ApplicationDbContext _context;
		private INotyfService _notification { get; }
		private readonly IWebHostEnvironment _webHostEnvironment;
		private readonly UserManager<ApplicationUser> _userManager;

		public DeviceController(ApplicationDbContext context,
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
			var devices = await _context.Devices!.ToListAsync();
			var vm = devices.Select(d => new DeviceVM
			{
				Id = d.Id,
				DeviceName = d.DeviceName,
				Rent = d.Rent,
				Price = d.Price,
				ThumbnailUrl = d.ThumbnailUrl
			}).ToList();
			// assinging role
			
			
			return View(vm.OrderBy(x => x.DeviceName).ToList());
			
		}

		[Authorize(Roles = "Admin")]
		[HttpGet]
		public IActionResult Create()
		{
			return View(new DeviceVM());
		}

		[Authorize(Roles = "Admin")]
		[HttpPost]
		public async Task<IActionResult> Create(DeviceVM vm)
		{
			if (!ModelState.IsValid) { return View(vm); }

			//check devices
			var checkDevices = await _context.Devices!.FirstOrDefaultAsync(x => x.DeviceName == vm.DeviceName);
			if(checkDevices != null)
			{
				_notification.Error("Device already exists");
				return View(vm);
			}
			
			var device = new Device();

			device.Id = vm.Id;
			device.DeviceName = vm.DeviceName;
			device.Rent = vm.Rent;
			device.Price = vm.Price;

			if (vm.Thumbnail != null)
			{
				device.ThumbnailUrl = UploadImage(vm.Thumbnail);
			}
			await _context.Devices!.AddAsync(device);
			await _context.SaveChangesAsync();
			_notification.Success("Device Created Successfully");

			return RedirectToAction("Index");
		}


		[HttpPost]
		public async Task<IActionResult> Delete(int id)
		{
			var post = await _context.Devices!.FirstOrDefaultAsync(x => x.Id == id);

			var loggedInUser = await _userManager.Users.FirstOrDefaultAsync(x => x.Email == User.Identity!.Name);
			var loggedInUserRole = await _userManager.GetRolesAsync(loggedInUser!);

			if (loggedInUserRole[0] == WebsiteRoles.WebsiteAdmin)
			{
				_context.Devices!.Remove(post!);
				await _context.SaveChangesAsync();

				_notification.Success("Device Deleted");
				return RedirectToAction("Index", "Device", new { area = "Admin" });

			}
			return View();

		}

		[HttpGet]
		public async Task<IActionResult> Edit(int id)
		{
			var post = await _context.Devices!.FirstOrDefaultAsync(x => x.Id == id);
			if (post == null)
			{
				_notification.Error("Device not found");
				return View();
			}

			var vm = new DeviceVM()
			{
				Id = post.Id,
				DeviceName = post.DeviceName,
				Rent = post.Rent,
				ThumbnailUrl = post.ThumbnailUrl,
				Price = post.Price,
			};

			return View(vm);
		}

		[HttpPost]
		public async Task<IActionResult> Edit(DeviceVM vm)
		{
			if (!ModelState.IsValid) { return View(vm); }
			var post = await _context.Devices!.FirstOrDefaultAsync(x => x.Id == vm.Id);
			if (post == null)
			{
				_notification.Error("Device not found");
				return View();
			}

			post.DeviceName = vm.DeviceName;
			post.Rent = vm.Rent;
			post.ThumbnailUrl = vm.ThumbnailUrl;
			post.Price = vm.Price;

			if (vm.Thumbnail != null)
			{
				post.ThumbnailUrl = UploadImage(vm.Thumbnail);
			}

			await _context.SaveChangesAsync();
			_notification.Success("Device updated succesfully");
			return RedirectToAction("Index", "Device", new { area = "Admin" });
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
