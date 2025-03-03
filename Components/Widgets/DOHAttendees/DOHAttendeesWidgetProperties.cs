using System.ComponentModel.DataAnnotations;
using Kentico.PageBuilder.Web.Mvc;
using Kentico.Xperience.Admin.Base.FormAnnotations;

namespace Convenience.org.Components.Widgets.DOHAttendees
{
    public class DOHAttendeesWidgetProperties : IWidgetProperties
    {
        [Required]
        [TextInputComponent(Label = "Event Key", Order = 1)]
        public string EventKey { get; set; } = "6632678b-39fb-4c0e-a2fd-27226141c451";

        [Required]
        [TextInputComponent(Label = "Event Year", Order = 2)]
        public string EventYear { get; set; }
    }
}
