using Convenience.org.Components.Widgets.EventDetail;

namespace Convenience.org.Models;

public class EventDetailViewModel
{
    public EyebrowTitleViewModel Eyebrow { get; set; }
    public string EventTiming { get; set; }
    public string EventDate { get; set; }
    public string EventAddress { get; set; }
    public string EventLocation { get; set; }
    public string EventStartTime { get; set; }

    public static EventDetailViewModel GetViewModel(EventDetailWidgetProperties properties)
    {
        var viewModel = new EventDetailViewModel();
        viewModel.Eyebrow = new EyebrowTitleViewModel();
        viewModel.Eyebrow.Title = properties?.Title;
        viewModel.Eyebrow.LeftBorderColor = "border_left_green EyebrowTitleStyle-module__eyebrow";
        viewModel.Eyebrow.TitleColor = "color-0053A5";
        viewModel.Eyebrow.TitleCase = "text-uppercase";
        return viewModel;
    }

}
