using CMS.MediaLibrary;
using System.Collections.Generic;
using System.Linq;
using Kentico.PageBuilder.Web.Mvc;
using Kentico.Xperience.Admin.Base.FormAnnotations;
using Kentico.Forms.Web.Mvc;

namespace Convenience.org.Components.Widgets
{
    public class NACSCTACardWithImageProperties : IWidgetProperties
    {
        [TextInputComponent(Order = 0, Label = "Eyebrow Title", Tooltip = "Eyebrow Title")]
        public string EyebrowTitle { get; set; }
        [TextInputComponent(Order = 1, Label = "Title", Tooltip = "Title")]
        public string Title { get; set; }
        [CheckBoxComponent(Order = 2, Label = "Tag Visible")]
        public bool IsTagVisible { get; set; }
        [TextInputComponent(Order = 3, Label = "Tag Name")]
        [VisibleIfTrue(nameof(IsTagVisible))]
        public string TagName { get; set; }
        [TextInputComponent(Order = 4, Label = "CTA Text")]
        public string CTAText { get; set; }
        [TextInputComponent(Order = 5, Label = "CTA Link")]
        public string CTALink { get; set; }
        [AssetSelectorComponent(MaximumAssets = 1, Order = 6, Label = "Image", AllowedExtensions = "gif;png;jpg;jpeg")]
        public IEnumerable<AssetRelatedItem> Image { get; set; } = Enumerable.Empty<AssetRelatedItem>();
    }
}
