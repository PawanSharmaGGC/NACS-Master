using CMS.ContentEngine;
using System.Collections.Generic;
using Kentico.Forms.Web.Mvc;
using Kentico.PageBuilder.Web.Mvc;
using Kentico.Xperience.Admin.Base.FormAnnotations;
using Kentico.Components.Web.Mvc.FormComponents;
using System.Linq;
using CMS.Websites;
using Kentico.Xperience.Admin.Websites.FormAnnotations;

namespace Convenience.org.Components.Widgets.Cards
{
    public class TierThreeContentCardProperties : IWidgetProperties
    {
        [TextInputComponent(ExplanationText = "Eyebrow Title", Order = 0, Label = "Title", Tooltip = "Eyebrow Title")]
        public string EyebrowTitle { get; set; }
        [TextInputComponent(Order = 1, Label = "CTA Text")]
        public string CTAText { get; set; }
        [CheckBoxComponent(Order = 2, Label = "CTA Left Icon Show")]
        public bool CTALeftIconVisible { get; set; }
        [WebPageSelectorComponent(TreePath = "/Community_News", MaximumPages = 10)]
        public IEnumerable<WebPageRelatedItem> SelectedCommunityNews { get; set; } = Enumerable.Empty<WebPageRelatedItem>();

    }
}
