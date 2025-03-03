using System.Collections.Generic;

namespace Convenience.org.Models;

public class TestimonialViewModel
{
	public string Heading { get; set; }
	public List<Testimonial> TestimonialItems { get; set; }
	public string CTAText { get; set; }
	public string CTAUrl { get; set; }
}

public class Testimonial
{
	public string Text { get; set; }
	public string Author { get; set; }
}
