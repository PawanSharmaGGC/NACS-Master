using ConvenienceCares.Models;
using ConvenienceCares.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ConvenienceCares.Components.ViewComponents.SocialMetaTags;

public class SocialMetaTagViewComponent(SocialMetaTagService metaService) : ViewComponent
{
    private readonly SocialMetaTagService metaService = metaService;

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var meta = await metaService.GetMeta();

        var vm = new WebpageCustomMetaViewModel(meta);

        return View("~/Components/ViewComponents/SocialMetaTags/SocialMetaTag.cshtml", vm);
    }
}
