using CMS.Websites;
using System.Collections.Generic;
using Kentico.PageBuilder.Web.Mvc;
using Kentico.Xperience.Admin.Base.FormAnnotations;
using Kentico.Xperience.Admin.Base.Forms;
using Kentico.Xperience.Admin.Websites.FormAnnotations;

namespace Convenience.org.Components.Widgets.ProductHero
{
    public class ProductHeroWidgetProperties : IWidgetProperties
    {
        [WebPageSelectorComponent(TreePath = "/", Label = "Select product", MaximumPages = 1, Order = 1)]
        public IEnumerable<WebPageRelatedItem> Products { get; set; } = new List<WebPageRelatedItem>();

        [TextInputComponent(Label = "CTA 1 Text", Order = 2)]
        public string CTA1Text { get; set; } = string.Empty;

        [UrlSelectorComponent(Label = "CTA 1 Link", Order = 3)]
        public string CTA1Link { get; set; } = string.Empty;

        [TextInputComponent(Label = "CTA 2 Text", Order = 4)]
        public string CTA2Text { get; set; } = string.Empty;

        [UrlSelectorComponent(Label = "CTA 2 Link", Order = 5)]
        public string CTA2Link { get; set; } = string.Empty;

        [TextInputComponent(Label = "Additional Licenses", Order = 6)]
        public string AdditionalLicenses { get; set; } = string.Empty;
    }
}
