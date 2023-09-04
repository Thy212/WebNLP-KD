using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using WebLabAI.Data;
using WebLabAI.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace WebLabAI.Controllers
{
    public class CourseMeetController : Controller
    {
        private readonly ApplicationDbContext _context;
        public INotyfService _notification { get; }

        public CourseMeetController(ApplicationDbContext context, INotyfService notification)
        {
            _context = context;
            _notification = notification;
        }

        [HttpGet("[controller]/{slug}")]
        public IActionResult CouMee(string slug)
        {
            if (slug == "")
            {
                _notification.Error("Post not found");
                return View();
            }
            var post = _context.Trainings!.Include(x => x.ApplicationUser).FirstOrDefault(x => x.Slug == slug);
            if (post == null)
            {
                _notification.Error("Post not found");
                return View();
            }
            var vm = new BlogTrainingVM()
            {
                Id = post.Id,
                Title = post.Title,
                ToDate = post.ToDate,
                ThumbnailUrl = post.ThumbnailUrl,
                Description = post.Description,
                ShortDescription = post.ShortDescription,
                IsCourse = post.IsCourse,
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
    }
}
