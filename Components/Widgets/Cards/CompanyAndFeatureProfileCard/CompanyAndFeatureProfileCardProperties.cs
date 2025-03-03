using CMS.Websites;
using System.Collections.Generic;
using System.Linq;
using Kentico.PageBuilder.Web.Mvc;
using Kentico.Xperience.Admin.Base.FormAnnotations;
using Kentico.Xperience.Admin.Websites.FormAnnotations;
using Kentico.Components.Web.Mvc.FormComponents;
using Kentico.Forms.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Convenience.org.Components.Widgets.Cards
{
    public class CompanyAndFeatureProfileCardProperties : IWidgetProperties
    {
        private const string Options =
        "Company Card;Company Card" + "\n" +
        "Featured Profile Card;Featured Profile Card" + "\n";


        [WebPageSelectorComponent(TreePath = "/", Label = "Select Person", Order = 0, MaximumPages = 1)]
        public IEnumerable<WebPageRelatedItem> SelectedPerson { get; set; } = Enumerable.Empty<WebPageRelatedItem>();


        [DropDownComponent(ExplanationText = "Card Type", Order = 1, Label = "Card Type", Options = Options)]
        public string CardType { get; set; }
    }
}
