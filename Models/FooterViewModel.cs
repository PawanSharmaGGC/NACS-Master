using CMS.DataEngine;
using CMS.MediaLibrary;
using System.Collections.Generic;
using System.Linq;

namespace Convenience.org.Models
{
    public class FooterViewModel
    {

        public FooterViewModel GetViewModel(Footer footer)
        {
            FooterViewModel viewModel = new FooterViewModel();
            if (footer != null)
            {
                ;

                viewModel.TopShortDescription = footer.TopShortDescription;
                viewModel.HeadquarterTitle = footer.HeadquarterTitle;
                viewModel.AddressLine1 = footer.AddressLine1;
                viewModel.AddressLine2 = footer.AddressLine2;
                viewModel.City = footer.City;
                viewModel.State = footer.State;
                viewModel.ZipCode = footer.ZipCode;
                viewModel.ContactNumber = footer.ContactNumber;
                viewModel.ContactEmail = footer.ContactEmail;
                viewModel.CopyRightText = footer.CopyRightText;
                viewModel.DesignByText = footer.DesignByText;
            }
            return viewModel;
        }

        public string TopShortDescription { get; set; }
        public string FooterLogo { get; set; }
        public string ClipImageSrc { get; set; }
        public string HeadquarterTitle { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string ContactNumber { get; set; }
        public string ContactEmail { get; set; }
        public string CopyRightText { get; set; }
        public string DesignByText { get; set; }
        public IEnumerable<NavBarMenuViewModel> FooterLinks { get; set; } = new List<NavBarMenuViewModel>();
        public IEnumerable<NavBarMenuViewModel> FooterPrivacyLinks { get; set; } = new List<NavBarMenuViewModel>();
        public IEnumerable<NavBarMenuViewModel> SocialMediaLinks { get; set; } = new List<NavBarMenuViewModel>();
        //public NavBarMenu IndustryPartnersLinks { get; set; } = new NavBarMenu();
        //public NavBarMenu StayCurrentLinks { get; set; } = new NavBarMenu();

    }
}
