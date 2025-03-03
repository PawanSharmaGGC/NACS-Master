using Convenience.org.Models;
using Microsoft.AspNetCore.Mvc;

namespace Convenience.org.Components.ViewComponents.EventCard
{
    public class EventCardViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(EventCardViewModel eventCard)
        {
            return View($"~/Components/ViewComponents/EventCard/EventCard.cshtml", eventCard);
        }
    }
}
