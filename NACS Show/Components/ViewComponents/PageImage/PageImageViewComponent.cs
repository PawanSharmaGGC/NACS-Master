using CMS.MediaLibrary;
using Microsoft.AspNetCore.Mvc;
using NACS.Portal.Core.Services;
using NACSShow.Models;

namespace NACSShow.Components.ViewComponents;

public class PageImageViewComponent : ViewComponent
{
    private readonly IAssetItemService _assetItemService;

    public PageImageViewComponent(IAssetItemService assetItemService)
    {
        _assetItemService = assetItemService;
    }

    public IViewComponentResult Invoke(IEnumerable<AssetRelatedItem> HeaderImage, IEnumerable<AssetRelatedItem> HeaderImageMobile)
    {
        var headerImage = _assetItemService.RetrieveMediaFileImage(HeaderImage?.FirstOrDefault()).GetAwaiter().GetResult()?.URLData?.RelativePath ?? string.Empty;
        var headerImageMobile = _assetItemService.RetrieveMediaFileImage(HeaderImageMobile?.FirstOrDefault()).GetAwaiter().GetResult()?.URLData?.RelativePath ?? string.Empty;

        var pageImageViewModel = new PageImageViewModel
        {
            HeaderImage = headerImage,
            HeaderImageMobile = headerImageMobile
        };

        return View("~/Components/ViewComponents/PageImage/PageImage.cshtml", pageImageViewModel);
    }
}
