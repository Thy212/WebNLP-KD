namespace WebLabAI.Models
{
	public class Research
	{
		public int Id { get; set; }
		public string? Title { get; set; }
		public string? ShortDescription { get; set; }
		public DateTime CreateDate { get; set; } // năm xuất bản ấn phẩm
		public string? Slug { get; set; }
		public string? ThumbnailUrl { get; set; }
		public string? AuthorName { get; set; }
        public ApplicationUser? ApplicationUser { get; set; }
        public string? ApplicationUserId { get; set; }
		public string? LinkWebsite { get; set; }
    }
}
