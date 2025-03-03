using Convenience.org.Components.Widgets.BioCardWidget;

namespace Convenience.org.Models;

public class PersonBioViewModel
{
    public PersonBioItem PersonBioItem { get; set; }

    public ButtonsViewModel CTA { get; set; }

    public static PersonBioViewModel GetViewModel()
    {
        var viewModel = new PersonBioViewModel();
        viewModel.PersonBioItem = new PersonBioItem();
        viewModel.CTA = new ButtonsViewModel();
        viewModel.CTA.ButtonURL = "#";
        viewModel.CTA.ButtonType = ButtonTypeEnum.TextLinkButton;

        viewModel.CTA.RightIconType = "fa-regular";
        viewModel.CTA.RightIconName = "fa-arrow-right-long";
        viewModel.CTA.RightIconColor = "color-396a99";

        return viewModel;
    }

}

public class PersonBioItem
{
    public string ImageUrl { get; set; }
    public string Name { get; set; }
    public string Designation { get; set; }
    public string ContactNo { get; set; }
    public string LinkedInUrl { get; set; }
    public string Bio { get; set; }
    public string AdditionalInfo { get; set; }
    public string CompanyName { get; set; }

}