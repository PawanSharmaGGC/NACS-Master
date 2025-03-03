using Convenience.org.Components.Widgets.TestimonialCarouselWidget;
using Convenience.org.Models;
using Convenience.org.Repositories.Interfaces;
using Kentico.PageBuilder.Web.Mvc;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

[assembly: RegisterWidget(identifier: TestimonialCarouselWidget.IDENTIFIER, name: "Testimonial Carousel", 
	viewComponentType: typeof(TestimonialCarouselWidget), 
	propertiesType: typeof(TestimonialCarouselProperties), Description = "Testimonial Carousel", 
	IconClass = "icon-right-double-quotation-mark", AllowCache = true)]


namespace Convenience.org.Components.Widgets.TestimonialCarouselWidget;

public class TestimonialCarouselWidget : ViewComponent
{
	public const string IDENTIFIER = "TestimonialCarousel";
	private readonly ITestimonialsRepository _testimonialsRepository;

	public TestimonialCarouselWidget(ITestimonialsRepository testimonialsRepository)
	{
		_testimonialsRepository = testimonialsRepository;
	}

	public IViewComponentResult Invoke(ComponentViewModel<TestimonialCarouselProperties> widgetProperties)
	{
		var testimonialItems = new TestimonialViewModel()
		{
			Heading = widgetProperties?.Properties?.Heading,
            CTAText = widgetProperties?.Properties?.CTAText,
            CTAUrl = widgetProperties?.Properties?.CTAUrl,
			TestimonialItems = new List<Testimonial>()
		};

		List<Guid> pageGuids = widgetProperties?.Properties?.Testimonials?.Select(i => i.WebPageGuid).ToList();
		if (pageGuids != null && pageGuids.Any())
		{
			testimonialItems.TestimonialItems = _testimonialsRepository.GetTestimonialsRepository(pageGuids).ToList();
		}

		return View("~/Components/Widgets/TestimonialCarouselWidget/TestimonialCarousel.cshtml", testimonialItems);
	}
}
