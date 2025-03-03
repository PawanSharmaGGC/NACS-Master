using CMS.ContentEngine;
using CMS.MediaLibrary;
using CMS.Websites;
using CMS.Websites.Routing;
using Microsoft.AspNetCore.Mvc;
using NACS.Portal.Core.Services;

namespace NACSShow.Components.ViewComponents;

public class NSHeaderImageInheritViewComponent : ViewComponent
{
    private readonly IAssetItemService assetItemService;
    private readonly IWebsiteChannelContext channelContext;
    private readonly IContentQueryExecutor executor;


    public NSHeaderImageInheritViewComponent(IAssetItemService assetItemService, IWebsiteChannelContext channelContext,
        IContentQueryExecutor executor)
    {
        this.assetItemService = assetItemService;
        this.channelContext = channelContext;
        this.executor = executor;
    }


    public async Task<IViewComponentResult> InvokeAsync(Page currentPage)
    {
        var image = currentPage.HeaderImage;

        // If there is no image, search for the header image in parent pages
        if (image == null || !image.Any())
        {
            image = await GetParentPageHeaderImageAsync(currentPage);
        }

        var headerImage = await GetHeaderImageUrlAsync(image?.FirstOrDefault());
        return View("~/Components/ViewComponents/NSHeaderImageInherit/NSHeaderImageInherit.cshtml", headerImage);
    }

    private async Task<IEnumerable<AssetRelatedItem>> GetParentPageHeaderImageAsync(Page currentPage)
    {
        object page = currentPage;
        int? pageParentId = currentPage.SystemFields?.WebPageItemParentID;

        while (page != null && pageParentId.HasValue)
        {
            var parentPage = await GetParentPageAsync(pageParentId.Value);

            if (parentPage == null)
            {
                break;
            }

            var image = GetHeaderImageFromPage(parentPage);
            if (image != null && image.ToList().Count>0)
            {
                return image;
            }

            pageParentId = GetParentPageId(parentPage);
            page = parentPage;
        }

        return Enumerable.Empty<AssetRelatedItem>();
    }

    private async Task<IContentItemFieldsSource> GetParentPageAsync(int parentId)
    {
        var builder = new ContentItemQueryBuilder()
            .ForContentTypes(parameters =>
            {
                parameters.ForWebsite(channelContext.WebsiteChannelName)
                          .WithContentTypeFields();
            })
            .Parameters(parameters =>
            {
                parameters.Where(w => w.WhereEquals("WebPageItemID", parentId));
            });

        var pageContent = await executor.GetMappedResult<IContentItemFieldsSource>(builder);
        return pageContent.FirstOrDefault();
    }

    private IEnumerable<AssetRelatedItem> GetHeaderImageFromPage(IContentItemFieldsSource parentPage)
    {
        var propertyInfo = parentPage.GetType().GetProperty("HeaderImage");
        var image = propertyInfo?.GetValue(parentPage) as IEnumerable<AssetRelatedItem>;

        return image;
    }

    private int? GetParentPageId(IContentItemFieldsSource parentPage)
    {
        var propertySystemFields = parentPage.GetType().GetProperty("SystemFields");
        var systemFields = propertySystemFields?.GetValue(parentPage) as WebPageFields;

        return systemFields?.WebPageItemParentID;
    }

    private async Task<string> GetHeaderImageUrlAsync(AssetRelatedItem image)
    {
        if (image == null)
        {
            return null;
        }

        var mediaFile = await assetItemService.RetrieveMediaFileImage(image);
        return mediaFile?.URLData?.RelativePath;
    }
}
