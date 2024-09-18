using System.ServiceModel.Syndication;
using System.Xml;
using TwitterAccountsBlazorWebpage.Models;

public class ArticlesRssFeedService
{
    public List<FeedItem> GetFeedItems(string rssFeedUrl)
    {
        try
        {
            using var reader = XmlReader.Create(rssFeedUrl);
            var feed = SyndicationFeed.Load(reader);

            // Kontrollera om feed är null
            if (feed == null)
            {
                Console.WriteLine($"Failed to load feed from URL: {rssFeedUrl}");
                return new List<FeedItem>(); // Returnera en tom lista om feed är null
            }

            // Skapa feed items och hantera null-kontroller på varje egenskap
            return feed.Items.Select(item => new FeedItem
            {
                Title = item.Title?.Text ?? "No Title", // Kontrollera om Title är null
                Summary = item.Summary?.Text ?? "No Summary", // Kontrollera om Summary är null
                Link = item.Links.FirstOrDefault()?.Uri.ToString() ?? "#", // Kontrollera om Links är null
                PublishDate = item.PublishDate.DateTime,
                IsVideo = IsVideoContent(item),
                MediaUrl = ExtractMediaUrl(item),
                ImageUrl = ExtractImageUrl(item)
            }).ToList();
        }
        catch (Exception ex)
        {
            // Logga eventuellt felet och returnera en tom lista
            Console.WriteLine($"Error loading RSS feed from {rssFeedUrl}: {ex.Message}");
            return new List<FeedItem>();
        }
    }

    private bool IsVideoContent(SyndicationItem item)
    {
        // Logic to determine if the item is a video based on media content
        return item.ElementExtensions.Any(ext => ext.OuterName == "video" || item.Links.Any(link => link.MediaType == "video/mp4"));
    }

    private string ExtractMediaUrl(SyndicationItem item)
    {
        // Extract the media URL (preferably video) from the RSS item
        var mediaUrl = item.ElementExtensions
            .FirstOrDefault(ext => ext.OuterName == "media:content" && ext.GetObject<XmlElement>().GetAttribute("medium") == "video")
            ?.GetObject<XmlElement>()?.GetAttribute("url");

        if (string.IsNullOrEmpty(mediaUrl))
        {
            mediaUrl = item.Links.FirstOrDefault(link => link.MediaType?.StartsWith("video/") == true)?.Uri.ToString();
        }

        return mediaUrl ?? string.Empty;
    }

    private string ExtractImageUrl(SyndicationItem item)
    {
        // Extract the image URL from the RSS item
        var imageUrl = item.ElementExtensions
            .FirstOrDefault(ext => ext.OuterName == "media:thumbnail" || ext.OuterName == "media:content" && ext.GetObject<XmlElement>().GetAttribute("medium") == "image")
            ?.GetObject<XmlElement>()?.GetAttribute("url");

        if (string.IsNullOrEmpty(imageUrl))
        {
            imageUrl = item.Links.FirstOrDefault(link => link.MediaType?.StartsWith("image/") == true)?.Uri.ToString();
        }

        return imageUrl ?? string.Empty;
    }

    public List<FeedItem> GetArticles(List<FeedItem> feedItems)
    {
        return feedItems.Where(item => !item.IsVideo).ToList();
    }

    public List<FeedItem> GetVideos(List<FeedItem> feedItems)
    {
        return feedItems.Where(item => item.IsVideo).ToList();
    }
}
