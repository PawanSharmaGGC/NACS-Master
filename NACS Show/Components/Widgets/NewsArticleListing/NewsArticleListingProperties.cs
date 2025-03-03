using Kentico.PageBuilder.Web.Mvc;
using Kentico.Xperience.Admin.Base.FormAnnotations;
using Kentico.Xperience.Admin.Websites.FormAnnotations;

namespace NACSShow.Components.Widgets;

public class NewsArticleListingProperties : IWidgetProperties
{
    [TextInputComponent(Label = "Heading", Order = 1)]
    public string Heading { get; set; } = string.Empty;
    [TextInputComponent(Label = "Path", Order = 2)]
    public string Path { get; set; } = string.Empty;
}
