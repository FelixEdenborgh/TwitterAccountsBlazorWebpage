using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Xml;
using TwitterAccountsBlazorWebpage.Models;

namespace TwitterAccountsBlazorWebpage.Rss_Feed_Import
{
    public class RiksArticlesRssFeedService
    {
        public List<FeedItem> GetFeedItems(string rssFeedUrl)
        {
            using var reader = XmlReader.Create(rssFeedUrl);
            var feed = SyndicationFeed.Load(reader);

            return feed?.Items.Select(item => new FeedItem
            {
                Title = item.Title.Text,
                Summary = item.Summary?.Text,
                Link = item.Links.FirstOrDefault()?.Uri.ToString(),
                PublishDate = item.PublishDate.DateTime,
                IsVideo = IsVideoContent(item),
                MediaUrl = ExtractMediaUrl(item),
                ImageUrl = ExtractImageUrl(item)
            }).ToList();
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

            return mediaUrl;
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

            return imageUrl;
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
}
