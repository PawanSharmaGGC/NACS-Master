using Convenience.org.Components.Widgets.SponsoresWidget;
using System.Collections.Generic;

namespace Convenience.org.Models;

public class SponsoresViewModel
{
    public EyebrowTitleViewModel Eyebrow { get; set; }
    public List<SponsorItem> SponsorItems { get; set; }
    public string CTAText { get; set; }
    public string CTAUrl { get; set; }

    public static SponsoresViewModel GetViewModel(SponsoresWidgetProperties properties)
    {
        var viewModel = new SponsoresViewModel();
        viewModel.Eyebrow = new EyebrowTitleViewModel();
        viewModel.Eyebrow.Title = properties?.EyebrowTitle;
        viewModel.Eyebrow.LeftBorderColor = "border_left_green EyebrowTitleStyle-module__eyebrow";
        viewModel.Eyebrow.TitleColor = "text-primary";
        viewModel.Eyebrow.TitleCase = "text-uppercase";

        viewModel.CTAText = properties?.CTAText;
        viewModel.CTAUrl = properties?.CTAUrl;
        return viewModel;
    }
}

public class SponsorItem
{
    public string SponsorName { get; set; }
    public string Image { get; set; }
    public string ImageAlt { get; set; }
}