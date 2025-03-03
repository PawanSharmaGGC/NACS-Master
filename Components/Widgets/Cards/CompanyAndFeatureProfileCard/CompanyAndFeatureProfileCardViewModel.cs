using Convenience.org.Components.Widgets.BioCardWidget;
using Convenience.org.Models;
using Convenience.org.Models.Cards;
using System;
using System.Collections.Generic;

namespace Convenience.org.Components.Widgets.Cards
{
    public class CompanyAndFeatureProfileCardViewModel
    {
        public CompanyAndFeatureProfileCardItem companyAndFeatureProfileCardItem { get; set; }
        public static CompanyAndFeatureProfileCardViewModel GetViewModel(CompanyAndFeatureProfileCardProperties properties)
        {
            var viewModel = new CompanyAndFeatureProfileCardViewModel();
            return viewModel;
        }
    }
    public class CompanyAndFeatureProfileCardItem
    {
        public string EyebrowTitle { get; set; }
        public string ImageUrl { get; set; }
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public string Website { get; set; }
        public string Email { get; set; }
        public string ContactNo { get; set; }
        public string CTAText { get; set; }
        public string CTALink { get; set; }
        public string CardType { get; set; }
    }
}
