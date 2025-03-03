using CMS.MediaLibrary;
using System.Collections.Generic;
using System.Linq;
using Kentico.PageBuilder.Web.Mvc;
using Kentico.Xperience.Admin.Base.FormAnnotations;
using Kentico.Xperience.Admin.Websites.FormAnnotations;
using System;

namespace Convenience.org.Components.Widgets.Tier3Hero
{
    public class Tier3HeroWidgetProperties : IWidgetProperties
    {

        [TextInputComponent(Label = "Eyebrow Title", Order = 0)]
        public string EyebrowTitle { get; set; } = string.Empty;

        [TextInputComponent(Label = "Title", Order = 1)]
        public string Title { get; set; } = string.Empty;
        [AssetSelectorComponent(MaximumAssets = 1, Label = "Image", AllowedExtensions = "gif;png;jpg;jpeg", Order = 2)]
        public IEnumerable<AssetRelatedItem> Image { get; set; } = Enumerable.Empty<AssetRelatedItem>();

        [TextInputComponent(Label = "Location Or Watch Time", Order = 3)]
        public string? LocationOrWatchTime { get; set; } = string.Empty;

        [DateTimeInputComponent(Label = "Date Time", Order = 3)]
        public DateTime? DateTime { get; set; }

        [TextInputComponent(Label = "CTA Text", Order = 4)]
        public string? CTAText { get; set; } = string.Empty;

        [UrlSelectorComponent(Label = "CTA Link", Order = 5)]
        public string? CTALink { get; set; } = string.Empty;
    }
}
