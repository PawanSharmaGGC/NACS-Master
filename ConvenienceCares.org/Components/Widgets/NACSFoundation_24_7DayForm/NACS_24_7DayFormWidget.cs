using Kentico.PageBuilder.Web.Mvc;

using Microsoft.AspNetCore.Mvc;

using NACS.Portal.Core.Models;

namespace ConvenienceCares.Components.Widgets.NACSFoundation_24_7DayForm;

public class NACS_24_7DayFormWidgetViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(ComponentViewModel<FormWidgetProperties> vm)
    {
        var model = new FormWidgetViewModel(vm.Properties);

        return View("~/Components/Widgets/NACSFoundation_24_7DayForm/_NACS_24_7DayForm.cshtml", model);
    }
}

