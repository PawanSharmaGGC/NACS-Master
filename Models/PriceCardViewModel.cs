using Convenience.org.Components.Widgets.PriceCard;

namespace Convenience.org.Models;

public class PriceCardViewModel
{
    public EyebrowTitleViewModel Eyebrow { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string RetailMembersPrice { get; set; }
    public string Others { get; set; }
    public ButtonsViewModel CTA { get; set; }

    public static PriceCardViewModel GetViewModel(PriceCardWidgetProperties widgetProperties)
    {
        var viewModel = new PriceCardViewModel();
        viewModel.Eyebrow = new EyebrowTitleViewModel();
        viewModel.Eyebrow.LeftBorderColor = "border_left_green EyebrowTitleStyle-module__eyebrow";
        viewModel.Eyebrow.TitleColor = "color-FFFFFF";
        viewModel.Eyebrow.TitleCase = "text-uppercase";
        viewModel.Eyebrow.Title = widgetProperties.EyebrowTitle;

        viewModel.Title=widgetProperties.Title;
        viewModel.Description=widgetProperties.Description;
        viewModel.RetailMembersPrice = widgetProperties.RetailMembersPrice;
        viewModel.Others = widgetProperties.Others;

        viewModel.CTA = new ButtonsViewModel();
        viewModel.CTA.ButtonText = widgetProperties.CTAText;
        viewModel.CTA.ButtonURL = widgetProperties.CTAUrl;
        viewModel.CTA.ButtonType = ButtonTypeEnum.InvertedButton;
        viewModel.CTA.LeftIconType = "fa-solid";
        viewModel.CTA.LeftIconName = "fa-circle-dollar";
        viewModel.CTA.LeftIconColor = "color-0053A5";
        viewModel.CTA.RightIconType = "fa-regular";
        viewModel.CTA.RightIconColor = "color-0053A5";
        viewModel.CTA.RightIconName = "fa-arrow-right";
        viewModel.CTA.ButtonBGColor = "bg-FFFFFF";
        
        viewModel.CTA.ButtonTextColor = "color-0053A5";
        

        return viewModel;
    }

}
