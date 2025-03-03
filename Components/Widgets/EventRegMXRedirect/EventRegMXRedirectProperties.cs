using Kentico.PageBuilder.Web.Mvc;
using Kentico.Xperience.Admin.Base.FormAnnotations;

namespace Convenience.org.Components.Widgets.EventRegMXRedirect;

public class EventRegMXRedirectProperties : IWidgetProperties
{
    [TextInputComponent(Label = "RegistrationSiteURL", Order = 0,ExplanationText = "Full URL to MX site")]
    public string? RegistrationSiteURL { get; set; } = "https://mynacs.convenience.org/Events/Calendar/Registration-Start";

    [TextInputComponent(Label = "RegistrationSite_Production", Order = 1, ExplanationText = "Domain name of production MX site")]
    public string? RegistrationSite_Production { get; set; } = "https://mynacs.convenience.org";

    [TextInputComponent(Label = "RegistrationSite_Staging", Order = 2, ExplanationText = "Domain name of staging MX site")]
    public string? RegistrationSite_Staging { get; set; } = "https://nacsstagednn1.pcbscloud.com";
}
