using CMS.ContentEngine;
using Kentico.PageBuilder.Web.Mvc;
using Kentico.Xperience.Admin.Base.FormAnnotations;
using System.Collections.Generic;

namespace NACSShow.Components.Widgets.DailyNewsListing
{
    public class DailyNewsListingProperties: IWidgetProperties
    {
        public string? Title { get; set; }
    }
}
