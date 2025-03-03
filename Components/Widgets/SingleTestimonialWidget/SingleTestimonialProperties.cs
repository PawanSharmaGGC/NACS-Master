using CMS.Websites;
using Kentico.PageBuilder.Web.Mvc;
using Kentico.Xperience.Admin.Base.FormAnnotations;
using Kentico.Xperience.Admin.Websites.FormAnnotations;
using System.Collections.Generic;

namespace Convenience.org.Components.Widgets.SingleTestimonialWidget
{
	public class SingleTestimonialProperties : IWidgetProperties
	{
        [WebPageSelectorComponent(TreePath = "/", Label = "Select testimonials", Order = 1)]
        public IEnumerable<WebPageRelatedItem> Testimonials { get; set; } = new List<WebPageRelatedItem>();

        [TextInputComponent(Label = "CTA text", Order = 2)]
        public string CTAText { get; set; }

        [TextInputComponent(Label = "CTA Url", Order = 3)]
        public string CTAUrl { get; set; }
    }
}
