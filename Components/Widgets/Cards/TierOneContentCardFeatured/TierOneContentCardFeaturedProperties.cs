using Kentico.PageBuilder.Web.Mvc;
using Kentico.Forms.Web.Mvc;
using System.Collections.Generic;
using Kentico.Xperience.Admin.Base.FormAnnotations;
using System.Linq;
using Kentico.Xperience.Admin.Websites.FormAnnotations;
using CMS.MediaLibrary;
namespace Convenience.org.Components.Widgets.Cards.TierOneContentCardFeatured;

public class TierOneContentCardFeaturedProperties : IWidgetProperties
{
    [TextInputComponent(Label = "Eyebrow Title", Order = 0)]
    public string EyebrowTitle { get; set; } = string.Empty;

    [TextInputComponent(Label = "Title", Order = 1)]
    public string Title { get; set; } = string.Empty;
    [AssetSelectorComponent(MaximumAssets = 1, Label = "Image", AllowedExtensions = "gif;png;jpg;jpeg", Order = 2)]
    public IEnumerable<AssetRelatedItem> Image { get; set; } = Enumerable.Empty<AssetRelatedItem>();

    [RichTextEditorComponent(Label = "Description", Order = 3)]
    public string Description { get; set; } = string.Empty;

    [TextInputComponent(Label = "CTA Text", Order = 4)]
    public string CTAText { get; set; } = string.Empty;

    [UrlSelectorComponent(Label = "CTA Link", Order = 5)]
    public string CTALink { get; set; } = string.Empty;

    [DropDownComponent(Label = "Image Position", Order = 6, Options = "image-left;Left\nimage-right;Right")]
    public string ImagePosition { get; set; } = "image-left";
}
