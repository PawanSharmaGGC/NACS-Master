using Convenience.org.Models;
using System;
using System.Collections.Generic;

namespace Convenience.org.Repositories.Interfaces;
public interface ITestimonialsRepository
{
	IEnumerable<Testimonial> GetTestimonialsRepository(List<Guid> WebPageGuids);
}
