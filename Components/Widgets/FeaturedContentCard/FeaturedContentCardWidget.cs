using Convenience.org.Components.Widgets.FeaturedContentCard;
using Convenience.org.Models;
using Convenience.org.Repositories.Interfaces;
using Kentico.PageBuilder.Web.Mvc;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CMS.Helpers;
using CMS.Core;

[assembly: RegisterWidget(identifier: FeaturedContentCardWidget.IDENTIFIER,
    propertiesType: typeof(FeaturedContentCardProperties), viewComponentType: typeof(FeaturedContentCardWidget),
    name: "Featured Content Card", Description = "Featured Content Card",
    IconClass = "icon-feature", AllowCache = true)]


namespace Convenience.org.Components.Widgets.FeaturedContentCard;

public class FeaturedContentCardWidget : ViewComponent
{
    public const string IDENTIFIER = "FeaturedContentCard";
    private readonly IFeaturedCardRepository featuredCardRepository;
    private readonly IEventLogService _eventLogService;

    public FeaturedContentCardWidget(IFeaturedCardRepository featuredCardRepository, IEventLogService eventLogService)
    {
        this.featuredCardRepository = featuredCardRepository;
        _eventLogService = eventLogService;
    }

    public async Task<IViewComponentResult> InvokeAsync(ComponentViewModel<FeaturedContentCardProperties> widgetProperties)
    {
        var model = FeaturedContentCardViewModel.GetViewModel();

        try
        {
            List<Guid> pageGuids = widgetProperties?.Properties?.CardDetail?
                                                        .Select(i => i.WebPageGuid)
                                                        .ToList();

            if (pageGuids != null && pageGuids.Any())
            {
                model = await featuredCardRepository.GetFeaturedCardRepositoryAsync(pageGuids);
                model.Eyebrow.Title = ValidationHelper.GetString(widgetProperties.Properties.Heading, string.Empty);
                model.CTA.ButtonText = string.IsNullOrEmpty(widgetProperties?.Properties.CTAText) ? model.CTA.ButtonText : widgetProperties?.Properties.CTAText;
            }
        }
        catch (Exception ex)
        {
            _eventLogService.LogException(nameof(FeaturedContentCardWidget), nameof(InvokeAsync), ex);
        }
        return View("~/Components/Widgets/FeaturedContentCard/FeaturedContentCard.cshtml", model);
    }
}
