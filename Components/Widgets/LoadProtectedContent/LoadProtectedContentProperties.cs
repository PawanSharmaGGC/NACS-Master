using Kentico.PageBuilder.Web.Mvc;
using Kentico.Xperience.Admin.Base.FormAnnotations;

namespace Convenience.org.Components.Widgets.LoadProtectedContent
{
    public class LoadProtectedContentProperties : IWidgetProperties
    {
        [TextInputComponent(Label = "Redirect Token Expiration", Order = 0)]
        public string TimeToExpire { get; set; }

        [TextInputComponent(Label = "Contact Email", Order = 1)]
        public string? ContactEmail { get; set; }
    }
}
