using CMS.Websites;
using Kentico.PageBuilder.Web.Mvc;
using Kentico.Xperience.Admin.Websites.FormAnnotations;
using System.Collections.Generic;

namespace Convenience.org.Components.Widgets.EventSpeakerCard
{
    public class EventSpeakerCardProperties : IWidgetProperties
    {
        [WebPageSelectorComponent(TreePath = "/", Label = "Select Speaker", Order = 1, MaximumPages = 1)]
        public IEnumerable<WebPageRelatedItem> SelectedSpeaker { get; set; } = new List<WebPageRelatedItem>();

    }
}
