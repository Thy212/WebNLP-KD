using AspNetCoreHero.ToastNotification.Abstractions;
using AspNetCoreHero.ToastNotification.Notyf;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System;
using WebLabAI.Data;
using WebLabAI.Models;
using WebLabAI.Utilies;
using WebLabAI.ViewModels;

namespace WebLabAI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private INotyfService _notification { get; }

        public UserController(ApplicationDbContext context, UserManager<ApplicationUser> userManager,
                                    IWebHostEnvironment webHostEnvironment,
                                    SignInManager<ApplicationUser> signInManager,
                                    INotyfService notyfService)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _notification = notyfService;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
		public async Task<IActionResult> StaffManager()
		{
			var users = await _userManager.Users.ToListAsync();
			var vm = users.Select(x => new UserVM()
			{
				Id = x.Id,
				FirstName = x.FirstName,
				LastName = x.LastName,
                Email = x.Email,
                ThumbnailUrl = x.ThumbnailUrl,
			}).ToList();
            // assinging role
            foreach(var user in vm)
            {
                var singleUser = await _userManager.FindByIdAsync(user.Id);
                var role = await _userManager.GetRolesAsync(singleUser);
                user.Role = role.FirstOrDefault();
            }
			return View(vm.OrderBy(x => x.FirstName).ToList());
        }

		[Authorize(Roles = "Admin")]
		[HttpGet]
		public async Task<IActionResult> ResetPassword(string id)
		{
            var existingUser = await _userManager.FindByIdAsync(id);
            if(existingUser == null)
            {
                _notification.Error("User doesn't exists");
                return View();
            }
            var vm = new ResetPasswordVM()
            {
                Id = existingUser.Id,
				Email = existingUser.Email
            };
			return View(vm);
		}

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordVM vm)
        {
            if (!ModelState.IsValid) { 
                return View(vm); 
            }
            var existingUser = await _userManager.FindByIdAsync(vm.Id);
            if ( existingUser == null)
            {
                _notification.Error("User doesn't exist");
                return View(vm);
            }
            if(existingUser.Email == "admin@gmail.com")
            {
                _notification.Error("Don't change password of Super Admin");
                return View(vm);
            }
            var token = await _userManager.GeneratePasswordResetTokenAsync(existingUser);
            var result = await _userManager.ResetPasswordAsync(existingUser, token, vm.NewPassword);
            if (result.Succeeded)
            {
                _notification.Success("Password reset successful");
                return RedirectToAction(nameof(StaffManager));
            }
            return View(vm);
        }


        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult AddStaff()
        {
            return View(new AddStaffVM());
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> AddStaff(AddStaffVM vm)  // them user
        {
            if (!ModelState.IsValid) { 
                return View(vm); 
            }
            // check email
            var checkUserByEmail = await _userManager.FindByEmailAsync(vm.Email);
            if(checkUserByEmail != null)
            {
                _notification.Error("Email already exists");
                return View(vm);
            }

            //
            var applicationUser = new ApplicationUser();

            if (vm.Thumbnail != null)
            {
                applicationUser.ThumbnailUrl = UploadImage(vm.Thumbnail);
            }
            applicationUser.FirstName = vm.FisrtName;
            applicationUser.LastName = vm.LastName;
            applicationUser.Email = vm.Email;
            applicationUser.Birthday = vm.Birthday;
            applicationUser.Education = vm.Education;
            //FacebookUrl = vm.FacebookUrl,
            applicationUser.LinkedinUrl = vm.LinkedinUrl;
            applicationUser.GithubUrl = vm.GithubUrl;
            applicationUser.isAdmin = vm.IsAdmin;
            //ThumbnailUrl = vm.ThumbnailUrl,
            applicationUser.UserName = vm.Email;
            applicationUser.Position = vm.Position;
            
            var result = await _userManager.CreateAsync(applicationUser, vm.Password);
            if (result.Succeeded)
            {
                if (vm.IsAdmin)
                {
                    await _userManager.AddToRoleAsync(applicationUser, WebsiteRoles.WebsiteAdmin);
                }
                else
                {
                    await _userManager.AddToRoleAsync(applicationUser, WebsiteRoles.WebsiteAuthor);
                }
                _notification.Success("User created successfully");
                return RedirectToAction("StaffManager", "User", new { area = "Admin" });
            }
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            var userWillDelete = await _userManager.FindByIdAsync(id);
            //var user = await _context.ApplicationUser!.FirstOrDefaultAsync(x => x.Id == id);

            var loggedInUser = await _userManager.Users.FirstOrDefaultAsync(x => x.Email == User.Identity!.Name); // check who loging
            var loggedInUserRole = await _userManager.GetRolesAsync(loggedInUser!); // xuat ra quyen cua nguoi da dang nhap

            var userHaveApost = await _context.Posts!.Where(x => x.ApplicationUserId == id).ToListAsync();

            var userHaveATraining = await _context.Trainings!.Where(x => x.ApplicationUserId == id).ToListAsync();
            var userHaveAResearch = await _context.Researchs!.Where(x => x.ApplicationUserId == id).ToListAsync();
            if(userHaveATraining != null || userHaveApost != null || userHaveAResearch != null)
            {
                if (userHaveATraining != null)
                {
                    if (userWillDelete?.Email == "admin@gmail.com")
                    {
                        _notification.Error("Super Admin Deletion Isn't Allowed");
                        return RedirectToAction("StaffManager", "User", new { area = "Admin" });
                    }

                    foreach (var train in userHaveATraining)
                    {
                        train.ApplicationUserId = loggedInUser!.Id;
                    }

                    await _context.SaveChangesAsync();
                    
                }
                if (userHaveAResearch != null)
                {
                    if (userWillDelete?.Email == "admin@gmail.com")
                    {
                        _notification.Error("Super Admin Deletion Isn't Allowed");
                        return RedirectToAction("StaffManager", "User", new { area = "Admin" });
                    }
                    //var post = new Post();
                    foreach (var resea in userHaveAResearch)
                    {
                        resea.ApplicationUserId = loggedInUser!.Id;
                    }

                    await _context.SaveChangesAsync();
                    
                }
                if (userHaveApost != null)
                {
                    if (userWillDelete?.Email == "admin@gmail.com")
                    {
                        _notification.Error("Super Admin Deletion Isn't Allowed");
                        return RedirectToAction("StaffManager", "User", new { area = "Admin" });
                    }
                    //var post = new Post();
                    foreach (var post in userHaveApost)
                    {
                        post.ApplicationUserId = loggedInUser!.Id;
                    }

                    await _context.SaveChangesAsync();
                    
                }

                await _userManager.DeleteAsync(userWillDelete!);
                _notification.Success("User Deleted");
                return RedirectToAction("StaffManager", "User", new { area = "Admin" });
            }
             

            if (userWillDelete == null)
            {
                _notification.Error($"User with Id = {id} cannot be found");
                return RedirectToAction("StaffManager", "User", new { area = "Admin" });
            }

            if (userWillDelete?.Email == "admin@gmail.com")
            {
                //_notification.Success("Super Admin Deletion Isn't Allowed");
                _notification.Error("Super Admin Deletion Isn't Allowed");
                return RedirectToAction("StaffManager", "User", new { area = "Admin" });
            }
            else if (loggedInUserRole[0] == WebsiteRoles.WebsiteAdmin && userWillDelete != null && userWillDelete?.Email != loggedInUser?.Email) // check who loging, co phai la admin khong, OR id cua ng dang loging có == vs id cua ng dung ko
            {
                await _userManager.DeleteAsync(userWillDelete!);
                //await _userManager.SaveChangesAsync();

                _notification.Success("User Deleted");
                return RedirectToAction("StaffManager", "User", new { area = "Admin" });
            }
            else if(loggedInUserRole[0] == WebsiteRoles.WebsiteAdmin && userWillDelete?.Email == loggedInUser?.Email)
            {
                _notification.Success("You won't be able to sign in if you delete yourself");
                await _userManager.DeleteAsync(userWillDelete!);
                return RedirectToAction("Index", "Home", new { area = "" });
            }
            
            return View();

        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var userEdit = await _userManager.FindByIdAsync(id);
            //var userEdit = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == user.UserName);
            if (userEdit == null)
            {
                _notification.Error("User Not Found");
                return View();
            }

            var loggedInUser = await _userManager.Users.FirstOrDefaultAsync(x => x.Email == User.Identity!.Name); // check who loging
            var loggedInUserRole = await _userManager.GetRolesAsync(loggedInUser!); // xuat ra quyen cua nguoi da dang nhap

            if (loggedInUserRole[0] != WebsiteRoles.WebsiteAdmin)
            {
                _notification.Error("You Aren't Admin");
                return RedirectToAction("Index");
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
                IsAdmin = userEdit.isAdmin,
                ThumbnailUrl = userEdit.ThumbnailUrl,
                Position = userEdit.Position,
            };

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditStaff vm)
        {
            if (!ModelState.IsValid) { 
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
            userEdit.isAdmin = vm.IsAdmin;
            userEdit.Position = vm.Position;

            if (vm.Thumbnail != null)
            {
                userEdit.ThumbnailUrl = UploadImage(vm.Thumbnail);
            }

            // Kiểm tra xem người dùng có thay đổi quyền hay không
            bool roleChanged = false;
            if (vm.IsAdmin == true && !await _userManager.IsInRoleAsync(userEdit, WebsiteRoles.WebsiteAdmin)) // not admin = author
            { // is admin and role hien tai khong phai admin
                roleChanged = true;
            }
            else if (vm.IsAdmin == false && !await _userManager.IsInRoleAsync(userEdit, WebsiteRoles.WebsiteAuthor))  // not author = admin
            { // is not admin and role hien tai la admin
                roleChanged = true;
            }

            // Nếu người dùng thay đổi quyền
            if (roleChanged)
            {
                // Xóa tất cả các quyền hiện tại của người dùng
                var currentRoles = await _userManager.GetRolesAsync(userEdit);
                await _userManager.RemoveFromRolesAsync(userEdit, currentRoles);

                // Thêm quyền mới cho người dùng
                if (vm.IsAdmin == true)
                {
                    await _userManager.AddToRoleAsync(userEdit, WebsiteRoles.WebsiteAdmin);
                }
                else
                {
                    await _userManager.AddToRoleAsync(userEdit, WebsiteRoles.WebsiteAuthor);
                }
            }

            await _userManager.UpdateAsync(userEdit);
            _notification.Success("User updated succesfully");
            return RedirectToAction("StaffManager", "User", new { area = "Admin" });
        }

        [HttpGet("Login")]
        public IActionResult Login()
        {
            //remember login
            if (!HttpContext.User.Identity!.IsAuthenticated)
            {
                return View(new LoginVM());
            }
            return RedirectToAction("Index", "User", new { area = "Admin" });
        }

		[HttpPost("Login")]
		public async Task<IActionResult> Login(LoginVM vm)
		{
			var existingUser = await _userManager.Users.FirstOrDefaultAsync(x => x.Email == vm.Email);

			if (existingUser == null)
			{
				_notification.Error("User doesn't exist");
				return View(vm);
			}

			if (await _userManager.IsLockedOutAsync(existingUser))
			{
				_notification.Error("Your account has been locked for 5 minutes due to too many failed login attempts.");
				return View(vm);
			}

			var verifyPassword = await _userManager.CheckPasswordAsync(existingUser, vm.Password);

			if (!verifyPassword)
			{
				await _userManager.AccessFailedAsync(existingUser);
				_notification.Error("Password doesn't match");
				return View(vm);
			}

			await _signInManager.PasswordSignInAsync(vm.Email, vm.Password, vm.RememberMe, true);
			await _userManager.ResetAccessFailedCountAsync(existingUser);
			_notification.Success("Login Successful");
			return RedirectToAction("Index", "User", new { area = "Admin" });
		}


		/*[HttpPost("Login")]
        public async Task<IActionResult> Login(LoginVM vm)
        {
            var existingUser = await _userManager.Users.FirstOrDefaultAsync(x => x.Email == vm.Email);

            if (existingUser == null )
            {
                _notification.Error("User doesn't exist");
                return View(vm);
            }

            var verifyPassword = await _userManager.CheckPasswordAsync(existingUser, vm.Password);

            if (!verifyPassword)
            {
                _notification.Error("Password doesn't match");
                return View(vm);
            }

            await _signInManager.PasswordSignInAsync(vm.Email, vm.Password, vm.RememberMe, true);
            _notification.Success("Login Successful");
            return RedirectToAction("Index", "User", new { area = "Admin" });
            
        }*/

        
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> ChangePassword(string id)
        {
            var existingUser = await _userManager.FindByIdAsync(id);
            if (existingUser == null)
            {
                _notification.Error("User doesn't exists");
                return View();
            }
            var vm = new ChangePasswordVM()
            {
                Id = existingUser.Id,
                Email = existingUser.Email
            };
            return View(vm);
        }

        
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordVM vm)
        {
            if (!ModelState.IsValid) { 
                return View(vm); 
            }
            var existingUser = await _userManager.Users.FirstOrDefaultAsync(x => x.Email == User.Identity!.Name); // check who loging
            if (existingUser == null)
            {
                _notification.Error("User doesn't exist");
                return View(vm);
            }

            var result = await _userManager.ChangePasswordAsync(existingUser, vm.OldPassword, vm.NewPassword);
            if (result.Succeeded)
            {
                _notification.Success("Password changed successfully");
                return RedirectToAction(nameof(Index));
            }
            else
            {
                _notification.Error("Old password is incorrect");
                return View(vm);
            }
        }

        [HttpPost]
        [Authorize]
        public IActionResult Logout()
        {
            _signInManager.SignOutAsync();
            _notification.Success("You are logged out");
            return RedirectToAction("Index", "Home", new {area = ""});
        }

        

        [HttpGet("AccessDenied")]
        [Authorize]
        public IActionResult AccessDenied()
        {
            return View();
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
