using Kentico.PageBuilder.Web.Mvc;
using Kentico.Xperience.Admin.Base.FormAnnotations;

namespace Convenience.org.Components.Sections.TwoColumnSection
{
    public class TwoColumnSectionProperties : ISectionProperties
    {
        [DropDownComponent(Label = "Select Layout", Options = "9-3;70-30\n8-4;60-40\n6-6;50-50\n4-8;40-60\n3-9;30-70", Order = 1)]
        public string SectionColumnWidth { get; set; }
    }
}
