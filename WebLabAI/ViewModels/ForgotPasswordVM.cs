using System.ComponentModel.DataAnnotations;

namespace WebLabAI.ViewModels
{
    public class ForgotPasswordVM
    {
        public int Id { get; set; }
        [Required]
        [EmailAddress]
        public string? Email { get; set; }

    }
}
