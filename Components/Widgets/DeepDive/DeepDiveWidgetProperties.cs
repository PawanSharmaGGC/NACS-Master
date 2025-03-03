using CMS.ContentEngine;
using CMS.MediaLibrary;
using Kentico.PageBuilder.Web.Mvc;
using Kentico.Xperience.Admin.Base.FormAnnotations;
using System.Collections.Generic;
using System.Linq;

namespace Convenience.org.Components.Widgets.DeepDive;

public class DeepDiveWidgetProperties : IWidgetProperties
{
    [AssetSelectorComponent(Label = "Banner Image", Order = 1, AllowedExtensions = "gif;png;jpg;jpeg", MaximumAssets = 1)]
    public IEnumerable<AssetRelatedItem> BannerImage { get; set; } = Enumerable.Empty<AssetRelatedItem>();

    [AssetSelectorComponent(Label = "Light Image", Order = 2, AllowedExtensions = "gif;png;jpg;jpeg", MaximumAssets = 1)]
    public IEnumerable<AssetRelatedItem> LightImage { get; set; } = Enumerable.Empty<AssetRelatedItem>();

    [TextInputComponent(Label = "Title", Order = 3)]
    public string Title { get; set; }

    [TagSelectorComponent("Convenience.org", Label = "Select Tags", Order = 4)]
    public IEnumerable<TagReference> SelectedTags { get; set; } = Enumerable.Empty<TagReference>();

    [NumberInputComponent(Label = "Number Of Items To Display",Order = 5)]
    public int TopN { get; set; } = 10;

    [TextInputComponent(Label = "Card CTA Text", Order = 6)]
    public string CardCTAText { get; set; } = "Read Story";

}
