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
    public class TrainingController : Controller
    {
        private readonly ApplicationDbContext _context;
        private INotyfService _notification { get; }
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly UserManager<ApplicationUser> _userManager;

        public TrainingController(ApplicationDbContext context,
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
        public async Task<IActionResult> IndexCourse(int? page)
        {
            var listOfTrainings = new List<Training>();
            var loggedInUser = await _userManager.Users.FirstOrDefaultAsync(x => x.Email == User.Identity!.Name);
            var loggedInUserRole = await _userManager.GetRolesAsync(loggedInUser!);
            if (loggedInUserRole[0] == WebsiteRoles.WebsiteAdmin) // is Admin, sẽ thấy all post
            {
                listOfTrainings = await _context.Trainings!.Include(x => x.ApplicationUser).ToListAsync();
            }
            else // không phải Admin, chỉ thấy post của mình
            {
                listOfTrainings = await _context.Trainings!.Include(x => x.ApplicationUser).Where(x => x.ApplicationUser!.Id == loggedInUser!.Id).ToListAsync();
            }

            var listofTrVM = listOfTrainings.Select(x => new TrainingVM()
            {
                Id = x.Id,
                Title = x.Title,
                ToDate = x.ToDate,
                ThumbnailUrl = x.ThumbnailUrl,
                ShortDescription = x.ShortDescription,
                IsCourse = true,
                Price = x.Price,
                RegisterUrl = x.RegisterUrl,
                VideoRecord = x.VideoRecord,
                Resources = x.Resources,
                Assignment = x.Assignment,
                Certificate = x.Certificate,
                Online = x.Online,
                Teacher = x.Teacher,
            }).ToList();

            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(await listofTrVM.OrderByDescending(x => x.ToDate).ToPagedListAsync(pageNumber, pageSize));

        }

        [HttpGet]
        public async Task<IActionResult> IndexMeeting(int? page)
        {
            var listOfTrainings = new List<Training>();
            var loggedInUser = await _userManager.Users.FirstOrDefaultAsync(x => x.Email == User.Identity!.Name);
            var loggedInUserRole = await _userManager.GetRolesAsync(loggedInUser!);
            if (loggedInUserRole[0] == WebsiteRoles.WebsiteAdmin) // is Admin, sẽ thấy all post
            {
                listOfTrainings = await _context.Trainings!.Include(x => x.ApplicationUser).ToListAsync();
            }
            else // không phải Admin, chỉ thấy post của mình
            {
                listOfTrainings = await _context.Trainings!.Include(x => x.ApplicationUser).Where(x => x.ApplicationUser!.Id == loggedInUser!.Id).ToListAsync();
            }

            var listOfReseachsVM = listOfTrainings.Select(x => new TrainingVM()
            {
                Id = x.Id,
                Title = x.Title,
                ToDate = x.ToDate,
                ThumbnailUrl = x.ThumbnailUrl,
                ShortDescription = x.ShortDescription,
                IsCourse = false,
                Price = x.Price,
                RegisterUrl = x.RegisterUrl,
                VideoRecord = x.VideoRecord,
                Resources = x.Resources,
                Assignment = x.Assignment,
                Certificate = x.Certificate,
                Online = x.Online,
                Teacher = x.Teacher,
            }).ToList();

            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(await listOfReseachsVM.OrderByDescending(x => x.ToDate).ToPagedListAsync(pageNumber, pageSize));

        }

        [HttpGet]
        public IActionResult CreateCourse()
        {
            return View(new TrainingVM());
        }

        [HttpPost]
        public async Task<IActionResult> CreateCourse(TrainingVM vm)
        {
            if (!ModelState.IsValid) { return View(vm); }
            //get logged in user id
            var loggedInUser = await _userManager.Users.FirstOrDefaultAsync(x => x.Email == User.Identity!.Name);

            var course = new Training();

            course.Title = vm.Title;
            course.ToDate = vm.ToDate;
            course.ShortDescription = vm.ShortDescription;
            course.Description = vm.Description;
            course.IsCourse = true;
            course.Price = vm.Price;
            course.RegisterUrl = vm.RegisterUrl;
            course.ApplicationUserId = loggedInUser!.Id;
            course.VideoRecord = vm.VideoRecord;
            course.Resources = vm.Resources;
            course.Assignment = vm.Assignment;
            course.Certificate = vm.Certificate;
            course.Online = vm.Online;
            course.Teacher = vm.Teacher;

            if (course.Title != null)
            {
                string slug = vm.Title!.Trim();
                slug = slug.Replace(" ", "-");
                course.Slug = slug + "-" + Guid.NewGuid();

            }

            if (vm.Thumbnail != null)
            {
                course.ThumbnailUrl = UploadImage(vm.Thumbnail);
            }
            await _context.Trainings!.AddAsync(course);
            await _context.SaveChangesAsync();
            _notification.Success("Course Created Successfully");

            return RedirectToAction("IndexCourse");
        }

        [HttpGet]
        public IActionResult CreateMeeting()
        {
            return View(new TrainingVM());
        }

        [HttpPost]
        public async Task<IActionResult> CreateMeeting(TrainingVM vm)
        {
            if (!ModelState.IsValid) { return View(vm); }
            //get logged in user id
            var loggedInUser = await _userManager.Users.FirstOrDefaultAsync(x => x.Email == User.Identity!.Name);

            var meet = new Training();

			meet.Title = vm.Title;
			meet.ToDate = vm.ToDate;
			meet.ShortDescription = vm.ShortDescription;
			meet.Description = vm.Description;
			meet.IsCourse = false;
			meet.Price = vm.Price;
			meet.RegisterUrl = vm.RegisterUrl;
			meet.ApplicationUserId = loggedInUser!.Id;
            meet.VideoRecord = vm.VideoRecord;
            meet.Resources = vm.Resources;
            meet.Assignment = vm.Assignment;
            meet.Certificate = vm.Certificate;
            meet.Online = vm.Online;
            meet.Teacher = vm.Teacher;

            if (meet.Title != null)
            {
                string slug = vm.Title!.Trim();
                slug = slug.Replace(" ", "-");
				meet.Slug = slug + "-" + Guid.NewGuid();

            }

            if (vm.Thumbnail != null)
            {
				meet.ThumbnailUrl = UploadImage(vm.Thumbnail);
            }
            await _context.Trainings!.AddAsync(meet);
            await _context.SaveChangesAsync();
            _notification.Success("Meeting Created Successfully");

            return RedirectToAction("IndexMeeting");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var post = await _context.Trainings!.FirstOrDefaultAsync(x => x.Id == id);

            var loggedInUser = await _userManager.Users.FirstOrDefaultAsync(x => x.Email == User.Identity!.Name);
            var loggedInUserRole = await _userManager.GetRolesAsync(loggedInUser!);

            if (loggedInUserRole[0] == WebsiteRoles.WebsiteAdmin || loggedInUser?.Id == post?.ApplicationUserId)
            {
                _context.Trainings!.Remove(post!);
                await _context.SaveChangesAsync();

                if (post!.IsCourse == true)
                {
                    _notification.Success("Course Deleted");
                    return RedirectToAction("IndexCourse", "Training", new { area = "Admin" });
                }
                else
                {
                    _notification.Success("Meeting Deleted");
                    return RedirectToAction("IndexMeeting", "Training", new { area = "Admin" });
                }
            }
            return View();

        }

        [HttpGet]
        public async Task<IActionResult> EditCourse(int id)
        {
            var post = await _context.Trainings!.FirstOrDefaultAsync(x => x.Id == id);
            if (post == null)
            {
                _notification.Error("Course not found");
                return View();
            }

            var loggedInUser = await _userManager.Users.FirstOrDefaultAsync(x => x.Email == User.Identity!.Name);
            var loggedInUserRole = await _userManager.GetRolesAsync(loggedInUser!);
            if (loggedInUserRole[0] != WebsiteRoles.WebsiteAdmin && loggedInUser!.Id != post.ApplicationUserId)
            {
                _notification.Error("You are not authorized");
                
                if (post!.IsCourse == true)
                {
                    return RedirectToAction("IndexCourse", "Training", new { area = "Admin" });
                }
                else
                {
                    return RedirectToAction("IndexMeeting", "Training", new { area = "Admin" });
                }
            }

            var vm = new TrainingVM()
            {
                Id = post.Id,
                Title = post.Title,
                ShortDescription = post.ShortDescription,
                Description = post.Description,
                ThumbnailUrl = post.ThumbnailUrl,
                ToDate = post.ToDate,
                IsCourse = true,
                Price = post.Price,
                RegisterUrl = post.RegisterUrl,
                VideoRecord = post.VideoRecord,
                Resources = post.Resources,
                Assignment = post.Assignment,
                Certificate = post.Certificate,
                Online = post.Online,
                Teacher = post.Teacher,
            };

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> EditCourse(TrainingVM vm)
        {
            if (!ModelState.IsValid) { return View(vm); }
            var post = await _context.Trainings!.FirstOrDefaultAsync(x => x.Id == vm.Id);
            if (post == null)
            {
                _notification.Error("Course not found");
                return View();
            }

            post.Title = vm.Title;
            post.ShortDescription = vm.ShortDescription;
            post.Description = vm.Description;
            post.ThumbnailUrl = vm.ThumbnailUrl;
            post.ToDate = vm.ToDate;
            post.Price = vm.Price;
            post.RegisterUrl = vm.RegisterUrl;
            post.VideoRecord = vm.VideoRecord;
            post.Resources = vm.Resources;
            post.Assignment = vm.Assignment;
            post.Certificate = vm.Certificate;
            post.Online = vm.Online;
            post.Teacher = vm.Teacher;

            if (vm.Thumbnail != null)
            {
                post.ThumbnailUrl = UploadImage(vm.Thumbnail);
            }

            await _context.SaveChangesAsync();
            _notification.Success("Course updated succesfully");
            return RedirectToAction("IndexCourse", "Training", new { area = "Admin" });
        }

        [HttpGet]
        public async Task<IActionResult> EditMeeting(int id)
        {
            var post = await _context.Trainings!.FirstOrDefaultAsync(x => x.Id == id);
            if (post == null)
            {
                _notification.Error("Meeting not found");
                return View();
            }

            var loggedInUser = await _userManager.Users.FirstOrDefaultAsync(x => x.Email == User.Identity!.Name);
            var loggedInUserRole = await _userManager.GetRolesAsync(loggedInUser!);
            if (loggedInUserRole[0] != WebsiteRoles.WebsiteAdmin && loggedInUser!.Id != post.ApplicationUserId)
            {
                _notification.Error("You are not authorized");

                if (post!.IsCourse == true)
                {
                    return RedirectToAction("IndexCourse", "Training", new { area = "Admin" });
                }
                else
                {
                    return RedirectToAction("IndexMeeting", "Training", new { area = "Admin" });
                }
            }

            var vm = new TrainingVM()
            {
                Id = post.Id,
                Title = post.Title,
                ShortDescription = post.ShortDescription,
                Description = post.Description,
                ThumbnailUrl = post.ThumbnailUrl,
                ToDate = post.ToDate,
                IsCourse = false,
                Price = post.Price,
                RegisterUrl = post.RegisterUrl,
                VideoRecord = post.VideoRecord,
                Resources = post.Resources,
                Assignment = post.Assignment,
                Certificate = post.Certificate,
                Online = post.Online,
                Teacher = post.Teacher,
            };

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> EditMeeting(TrainingVM vm)
        {
            if (!ModelState.IsValid) { return View(vm); }
            var post = await _context.Trainings!.FirstOrDefaultAsync(x => x.Id == vm.Id);
            if (post == null)
            {
                _notification.Error("Meeting not found");
                return View();
            }

            post.Title = vm.Title;
            post.ShortDescription = vm.ShortDescription;
            post.Description = vm.Description;
            post.ThumbnailUrl = vm.ThumbnailUrl;
            post.ToDate = vm.ToDate;
            post.Price = vm.Price;
            post.RegisterUrl = vm.RegisterUrl;
            post.VideoRecord = vm.VideoRecord;
            post.Resources = vm.Resources;
            post.Assignment = vm.Assignment;
            post.Certificate = vm.Certificate;
            post.Online = vm.Online;
            post.Teacher = vm.Teacher;

            if (vm.Thumbnail != null)
            {
                post.ThumbnailUrl = UploadImage(vm.Thumbnail);
            }

            await _context.SaveChangesAsync();
            _notification.Success("Meeting updated succesfully");
            return RedirectToAction("IndexMeeting", "Training", new { area = "Admin" });
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
