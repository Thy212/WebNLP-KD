using System.ComponentModel.DataAnnotations;

namespace WebLabAI.ViewModels
{
	public class ChangePasswordVM
	{
		public string? Id { get; set; }
		public string? Email { get; set; }
		//public string? UserName { get; set; }
		[Required]
		public string? NewPassword { get; set; }
		[Compare(nameof(NewPassword))]
		[Required]
		public string? ConfirmPassword { get; set; }
		[Required]
		public string? OldPassword { get; set; }
	}
}
