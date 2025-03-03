using Kentico.PageBuilder.Web.Mvc;
using Kentico.Xperience.Admin.Base.FormAnnotations;

namespace Convenience.org.Components.Widgets.EventRegMXRedirectSessionSignup;

public class EventRegMXRedirectSessionSignupProperties : IWidgetProperties
{
    //This property not in use in legacy code
    [TextInputComponent(Label = "RegistrationSiteURL", Order = 0,ExplanationText = "Full URL to MX site")]
    public string? RegistrationSiteURL { get; set; } = "https://mynacs.convenience.org/Events/Calendar/Registration-Start";

    [TextInputComponent(Label = "Event Code", Order = 1, ExplanationText = "DExample: NS22")]
    public string? EventCode { get; set; } = "22SOI";

    //This property not in use in legacy code
    [TextInputComponent(Label = "Return URL", Order = 2)]
    public string ReturnURL { get; set; } = "/";

    //This property not in use in legacy code
    [CheckBoxComponent(Label = "Show Register Button Instead of Redirect", Order = 3)]
    public bool ShowRegisterButton { get; set; } =false;
}
