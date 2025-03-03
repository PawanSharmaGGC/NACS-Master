using Kentico.PageBuilder.Web.Mvc;
using Kentico.Xperience.Admin.Base.FormAnnotations;

namespace Convenience.org.Components.Sections.CAccordionSection;

public class CAccordionSectionProperties : ISectionProperties
{
    [TextInputComponent(Label = "Heading", Order = 1)]
    public string Heading { get; set; }
}
