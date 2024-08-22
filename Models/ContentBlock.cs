namespace TwitterAccountsBlazorWebpage.Models
{
    public class ContentBlock
    {
        public string Type { get; set; } // "image" or "description"
        public string Content { get; set; } // URL for image or text for description

        // Add these properties for articles
        public string Title { get; set; } // Title of the article
        public string ImageUrl { get; set; } // URL to the image associated with the article
        public string Link { get; set; } // URL to the full article
    }
}
