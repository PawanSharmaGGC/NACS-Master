using Convenience.org.Components.Widgets.RetailMembershipDuesCalculator;
using Kentico.PageBuilder.Web.Mvc;
using Microsoft.AspNetCore.Mvc;

[assembly: RegisterWidget(identifier: RetailMembershipCalculatorWidget.IDENTIFIER,
    propertiesType: null, viewComponentType: typeof(RetailMembershipCalculatorWidget),
    name: "Retail Membership Dues Calculator", Description = "Retail Membership Dues Calculator",
    IconClass = "icon-pda", AllowCache = true)]

namespace Convenience.org.Components.Widgets.RetailMembershipDuesCalculator;

public class RetailMembershipCalculatorWidget : ViewComponent
{
    public const string IDENTIFIER = "RetailMembershipDuesCalculator";
    public IViewComponentResult Invoke()
    {
        return View("~/Components/Widgets/RetailMembershipDuesCalculator/RetailMembershipCalculator.cshtml");
    }

}
