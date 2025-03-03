using Kentico.PageBuilder.Web.Mvc;
using Kentico.Xperience.Admin.Base.FormAnnotations;

namespace NACSShow.Components.Sections;

public class AccordionSectionProperties : ISectionProperties
{
    [TextInputComponent(Label = "Section Width", Order = 1, Tooltip = "Enter section width")]
    public string SectionWidth { get; set; }

    [NumberInputComponent(Label = "Accordion Items", Order = 2, Tooltip = "Number of items in accordion")]
    public int AccordionItems { get; set; } = 1;

}
