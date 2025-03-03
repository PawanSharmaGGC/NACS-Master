using CMS.MediaLibrary;
using CMS.Websites;
using Kentico.PageBuilder.Web.Mvc;
using Kentico.Xperience.Admin.Base.FormAnnotations;
using Kentico.Xperience.Admin.Websites.FormAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Convenience.org.Components.Widgets.Heros.Tier1GlassSuperHeroCard
{
    public class Tier1GlassSuperHeroCardWidgetProperties : IWidgetProperties
    {
        [TextInputComponent(Label = "Eyebrow Title", Order = 0)]
        public string EyebrowTitle { get; set; }

        [TextInputComponent(Label = "Title", Order = 1)]
        public string? Title { get; set; } = string.Empty;
        [AssetSelectorComponent(MaximumAssets = 1, Label = "Image", AllowedExtensions = "gif;png;jpg;jpeg", Order = 2)]
        public IEnumerable<AssetRelatedItem> Image { get; set; } = Enumerable.Empty<AssetRelatedItem>();

        [DateTimeInputComponent(Label = "Date", Order = 3)]
        public DateTime? DateTime { get; set; }

        [TextInputComponent(Label = "Read Time or Location", Order = 4)]
        public string? ReadTimeOrLocation { get; set; }

        [TextInputComponent(Label = "CTA Text", Order = 5)]
        public string? CTAText { get; set; } = string.Empty;

        [UrlSelectorComponent(Label = "CTA Link", Order = 6)]
        public string? CTALink { get; set; } = string.Empty;

        [WebPageSelectorComponent(TreePath = "/", MaximumPages = 1, Label = "Select article", Order = 0)]
        // Returns a list of page selector items (node GUIDs)
        public IEnumerable<WebPageRelatedItem> ArticleItems { get; set; } = new List<WebPageRelatedItem>();

    }
}
