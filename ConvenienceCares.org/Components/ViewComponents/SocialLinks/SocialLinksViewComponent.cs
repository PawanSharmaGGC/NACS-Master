using ConvenienceCares.Repository;
using Kentico.Content.Web.Mvc.Routing;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ConvenienceCares.Components.ViewComponents.SocialLinks;

public class SocialLinksViewComponent : ViewComponent
{
    private readonly SocialLinkRepository socialLinkRepository;
    private readonly IPreferredLanguageRetriever currentLanguageRetriever;

    public SocialLinksViewComponent(SocialLinkRepository socialLinkRepository, IPreferredLanguageRetriever currentLanguageRetriever)
    {
        this.socialLinkRepository = socialLinkRepository;
        this.currentLanguageRetriever = currentLanguageRetriever;
    }

    /// <summary>
    /// Retrun social media links
    /// </summary>
    /// <returns></returns>
    public async Task<IViewComponentResult> InvokeAsync()
    {
        var languageName = currentLanguageRetriever.Get();

        var socialLinks = await socialLinkRepository.GetSocialLinks(languageName, HttpContext.RequestAborted);

        return View("~/Components/ViewComponents/SocialLinks/SocialLink.cshtml", socialLinks);
    }
}
