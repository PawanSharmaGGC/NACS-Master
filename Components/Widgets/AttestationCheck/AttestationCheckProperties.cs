using Kentico.PageBuilder.Web.Mvc;
using Kentico.Xperience.Admin.Base.FormAnnotations;

namespace Convenience.org.Components.Widgets.AttestationCheck
{
    public class AttestationCheckProperties : IWidgetProperties
    {
        [TextInputComponent(Label = "Redirect Url", Order = 0)]
        public string RedirectURL { get; set; }

        [TextInputComponent(Label = "Attestation Text", Order = 1)]
        public string? AttestationText { get; set; }
    }
}
