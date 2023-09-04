using System.ComponentModel.DataAnnotations;

namespace WebLabAI.ViewModels
{
    public class PartnerVM
    {
        public int Id { get; set; }
        [Required]
        public string? Name { get; set; }
        public string? ThumbnailUrl { get; set; }
        [Required]
        public string? LinkWebsite { get; set; }
        public IFormFile? Thumbnail { get; set; }
    }
}
