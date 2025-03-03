using CMS.Websites;
using Kentico.PageBuilder.Web.Mvc;
using Kentico.Xperience.Admin.Base.FormAnnotations;
using Kentico.Xperience.Admin.Websites.FormAnnotations;
using System.Collections.Generic;

namespace Convenience.org.Components.Widgets.FeaturedContentCard;

public class FeaturedContentCardProperties : IWidgetProperties
{
    [TextInputComponent(Label = "Heading", Order = 0)]
    public string Heading { get; set; }

    [WebPageSelectorComponent(TreePath = "/", Label = "Select Card Detail", Order = 1, MaximumPages = 1)]
    public IEnumerable<WebPageRelatedItem> CardDetail { get; set; } = new List<WebPageRelatedItem>();

    [TextInputComponent(Label = "CTA text", Order = 2)]
    public string CTAText { get; set; }
}
