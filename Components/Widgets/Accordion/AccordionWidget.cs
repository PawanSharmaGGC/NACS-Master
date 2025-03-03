using Convenience.org.Components.Widgets.Accordion;
using Kentico.PageBuilder.Web.Mvc;
using Microsoft.AspNetCore.Mvc;


[assembly: RegisterWidget(identifier: AccordionWidget.IDENTIFIER,
    propertiesType: typeof(AccordionWidgetProperties), viewComponentType: typeof(AccordionWidget),
    name: "Accordion Widget", Description = "Accordion Widget",
    IconClass = "icon-accordion", AllowCache = true)]

namespace Convenience.org.Components.Widgets.Accordion;

public class AccordionWidget : ViewComponent
{
    public const string IDENTIFIER = "Convenience.AccordionWidget";

    public IViewComponentResult Invoke(ComponentViewModel<AccordionWidgetProperties> widgetProperties) {
        return View("~/Components/Widgets/Accordion/Accordion.cshtml", widgetProperties);
    }
}
