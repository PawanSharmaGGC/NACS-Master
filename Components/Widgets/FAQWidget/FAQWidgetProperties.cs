using CMS.ContentEngine;
using Kentico.PageBuilder.Web.Mvc;
using Kentico.Xperience.Admin.Base.FormAnnotations;
using System.Collections.Generic;

namespace Convenience.org.Components.Widgets.FAQWidget;

public class FAQWidgetProperties : IWidgetProperties
{
    [TextInputComponent(Label = "Heading", Order = 0)]
    public string Heading { get; set; }

    [ContentItemSelectorComponent(FAQ.CONTENT_TYPE_NAME, Label = "Select FAQ's", Order = 1, MaximumItems = 0)]
    public IEnumerable<ContentItemReference> faqs { get; set; } = new List<ContentItemReference>();

}
