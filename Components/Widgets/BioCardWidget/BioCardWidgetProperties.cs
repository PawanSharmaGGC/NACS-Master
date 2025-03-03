using CMS.Websites;
using Kentico.PageBuilder.Web.Mvc;
using Kentico.Xperience.Admin.Base.FormAnnotations;
using Kentico.Xperience.Admin.Websites.FormAnnotations;
using System.Collections.Generic;

namespace Convenience.org.Components.Widgets.BioCardWidget;

public class BioCardWidgetProperties : IWidgetProperties
{
    [WebPageSelectorComponent(TreePath = "/", Label = "Select Person", Order = 1, MaximumPages = 1)]
    public IEnumerable<WebPageRelatedItem> SelectedPerson { get; set; } = new List<WebPageRelatedItem>();

    [TextInputComponent(Label = "CTA Text", Order = 2)]
    public string CTAText { get; set; }

}
