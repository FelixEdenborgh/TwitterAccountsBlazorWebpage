using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.ServiceModel.Syndication;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.Extensions.Logging;

namespace TwitterAccountsBlazorWebpage.Rss_Feed_Import
{
    public class TwitterRSSFeedService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<TwitterRSSFeedService> _logger;

        public TwitterRSSFeedService(HttpClient httpClient, ILogger<TwitterRSSFeedService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        // Get tweets, count is how many you should get
        public async Task<List<SyndicationItem>> GetLatestTweetsAsync(string feedUrl, int count = 20)
        {
            try
            {
                _logger.LogInformation("Fetching RSS feed from {FeedUrl}", feedUrl);
                using var responseStream = await _httpClient.GetStreamAsync(feedUrl);
                using var xmlReader = XmlReader.Create(responseStream);
                var feed = SyndicationFeed.Load(xmlReader);
                _logger.LogInformation("Successfully fetched and parsed RSS feed with {ItemCount} items", feed.Items.Count());

                return feed.Items.Take(count).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to fetch or parse RSS feed");
                return new List<SyndicationItem>();
            }
        }

        public List<string> ExtractLinks(string tweetContent)
        {
            var links = new List<string>();

            _logger.LogInformation("Extracting links from tweet content: {TweetContent}", tweetContent);

            // YouTube link regex
            var youtubeMatch = Regex.Match(tweetContent, @"(https?://www\.youtube\.com/watch\?v=[\w-]+)");
            if (youtubeMatch.Success)
            {
                links.Add(youtubeMatch.Value);
                _logger.LogInformation("Found YouTube link: {YouTubeLink}", youtubeMatch.Value);
            }

            // General video link regex
            var generalMatch = Regex.Match(tweetContent, @"(https?://[\w./-]+\.(mp4|webm|ogg))");
            if (generalMatch.Success)
            {
                links.Add(generalMatch.Value);
                _logger.LogInformation("Found video link: {VideoLink}", generalMatch.Value);
            }

            return links;
        }
    }
}
