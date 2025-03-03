using CMS.MediaLibrary;
using Kentico.PageBuilder.Web.Mvc;
using Kentico.Xperience.Admin.Base.FormAnnotations;
using System.Collections.Generic;
using System.Linq;

namespace Convenience.org.Components.Widgets.GoodJobCalculator;

public class GoodJobCalculatorProperties : IWidgetProperties
{
    [AssetSelectorComponent(Label = "Logo Image", Order = 1, AllowedExtensions = "gif;png;jpg;jpeg", MaximumAssets = 1)]
    public IEnumerable<AssetRelatedItem> Image { get; set; } = Enumerable.Empty<AssetRelatedItem>();

}
