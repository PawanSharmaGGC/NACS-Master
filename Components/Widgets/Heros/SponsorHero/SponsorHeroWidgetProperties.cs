using CMS.Websites;
using System.Collections.Generic;
using Kentico.PageBuilder.Web.Mvc;
using Kentico.Xperience.Admin.Base.FormAnnotations;
using Kentico.Xperience.Admin.Websites.FormAnnotations;
using CMS.MediaLibrary;
using System.Linq;

namespace Convenience.org.Components.Widgets.Heros.SponsorHero
{
    public class SponsorHeroWidgetProperties : IWidgetProperties
    {
        [WebPageSelectorComponent(TreePath = "/", MaximumPages = 1, Label = "Select article", Order = 0)]
        // Returns a list of page selector items (node GUIDs)
        public IEnumerable<WebPageRelatedItem> ArticleItems { get; set; } = new List<WebPageRelatedItem>();

        [TextInputComponent(Label = "Eyebrow Title", Order = 1)]
        public string EyebrowTitle { get; set; }

        [AssetSelectorComponent(MaximumAssets = 1, Order = 2, Label = "Video File", AllowedExtensions = "MP4")]
        public IEnumerable<AssetRelatedItem> VideoURL { get; set; } = Enumerable.Empty<AssetRelatedItem>();

        [TextInputComponent(Label = "CTA Text", Order = 3)]
        public string? CTAText { get; set; } = string.Empty;

        [UrlSelectorComponent(Label = "CTA Link", Order = 4)]
        public string? CTALink { get; set; } = string.Empty;

        [CheckBoxComponent(Label = "Hide Add To Calendar Button?", Order = 5)]
        public bool HideAddToCalendarButton { get; set; }

    }
}
