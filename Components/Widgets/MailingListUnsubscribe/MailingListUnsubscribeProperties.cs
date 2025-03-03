using Kentico.PageBuilder.Web.Mvc;
using Kentico.Xperience.Admin.Base.FormAnnotations;

namespace Convenience.org.Components.Widgets.MailingListUnsubscribe;

public class MailingListUnsubscribeProperties : IWidgetProperties
{
    [TextInputComponent(Label = "Mailing List Keys And Names", Order = 0,ExplanationText = "MailingListKeysAndNames")]
    public string MailingListKeysAndNames { get; set; } = "021f3b50-272a-4484-932e-f032a7df8388";
}
