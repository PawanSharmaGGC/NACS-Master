using CMS.MediaLibrary;
using System.Collections.Generic;
using System.Linq;
using Kentico.PageBuilder.Web.Mvc;
using Kentico.Xperience.Admin.Base.FormAnnotations;
using Kentico.Xperience.Admin.Websites.FormAnnotations;
using System;
using CMS.ContentEngine;

namespace Convenience.org.Components.Widgets.Tier2Hero
{
    public class Tier2HeroWidgetProperties : IWidgetProperties
    {
        [TextInputComponent(Label = "Eyebrow Title", Order = 0)]
        public string EyebrowTitle { get; set; } = string.Empty;

        [TextInputComponent(Label = "Title", Order = 1)]
        public string Title { get; set; } = string.Empty;
        [AssetSelectorComponent(MaximumAssets = 1, Label = "Image", AllowedExtensions = "gif;png;jpg;jpeg", Order = 2)]
        public IEnumerable<AssetRelatedItem> Image { get; set; } = Enumerable.Empty<AssetRelatedItem>();

        [DateTimeInputComponent(Label = "Date time", Order = 3)]
        public DateTime? DateTime { get; set; }
        
        [TextInputComponent(Label = "Location or read minutes", Order = 4)]
        public string LocationOrReadMinutes { get; set; } = string.Empty;

        [TextInputComponent(Label = "Mobile text", Order = 5)]
        public string? MobileTitle { get; set; } = string.Empty;

        [TextInputComponent(Label = "CTA Text", Order = 6)]
        public string? CTAText { get; set; } = string.Empty;

        [UrlSelectorComponent(Label = "CTA Url", Order = 7)]
        public string? CTAUrl { get; set; } = string.Empty;

    }
}
