using Kentico.PageBuilder.Web.Mvc;
using Microsoft.AspNetCore.Mvc;
using NACS.Portal.Core.Models;

namespace ConvenienceCares.Components.Widgets.ScholarshipUpdatesForm;

public class ScholarshipUpdatesFormWidgetViewComponent : ViewComponent
{

    public IViewComponentResult Invoke(ComponentViewModel<FormWidgetProperties> vm)
    {
        var model = new FormWidgetViewModel(vm.Properties);

        return View("~/Components/Widgets/ScholarshipUpdatesForm/_ScholarshipUpdatesForm.cshtml", model);
    }
}
