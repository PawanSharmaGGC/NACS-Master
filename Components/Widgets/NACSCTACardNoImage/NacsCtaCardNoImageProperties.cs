using CMS.MediaLibrary;
using System.Collections.Generic;
using System.Linq;
using Kentico.PageBuilder.Web.Mvc;
using Kentico.Xperience.Admin.Base.FormAnnotations;

namespace Convenience.org.Components.Widgets
{
    public class NacsCtaCardNoImageProperties:IWidgetProperties
    {
        [TextInputComponent(ExplanationText = "Eyebrow Title", Order = 0, Label = "Eyebrow Title", Tooltip = "Eyebrow Title")]
        public string EyebrowTitle { get; set; }
        [TextInputComponent(ExplanationText = "Title", Order = 1, Label = "Title", Tooltip = "Title")]
        public string Title { get; set; }
        [TextInputComponent(Order = 2, Label = "CTA Text")]
        public string CTAText { get; set; }
        [TextInputComponent(Order = 3, Label = "CTA Link")]
        public string CTALink { get; set; }
    }
}
