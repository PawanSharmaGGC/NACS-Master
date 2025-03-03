namespace Convenience.org.Models;

public class FeaturedContentCardViewModel
{
    public EyebrowTitleViewModel Eyebrow { get; set; }
    public string Title { get; set; }
    public string Image { get; set; }
    public string ImageAltText { get; set; }
    public string AuthorName { get; set; }
    public string PublishedDate { get; set; }
    public string PageContent { get; set; }
    public ButtonsViewModel CTA { get; set; }

    public AvatarsViewModel Author { get; set; }

    public static FeaturedContentCardViewModel GetViewModel()
    {
        var viewModel = new FeaturedContentCardViewModel();
        viewModel.Eyebrow = new EyebrowTitleViewModel();
        viewModel.Eyebrow.LeftBorderColor = "border_left_green EyebrowTitleStyle-module__eyebrow";
        viewModel.Eyebrow.TitleColor = "color-0053A5";
        viewModel.Eyebrow.TitleCase = "text-uppercase";

        viewModel.CTA = new ButtonsViewModel();
        viewModel.CTA.ButtonText = "Read More";
        viewModel.CTA.ButtonURL = "#";
        viewModel.CTA.ButtonType = ButtonTypeEnum.SideRoundButton;
        viewModel.CTA.ButtonBGColor = "bg-0053A5";
        viewModel.CTA.LeftIconColor = "color-FFFFFF";
        viewModel.CTA.ButtonTextColor = "color-FFFFFF";
        viewModel.CTA.RightIconColor = "color-FFFFFF";
        viewModel.CTA.RightIconType = "fa-regular";
        viewModel.CTA.RightIconName = "fa-arrow-right-long";

        viewModel.Author = new AvatarsViewModel();
        viewModel.Author.Size = 40;

        return viewModel;
    }
}
