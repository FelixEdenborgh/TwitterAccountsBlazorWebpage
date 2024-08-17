namespace TwitterAccountsBlazorWebpage.Models
{
    public class LinkItem
    {
        public string Name { get; set; }
        public bool IsVisible { get; set; } = false; // Default to not visible
        public string Url { get; set; }
        public List<ContentBlock> ContentBlocks { get; set; } = new List<ContentBlock>();
    }
}
