using Kentico.PageBuilder.Web.Mvc;
using Kentico.Xperience.Admin.Base.FormAnnotations;

namespace NACS.Portal.Core.Models;

public class FormWidgetProperties : IWidgetProperties
{
    [TextAreaComponent(Label = "Heading", Order = 1)]
    public string Heading { get; set; } = "";

    [TextAreaComponent(Label = "Description", Order = 2)]
    public string Description { get; set; } = "";

    [TextAreaComponent(Label = "Submit Message", Order = 3)]
    public string SubmitMessage { get; set; } = "";


}