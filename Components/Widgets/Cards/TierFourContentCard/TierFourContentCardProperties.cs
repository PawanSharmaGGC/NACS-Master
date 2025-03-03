using Kentico.PageBuilder.Web.Mvc;
using Kentico.Xperience.Admin.Base.FormAnnotations;
using Kentico.Components.Web.Mvc.FormComponents;
using Kentico.Xperience.Admin.Websites.FormAnnotations;
using Kentico.Forms.Web.Mvc;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualBasic;
using System;
namespace Convenience.org.Components.Widgets.Cards.TierFourContentCard;

public class TierFourContentCardProperties : IWidgetProperties
{

    [TextInputComponent(Label = "Title", Order = 0)]
    public string Title { get; set; } = string.Empty;

    [DateTimeInputComponent(Label = "Published Date", Order = 1)]
    public DateTime PublishedDate { get; set; }

    [TextInputComponent(Label = "Caption", Order = 2)]
    public string Caption { get; set; } = string.Empty;
}
