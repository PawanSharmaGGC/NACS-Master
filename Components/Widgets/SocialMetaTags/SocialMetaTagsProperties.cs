using CMS.Websites;
using Kentico.PageBuilder.Web.Mvc;
using Kentico.Xperience.Admin.Base.FormAnnotations;
using Kentico.Xperience.Admin.Websites.FormAnnotations;
using System.Collections.Generic;

namespace Convenience.org.Components.Widgets.SocialMetaTags;

public class SocialMetaTagsProperties : IWidgetProperties
{
    [TextInputComponent(Label = "Social Media Handler", Order = 0)]
    public string? SocialMediaHandler { get; set; }

    [TextInputComponent(Label = "Default Title", Order = 1)]
    public string DefaultTitle { get; set; }

    [TextAreaComponent(Label = "Default Description", Order = 2)]
    public string? DefaultDescription { get; set; }

    [TextAreaComponent(Label = "Default Image URL", Order = 3,ExplanationText = "Please provide the full URL")]
    public string DefaultImage { get; set; }

}
