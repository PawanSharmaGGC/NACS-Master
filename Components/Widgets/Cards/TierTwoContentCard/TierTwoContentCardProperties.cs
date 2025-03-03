using Kentico.PageBuilder.Web.Mvc;
using Kentico.Forms.Web.Mvc;
using System.Collections.Generic;
using System.Linq;
using System;
using CMS.MediaLibrary;
using Kentico.Xperience.Admin.Base.FormAnnotations;
namespace Convenience.org.Components.Widgets.Cards.TierTwoContentCard;

public class TierTwoContentCardProperties : IWidgetProperties
{
    [TextInputComponent(Label = "Title", Order = 0)]
    public string Title { get; set; } = string.Empty;

    [AssetSelectorComponent(MaximumAssets = 1, Label ="Image", AllowedExtensions = "gif;png;jpg;jpeg")]
    public IEnumerable<AssetRelatedItem> Image { get; set; } = Enumerable.Empty<AssetRelatedItem>();

    [DateTimeInputComponent(Label = "Published Date", Order = 2)]
    public DateTime PublishedDate { get; set; } 

    [TextInputComponent(Label = "Read Caption", Order = 3)]
    public string ReadCaption { get; set; } = string.Empty;
}
