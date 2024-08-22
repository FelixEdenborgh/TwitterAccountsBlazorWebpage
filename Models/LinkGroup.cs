namespace TwitterAccountsBlazorWebpage.Models
{
    public class LinkGroup
    {
        public string Header { get; set; }
        public List<LinkItem> Links { get; set; }
        public bool IsVisible { get; set; } = true;
    }
}
