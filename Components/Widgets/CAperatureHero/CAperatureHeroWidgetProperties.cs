using CMS.MediaLibrary;
using Kentico.PageBuilder.Web.Mvc;
using Kentico.Xperience.Admin.Base.FormAnnotations;
using Kentico.Xperience.Admin.Websites.FormAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Convenience.org.Components.Widgets.CAperatureHero
{
    public class CAperatureHeroWidgetProperties: IWidgetProperties
    {
        [TextInputComponent(Label = "Eyebrow Title", Order = 0)]
        public string EyebrowTitle { get; set; } = string.Empty;

        [TextInputComponent(Label = "Title", Order = 1)]
        public string Title { get; set; } = string.Empty;
       
        [AssetSelectorComponent(MaximumAssets = 1, Label = "Clip Image", AllowedExtensions = "gif;png;jpg;jpeg", Order = 2)]
        public IEnumerable<AssetRelatedItem> ClipImage { get; set; } = Enumerable.Empty<AssetRelatedItem>();

        [TextInputComponent(Label = "Video URL", Order = 3)]
        public string? VideoUrl { get; set; } = string.Empty;
        [AssetSelectorComponent(MaximumAssets = 1, Label = "Video (From medial library)", AllowedExtensions = "webm;mp4", Order = 4)]
        public IEnumerable<AssetRelatedItem> Video { get; set; } = Enumerable.Empty<AssetRelatedItem>();
        
        [AssetSelectorComponent(MaximumAssets = 1, Label = "Mobile Image", AllowedExtensions = "gif;png;jpg;jpeg", Order = 5)]
        public IEnumerable<AssetRelatedItem>? MobileImage { get; set; } = Enumerable.Empty<AssetRelatedItem>();

        [DateInputComponent(Label = "DateTime", Order = 6)]
        public DateTime? DateTime { get; set; }

        [TextInputComponent(Label = "Location Or Read Minutes", Order = 7)]
        public string? LocationOrReadMinutes { get; set; } = string.Empty;
        [TextInputComponent(Label = "CTA Text", Order = 8)]
        public string? CTAText { get; set; } = string.Empty;

        [UrlSelectorComponent(Label = "CTA Link", Order = 9)]
        public string? CTALink { get; set; } = string.Empty;

    }
}
