using System.ComponentModel.DataAnnotations;

namespace WebLabAI.ViewModels
{
    public class ContactFormVM
    {
        public int Id { get; set; }
        [Required]
        public string? FirstName { get; set; }
        [Required]
        public string? LastName { get; set; }
        [Required]
        public string? Email { get; set; }
        [Required]
        public string? Phone { get; set; }
        public string? Message { get; set; }
        public DateTime DateSend { get; set; }
    }
}
