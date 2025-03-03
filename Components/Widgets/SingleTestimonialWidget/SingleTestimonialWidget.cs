using Convenience.org.Components.Widgets.SingleTestimonialWidget;
using Convenience.org.Models;
using Convenience.org.Repositories.Interfaces;
using Kentico.PageBuilder.Web.Mvc;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;


[assembly: RegisterWidget(identifier: SingleTestimonialWidget.IDENTIFIER, name: "Single Testimonial", 
	viewComponentType: typeof(SingleTestimonialWidget), 
	propertiesType: typeof(SingleTestimonialProperties), Description = "Single Testimonial", 
	IconClass = "icon-right-double-quotation-mark", AllowCache = true)]

namespace Convenience.org.Components.Widgets.SingleTestimonialWidget;

public class SingleTestimonialWidget : ViewComponent
{
    public const string IDENTIFIER = "SingleTestimonial";
    private readonly ITestimonialsRepository _testimonialsRepository;

	public SingleTestimonialWidget(ITestimonialsRepository testimonialsRepository)
	{
		_testimonialsRepository = testimonialsRepository;
	}

	public IViewComponentResult Invoke(ComponentViewModel<SingleTestimonialProperties> widgetProperties)
	{
		var testimonialItems = new TestimonialViewModel()
		{
            CTAText = widgetProperties?.Properties?.CTAText,
            CTAUrl = widgetProperties?.Properties?.CTAUrl,
			TestimonialItems = new List<Testimonial>()
		};

		List<Guid> pageGuids = widgetProperties?.Properties?.Testimonials?.Select(i => i.WebPageGuid).ToList();
		if (pageGuids != null && pageGuids.Any())
		{
			testimonialItems.TestimonialItems = _testimonialsRepository.GetTestimonialsRepository(pageGuids).ToList();
		}

		return View("~/Components/Widgets/SingleTestimonialWidget/SingleTestimonial.cshtml", testimonialItems);
	}
}
