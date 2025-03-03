using Kentico.PageBuilder.Web.Mvc;
using Kentico.Xperience.Admin.Base.FormAnnotations;

namespace Convenience.org.Components.Widgets.Accordion;

public class AccordionWidgetProperties : IWidgetProperties
{
    [TextInputComponent(Label ="Title" ,Order =1)]
    public string Title { get; set; }

    [TextAreaComponent(Label = "Description", Order = 2)]
    public string Description { get; set; }
}
