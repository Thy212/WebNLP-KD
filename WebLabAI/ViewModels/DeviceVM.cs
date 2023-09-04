using System.ComponentModel.DataAnnotations;

namespace WebLabAI.ViewModels
{
    public class DeviceVM
    {
		public int Id { get; set; }
		[Required]
		public string? DeviceName { get; set; }
		public int Rent { get; set; }
		public int Price { get; set; }
		public string? ThumbnailUrl { get; set; }
		public IFormFile? Thumbnail { get; set; }
	}
}
