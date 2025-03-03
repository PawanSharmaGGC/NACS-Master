using CMS.MediaLibrary;
using Microsoft.AspNetCore.Mvc;
using NACS.Portal.Core.Services;
using NACSShow.Models;

namespace NACSShow.Components.ViewComponents;

public class NewsArticleTitleViewComponent : ViewComponent
{
    private readonly IAssetItemService assetItemService;

    public NewsArticleTitleViewComponent(IAssetItemService assetItemService)
    {
        this.assetItemService = assetItemService;
    }

    public IViewComponentResult Invoke(string title, string description, DateTime date, IEnumerable<AssetRelatedItem> headerImage)
    {
        var headerImageUrl = string.Empty;
        if (headerImage != null && headerImage.Any())
        {
            var firstHeaderImage = headerImage.FirstOrDefault();
            if (firstHeaderImage != null)
            {
                var mediaFileImage = assetItemService?.RetrieveMediaFileImage(firstHeaderImage).GetAwaiter().GetResult();
                if (mediaFileImage != null)
                {
                    headerImageUrl = mediaFileImage.URLData.RelativePath;
                }
            }
        }

        return View("~/Components/ViewComponents/NewsArticleTitle/NewsArticleTitle.cshtml",
            new NewsArticleTitleViewModel
            {
                Title = title,
                Description = description,
                Date = date != DateTime.MinValue ? date.ToString("MMMM dd, yyyy") : "",
                HeaderImage = headerImageUrl
            });
    }
}
