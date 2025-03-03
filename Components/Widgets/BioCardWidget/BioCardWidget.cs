using CMS.Core;
using CMS.Websites;
using Convenience.org.Components.Widgets.BioCardWidget;
using Convenience.org.Models;
using Convenience.org.Repositories.Interfaces;
using Kentico.Content.Web.Mvc.Routing;
using Kentico.PageBuilder.Web.Mvc;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

[assembly: RegisterWidget(identifier: BioCardWidget.IDENTIFIER,
    propertiesType: typeof(BioCardWidgetProperties), viewComponentType: typeof(BioCardWidget),
    name: "Bio Card", Description = "Bio Card",
    IconClass = "icon-user", AllowCache = true)]

namespace Convenience.org.Components.Widgets.BioCardWidget;

public class BioCardWidget : ViewComponent
{
    public const string IDENTIFIER = "BioCardWidget";
    private readonly IPersonBioRepository personBioRepository;
    private readonly IWebPageUrlRetriever urlRetriever;
    private readonly IPreferredLanguageRetriever languageRetriever;
    private readonly IEventLogService eventLogService;

    public BioCardWidget(IPersonBioRepository personBioRepository, IWebPageUrlRetriever urlRetriever,
        IPreferredLanguageRetriever languageRetriever, IEventLogService eventLogService)
    {
        this.personBioRepository = personBioRepository;
        this.urlRetriever = urlRetriever;
        this.languageRetriever = languageRetriever;
        this.eventLogService = eventLogService;
    }
    public async Task<IViewComponentResult> InvokeAsync(BioCardWidgetProperties widgetProperties)
    {
        var model = PersonBioViewModel.GetViewModel();

        try
        {
            if (widgetProperties != null)
            {
                // Gets the GUIDs from the annotated property
                var pageGuids = widgetProperties?.SelectedPerson.Select(i => i.WebPageGuid).ToList();

                if (pageGuids != null && pageGuids.Any())
                {
                    var languageName = languageRetriever.Get();
                    var pageUrl = await urlRetriever.Retrieve(pageGuids.FirstOrDefault(), languageName);

                    var person = personBioRepository.GetPersonBioRepository(pageGuids);
                    model.PersonBioItem = person;
                    model.CTA.ButtonURL = pageUrl.RelativePath;
                    model.CTA.ButtonText = string.IsNullOrEmpty(widgetProperties?.CTAText) ? "View Bio Page" : widgetProperties?.CTAText;
                }
            }
        }
        catch (Exception ex)
        {
            eventLogService.LogException(nameof(BioCardWidget), nameof(InvokeAsync), ex);
        }
        return View("~/Components/Widgets/BioCardWidget/BioCard.cshtml", model);
    }
}
