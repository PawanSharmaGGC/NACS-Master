using Kentico.PageBuilder.Web.Mvc;
using Kentico.Xperience.Admin.Base.FormAnnotations;

namespace Convenience.org.Components.Sections.ThreeColumnSection
{
    public class ThreeColumnSectionProperties : ISectionProperties
    {
        [DropDownComponent(Label = "Select Layout Options", Options = "4-4-4;33.33-33.33-33.33\n5--;40-30-30", Order = 1)]
        public string SectionColumnWidth { get; set; }
    }
}
