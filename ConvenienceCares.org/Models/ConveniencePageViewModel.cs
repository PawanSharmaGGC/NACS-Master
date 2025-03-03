using Convenience;

namespace ConvenienceCares.Models;

public record ConveniencePageViewModel(string Title, string Description, string SectionHeader, string PageContent, string PageTrimmedPath)
{
    public static ConveniencePageViewModel GetViewModel(Page page)
    {
        return new ConveniencePageViewModel(page.Title, page.Description, page.SectionHeader, page.PageContent, GetPageLastSegment(page.SystemFields.WebPageUrlPath));
    }

    private static string GetPageLastSegment(string pageUrlPath)
    {
        if (string.IsNullOrEmpty(pageUrlPath)) return string.Empty;
        int lastSlashIndex = pageUrlPath.LastIndexOf('/');
        var pageLastSegment = lastSlashIndex >= 0 ? pageUrlPath.Substring(lastSlashIndex + 1) : pageUrlPath;
        return pageLastSegment.ToLowerInvariant();
    }
}
