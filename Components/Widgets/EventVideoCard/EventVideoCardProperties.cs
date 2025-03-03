using CMS.MediaLibrary;
using System.Collections.Generic;
using System.Linq;
using Kentico.PageBuilder.Web.Mvc;
using Kentico.Xperience.Admin.Base.FormAnnotations;

namespace Convenience.org.Components.Widgets
{
    public class EventVideoCardProperties: IWidgetProperties
    {
        [TextInputComponent(ExplanationText = "Eyebrow Title", Order = 0, Label = "Eyebrow Title", Tooltip = "Eyebrow Title")]
        public string EyebrowTitle { get; set; }
        [TextInputComponent(ExplanationText = "Short Description", Order = 1, Label = "Sub Title", Tooltip = "Sub Title")]
        public string Title { get; set; }
        [TextInputComponent(Order = 2, Label = "Tag Name")]
        public string TagName { get; set; }
        [CheckBoxComponent(Order = 3, Label = "Overlay Show")]
        public bool IsOverlayVisible { get; set; }
        [RequiredValidationRule]
        [AssetSelectorComponent(MaximumAssets = 1, Order = 4, Label = "Video File", AllowedExtensions = "MP4")]
        public IEnumerable<AssetRelatedItem> VideoURL { get; set; } = Enumerable.Empty<AssetRelatedItem>();
        [RequiredValidationRule]
        [AssetSelectorComponent(MaximumAssets = 1, Order = 5, Label = "Video Poster", AllowedExtensions = "gif;png;jpg;jpeg")]
        public IEnumerable<AssetRelatedItem> VideoPoster { get; set; } = Enumerable.Empty<AssetRelatedItem>();
    }
}
