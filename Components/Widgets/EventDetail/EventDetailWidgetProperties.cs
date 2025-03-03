using CMS.Websites;
using Kentico.PageBuilder.Web.Mvc;
using Kentico.Xperience.Admin.Base.FormAnnotations;
using Kentico.Xperience.Admin.Websites.FormAnnotations;
using System.Collections.Generic;

namespace Convenience.org.Components.Widgets.EventDetail;

public class EventDetailWidgetProperties : IWidgetProperties
{
	[TextInputComponent(Label = "Title", Order = 1)]
	public string Title { get; set; }

	[WebPageSelectorComponent(TreePath = "/", Label = "Select event", Order = 2, MaximumPages = 1)]
	public IEnumerable<WebPageRelatedItem> EventDetail { get; set; }=new List<WebPageRelatedItem>();
}
