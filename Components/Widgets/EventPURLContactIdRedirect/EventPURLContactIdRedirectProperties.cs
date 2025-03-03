using Kentico.PageBuilder.Web.Mvc;
using Kentico.Xperience.Admin.Base.FormAnnotations;

namespace Convenience.org.Components.Widgets.EventPURLContactIdRedirect
{
    public class EventPURLContactIdRedirectProperties : IWidgetProperties
    {
        [TextInputComponent(Label = "Event Id from Protech", Order = 0)]
        public string EventID { get; set; } = "4a80b4f0-0a2b-ee11-bdf4-0022482a4ba4";
    }
}
