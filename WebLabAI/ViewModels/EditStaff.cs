using System.ComponentModel.DataAnnotations;

namespace WebLabAI.ViewModels
{
    public class EditStaff
    {
        [Required]
        public string? FisrtName { get; set; }
        [Required]
        public string? LastName { get; set; }
        [Required]
        [EmailAddress]
        public string? Email { get; set; }
        
        public string? UserName { get; set; }

        public DateTime Birthday { get; set; }

        public string? Education { get; set; }

        public string? LinkedinUrl { get; set; }

        public string? GithubUrl { get; set; }

        public bool IsAdmin { get; set; }

        public string? Position { get; set; }

        public string? ThumbnailUrl { get; set; }

        public IFormFile? Thumbnail { get; set; }
    }
}
