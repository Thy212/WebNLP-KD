namespace WebLabAI.ViewModels
{
	public class UserVM
	{
		public string? Id { get; set; }
		public string? FirstName { get; set; }
		public string? LastName { get; set; }
		public DateTime? Birthday { get; set; }
		public string? Education { get; set; }
		public string? Email { get; set; }
		public string? Role { get; set; }
        public string? FacebookUrl { get; set; }
        public string? LinkedinUrl { get; set; }
        public string? GithubUrl { get; set; }
        public bool isAdmin { get; set; }
        public string? ThumbnailUrl { get; set; }
        public IFormFile? Thumbnail { get; set; }
    }
    
}
