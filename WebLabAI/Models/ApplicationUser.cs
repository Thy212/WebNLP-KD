using Microsoft.AspNetCore.Identity;

namespace WebLabAI.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime Birthday { get; set; }
        public string? Education { get; set; }
        public bool isAdmin { get; set; } // khong phai admin thi la thanh vien
        //public string? FacebookUrl { get; set; }
        public string? LinkedinUrl { get; set; }
        public string? GithubUrl { get; set; }
        public string? Position { get; set; }
        public List<Post>? Posts { get; set; }
        public List<Page>? Pages { get; set; }

        public string? ThumbnailUrl { get; set; }
    }
}
