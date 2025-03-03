using CMS.Core;
using Convenience.org.Components.Widgets.PriceCard;
using Convenience.org.Models;
using Kentico.PageBuilder.Web.Mvc;
using Microsoft.AspNetCore.Mvc;
using System;

[assembly: RegisterWidget(identifier: PriceCardWidget.IDENTIFIER,
    propertiesType: typeof(PriceCardWidgetProperties), viewComponentType: typeof(PriceCardWidget),
    name: "Price Card", Description = "Price Card",
    IconClass = "icon-dollar-sign", AllowCache = true)]

namespace Convenience.org.Components.Widgets.PriceCard;

public class PriceCardWidget : ViewComponent
{
    public const string IDENTIFIER = "PriceCardWidget";
    private readonly IEventLogService _eventLogService;

    public PriceCardWidget(IEventLogService eventLogService)
    {
        _eventLogService = eventLogService;
    }

    public IViewComponentResult Invoke(ComponentViewModel<PriceCardWidgetProperties> widgetProperties)
    {

        try
        {
            var model = PriceCardViewModel.GetViewModel(widgetProperties.Properties);
            return View("~/Components/Widgets/PriceCard/PriceCard.cshtml", model);

        }
        catch (Exception ex)
        {
            _eventLogService.LogException(nameof(PriceCardWidget), nameof(Invoke), ex);
        }
        return View("~/Components/Widgets/PriceCard/PriceCard.cshtml", new PriceCardViewModel());
    }
}
