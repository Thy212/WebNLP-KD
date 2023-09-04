using System.ComponentModel.DataAnnotations;
using WebLabAI.Models;

namespace WebLabAI.ViewModels
{
    public class PageVM
    {
        public int Id { get; set; }
        [Required]
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? ShortDescription { get; set; }
        public string? ThumbnailUrl { get; set; }
        public IFormFile? Thumbnail { get; set; }

        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }

        public List<ApplicationUser>? ApplicationUser { get; set; }

        public List<Post>? Posts { get; set; }
    }
}
