using CMS.Websites;
using Kentico.PageBuilder.Web.Mvc;
using Kentico.Xperience.Admin.Base.FormAnnotations;
using Kentico.Xperience.Admin.Websites.FormAnnotations;
using System.Collections.Generic;

namespace Convenience.org.Components.Widgets.SponsoresWidget;

public class SponsoresWidgetProperties : IWidgetProperties
{
    [TextInputComponent(Label = "Eyebrow Title", Order = 0)]
    public string? EyebrowTitle { get; set; }

    [WebPageSelectorComponent(TreePath = "/", Label = "Select sponsores", Order = 1, MaximumPages = 0)]
    public IEnumerable<WebPageRelatedItem> Sponsores { get; set; } = new List<WebPageRelatedItem>();

    [TextInputComponent(Label = "CTA text", Order = 2)]
    public string CTAText { get; set; }

    [TextInputComponent(Label = "CTA Url", Order = 3)]
    public string CTAUrl { get; set; }

}
