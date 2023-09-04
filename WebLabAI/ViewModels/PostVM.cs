
using System.ComponentModel.DataAnnotations;
using WebLabAI.Models;

namespace WebLabAI.ViewModels
{
    public class PostVM
    {
        public int Id { get; set; }
        [Required]
        public string? Title { get; set; }
        [Required]
        public string? ShortDescription { get; set; }
        public string? AuthorName { get; set; }
        public DateTime CreateDate { get; set; }
        public string? ThumbnailUrl { get; set; }

    }
}
