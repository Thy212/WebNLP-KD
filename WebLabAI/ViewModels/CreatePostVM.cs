using System.ComponentModel.DataAnnotations;
using WebLabAI.Models;

namespace WebLabAI.ViewModels
{
	public class CreatePostVM
	{
		public int Id { get; set; }
		[Required]
		public string? Title { get; set; }
		public string? ShortDescription { get; set; }
		public string? Description { get; set; }
		public string? ApplicationUserId { get; set; }
		public string? ThumbnailUrl { get; set; }
		public IFormFile? Thumbnail { get; set; }
		public string? AuthorName { get; set; }

        [DataType(DataType.Date, ErrorMessage = "Date is required")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime CreateDate { get; set; }
		public string? LinkWebsite { get; set; }
	}
}
