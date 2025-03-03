using CMS.Websites;
using Kentico.PageBuilder.Web.Mvc;
using Kentico.Xperience.Admin.Base.FormAnnotations;
using Kentico.Xperience.Admin.Websites.FormAnnotations;
using System.Collections.Generic;

namespace Convenience.org.Components.Widgets.EventCarousel
{
    public class EventCarouselWidgetProperties : IWidgetProperties
    {
        [TextInputComponent(Label = "Event Title")]
        public string Title { get; set; }

        [TextInputComponent(Label = "Eyebrow Title")]
        public string? EyebrowTitle { get; set; }

        [TextInputComponent(Label = "Status")]
        public string? Status { get; set; }


        [WebPageSelectorComponent(TreePath = "/events", MaximumPages = 5, Label = "Select events")]
        // Returns a list of page selector items (node GUIDs)
        public IEnumerable<WebPageRelatedItem> EventItems { get; set; } = new List<WebPageRelatedItem>();
    }
}
