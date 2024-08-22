namespace TwitterAccountsBlazorWebpage.Models
{
    public class FeedItem
    {
        public string Title { get; set; }
        public string Summary { get; set; }
        public string Link { get; set; }
        public DateTime PublishDate { get; set; }
        public bool IsVideo { get; set; }
        public string MediaUrl { get; set; }  // Property for video URL if it's a video
        public string ImageUrl { get; set; }  // Property for image URL
        public bool IsVisible { get; set; } = false;
    }
}
