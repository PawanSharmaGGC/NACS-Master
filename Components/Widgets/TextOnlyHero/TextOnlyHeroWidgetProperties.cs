using System;
using Kentico.PageBuilder.Web.Mvc;
using Kentico.Xperience.Admin.Base.FormAnnotations;
using Kentico.Xperience.Admin.Websites.FormAnnotations;

namespace Convenience.org.Components.Widgets.TextOnlyHero
{
    public class TextOnlyHeroWidgetProperties : IWidgetProperties
    {
        [TextInputComponent(Label = "Eyebrow Title", Order = 0)]
        public string EyebrowTitle { get; set; } = string.Empty;

        [TextInputComponent(Label = "Title", Order = 1)]
        public string Title { get; set; } = string.Empty;
        [RichTextEditorComponent(Label = "Description", Order = 3)]
        public string Description { get; set; } = string.Empty;
        [DateTimeInputComponent(Label = "Date", Order = 4)]
        public DateTime? Date { get; set; }
        [TextInputComponent(Label = "Location Or ReadTime", Order = 5)]
        public string? LocationOrReadTime { get; set; }

        [TextInputComponent(Label = "CTA Text", Order = 6)]
        public string CTAText { get; set; } = string.Empty;

        [UrlSelectorComponent(Label = "CTA Link", Order = 7)]
        public string CTALink { get; set; } = string.Empty;
    }
}
