using CMS.MediaLibrary;

namespace NACSShow.Models;

public class DailyNewsArticleViewModel
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public IEnumerable<AssetRelatedItem> HeaderImage { get; set; } = [];
    public string PageContent { get; set; } = string.Empty;

}
