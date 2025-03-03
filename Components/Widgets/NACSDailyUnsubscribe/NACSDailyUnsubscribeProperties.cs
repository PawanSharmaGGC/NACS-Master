using Kentico.PageBuilder.Web.Mvc;
using Kentico.Xperience.Admin.Base.FormAnnotations;

namespace Convenience.org.Components.Widgets.NACSDailyUnsubscribe;

public class NACSDailyUnsubscribeProperties : IWidgetProperties
{
    [TextInputComponent(Label = "Mailing List Key", Order = 0,ExplanationText = "Mailing List Key")]
    public string MailingListKey { get; set; } = "021f3b50-272a-4484-932e-f032a7df8388";
}
