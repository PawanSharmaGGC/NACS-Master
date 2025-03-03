using Convenience.org.Components.Widgets.TestimonialStackWidget;
using Convenience.org.Models;
using Convenience.org.Repositories.Interfaces;
using Kentico.PageBuilder.Web.Mvc;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

[assembly: RegisterWidget(identifier: TestimonialStackWidget.IDENTIFIER, name: "Testimonial Stack", 
	viewComponentType: typeof(TestimonialStackWidget), 
	propertiesType: typeof(TestimonialStackProperties), Description = "Testimonial Stack", 
	IconClass = "icon-right-double-quotation-mark", AllowCache = true)]


namespace Convenience.org.Components.Widgets.TestimonialStackWidget;

public class TestimonialStackWidget : ViewComponent
{
	public const string IDENTIFIER = "TestimonialStack";
	private readonly ITestimonialsRepository _testimonialsRepository;

	public TestimonialStackWidget(ITestimonialsRepository testimonialsRepository)
	{
		_testimonialsRepository = testimonialsRepository;
	}
	public IViewComponentResult Invoke(ComponentViewModel<TestimonialStackProperties> widgetProperties)
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

		return View("~/Components/Widgets/TestimonialStackWidget/TestimonialStack.cshtml", testimonialItems);
	}
}
