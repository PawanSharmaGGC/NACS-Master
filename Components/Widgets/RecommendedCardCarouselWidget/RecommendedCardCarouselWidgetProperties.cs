using CMS.Websites;
using System.Collections.Generic;
using Kentico.PageBuilder.Web.Mvc;
using Kentico.Xperience.Admin.Base.FormAnnotations;
using Kentico.Xperience.Admin.Websites.FormAnnotations;

namespace Convenience.org.Components.Widgets.RecommendedCardCarouselWidget
{
    public class RecommendedCardCarouselWidgetProperties : IWidgetProperties
    {
        [TextInputComponent(Label = "Left Title", Order = 1)]
        public string LeftTitle { get; set; }

        [TextInputComponent(Label = "CTA Text", Order = 2)]
        public string CTAText { get; set; }

        [UrlSelectorComponent(Label = "CTA Link", Order = 3)]
        public string CTALink { get; set; }

        [WebPageSelectorComponent(TreePath = "/", MaximumPages = 10, Label = "Select article")]
        // Returns a list of page selector items (node GUIDs)
        public IEnumerable<WebPageRelatedItem> ArticleItems { get; set; } = new List<WebPageRelatedItem>();
    }
}
