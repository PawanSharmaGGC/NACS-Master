namespace NACS.Portal.Core.Models;

public class FormWidgetViewModel(FormWidgetProperties props)
{
    public string Heading { get; set; } = props.Heading;
    public string Description { get; set; } = props.Description;
    public string SubmitMessage { get; set; } = props.SubmitMessage;
    public string? captchaSiteKey { get; set; }
}