using CMS.MediaLibrary;
using System.Collections.Generic;
using System.Linq;
using Kentico.PageBuilder.Web.Mvc;
using Kentico.Xperience.Admin.Base.FormAnnotations;

namespace Convenience.org.Components.Widgets.Cards.ExpandableContentCard
{
    public class ExpandableContentCardWidgetProperties : IWidgetProperties
    {

        [TextInputComponent(Label = "Eyebrow Title", Order = 0)]
        public string? EyebrowTitle { get; set; } = string.Empty;

        [TextInputComponent(Label = "Title", Order = 1)]
        public string Title { get; set; } = string.Empty;

        [TextInputComponent(Label = "Summary", Order = 2)]
        public string? Summary { get; set; }

        [RequiredValidationRule]
        [AssetSelectorComponent(MaximumAssets = 1, Label = "Image", AllowedExtensions = "gif;png;jpg;jpeg", Order = 3)]
        public IEnumerable<AssetRelatedItem> Image { get; set; } = Enumerable.Empty<AssetRelatedItem>();

        [RichTextEditorComponent(Label = "Expanded richtext content", Order = 4)]
        public string? ExpandedRichtextContent { get; set; } = string.Empty;

    }
}
