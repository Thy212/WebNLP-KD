namespace WebLabAI.ViewModels
{
	public class ResearchVM
	{
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? AuthorName { get; set; }
        public DateTime CreateDate { get; set; }
        public string? ThumbnailUrl { get; set; }
        public string? ShortDescription { get; set; }
        public string? LinkWebsite { get; set; }
    }
}
