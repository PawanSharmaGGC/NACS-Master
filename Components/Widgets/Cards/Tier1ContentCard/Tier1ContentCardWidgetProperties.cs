using CMS.MediaLibrary;
using System.Collections.Generic;
using System.Linq;
using Kentico.PageBuilder.Web.Mvc;
using Kentico.Xperience.Admin.Base.FormAnnotations;
using Kentico.Xperience.Admin.Websites.FormAnnotations;
using CMS.Websites;
using System;

namespace Convenience.org.Components.Widgets.Cards.Tier1ContentCard
{
    public class Tier1ContentCardWidgetProperties : IWidgetProperties
    {
        [WebPageSelectorComponent(TreePath = "/events", MaximumPages = 1, Label = "Select events",Order =0,ExplanationText ="If the event is selected then no need to select other proerties as all content will be from this selected item.")]
        // Returns a list of page selector items (node GUIDs)
        public IEnumerable<WebPageRelatedItem>? EventItems { get; set; } = new List<WebPageRelatedItem>();
    
        [TextInputComponent(Label = "Eyebrow Title", Order = 0)]
        public string? EyebrowTitle { get; set; } = string.Empty;

        [TextInputComponent(Label = "Eyebrow Status", Order = 0)]
        public string? EyebrowStatus { get; set; }

        [TextInputComponent(Label = "Title", Order = 1)]
        public string? Title { get; set; } = string.Empty;
        [AssetSelectorComponent(MaximumAssets = 1, Label = "Image", AllowedExtensions = "gif;png;jpg;jpeg", Order = 2)]
        public IEnumerable<AssetRelatedItem> Image { get; set; } = Enumerable.Empty<AssetRelatedItem>();

        [RichTextEditorComponent(Label = "Description", Order = 3)]
        public string? Description { get; set; } = string.Empty;

        [TextInputComponent(Label = "CTA 1 Text", Order = 4)]
        public string CTA1Text { get; set; } = string.Empty;

        [UrlSelectorComponent(Label = "CTA 1 Link", Order = 5)]
        public string CTA1Link { get; set; } = string.Empty;

        [TextInputComponent(Label = "CTA 2 Text", Order = 6)]
        public string CTA2Text { get; set; } = string.Empty;

        [UrlSelectorComponent(Label = "CTA 2 Link", Order = 7)]
        public string CTA2Link { get; set; } = string.Empty;

        [DateTimeInputComponent(Label = "Datetime", Order = 8)]
        public DateTime? DateTime { get; set; }

        [TextInputComponent(Label = "Location or read time", Order = 9)]
        public string? LocationOrReadTime { get; set; }
}
}
