using CMS.Websites;
using System.Collections.Generic;
using Kentico.PageBuilder.Web.Mvc;
using Kentico.Xperience.Admin.Base.FormAnnotations;
using Kentico.Xperience.Admin.Websites.FormAnnotations;

namespace Convenience.org.Components.Widgets.Cards.RelatedContentCard
{
    public class RelatedContentCardWidgetProperties : IWidgetProperties
    {
        [WebPageSelectorComponent(TreePath = "/", Label = "Select Article", Order = 1, MaximumPages = 1)]
        public IEnumerable<WebPageRelatedItem> SelectedArticle { get; set; } = new List<WebPageRelatedItem>();

        [TextInputComponent(Label = "CTA Text", Order = 2)]
        public string CTAText { get; set; }

        [TextInputComponent(Label = "Eyebrow title", Order = 3)]
        public string EyebrowTitle { get; set; }

    }
}
