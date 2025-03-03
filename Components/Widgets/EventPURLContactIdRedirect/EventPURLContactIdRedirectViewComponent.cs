using Convenience.org.Components.Widgets.EventPURLContactIdRedirect;
using System.Threading.Tasks;
using Convenience.org.Helpers;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Kentico.PageBuilder.Web.Mvc;
using System;
using System.Collections.Generic;
using Convenience.org.Repositories.Interfaces;

[assembly: RegisterWidget(identifier: EventPURLContactIdRedirectViewComponent.IDENTIFIER,
    name: "Event PURL ContactId Redirect Widget",
    viewComponentType: typeof(EventPURLContactIdRedirectViewComponent),
    propertiesType: typeof(EventPURLContactIdRedirectProperties),
    Description = "Event PURL ContactId Redirect Widget",
    IconClass = "icon-l-img-3-cols-3",
    AllowCache = true)]

namespace Convenience.org.Components.Widgets.EventPURLContactIdRedirect
{
    public class EventPURLContactIdRedirectViewComponent : ViewComponent
    {
        public const string IDENTIFIER = "Convenience.EventPURLContactIdRedirect";

        public async Task<IViewComponentResult> InvokeAsync(EventPURLContactIdRedirectProperties properties)
        {
            EventPURLContactIdRedirectViewModel vm = new EventPURLContactIdRedirectViewModel();
            vm.EventID = properties.EventID;
            return View($"~/Components/Widgets/EventPURLContactIdRedirect/_EventPURLContactIdRedirect.cshtml", vm);
        }
    }
}
