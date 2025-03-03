using Microsoft.AspNetCore.Mvc;

namespace Convenience.org.Components.ViewComponents.CountdownTimer
{
	public class CountdownTimerViewComponent : ViewComponent
	{
		public IViewComponentResult Invoke(string eventDateTime)
		{
			return View("~/Components/ViewComponents/CountdownTimer/CountdownTimer.cshtml", eventDateTime);
		}
	}
}
