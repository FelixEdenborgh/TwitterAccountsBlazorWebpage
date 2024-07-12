using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;
using TwitterAccountsBlazorWebpage.Models;

public class RssFeedService
{
    private readonly HttpClient _httpClient;

    public RssFeedService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IEnumerable<Product>> GetProductsAsync()
    {
        var response = await _httpClient.GetStringAsync("https://rss.app/feeds/iD1r5QRdw0njjdkI.xml");
        var products = ParseRssFeed(response);
        return products;
    }

    private IEnumerable<Product> ParseRssFeed(string rssContent)
    {
        var xdoc = XDocument.Parse(rssContent);
        var items = xdoc.Descendants("item");
        var products = new List<Product>();

        XNamespace media = "http://search.yahoo.com/mrss/";

        foreach (var item in items)
        {
            var product = new Product
            {
                Title = item.Element("title")?.Value,
                Link = item.Element("link")?.Value,
                ImageUrl = item.Element(media + "content")?.Attribute("url")?.Value
            };
            products.Add(product);
        }

        return products;
    }
}
