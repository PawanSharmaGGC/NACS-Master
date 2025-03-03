using Kentico.PageBuilder.Web.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Convenience.org.Models;
using CMS.Websites;
using Convenience.org.Repositories.Interfaces;
using Kentico.Content.Web.Mvc.Routing;
using System.Linq;

namespace Convenience.org.Components.Widgets.Cards
{
    public class CompanyAndFeatureProfileCardViewComponent : ViewComponent
    {
        private readonly IPersonBioRepository personBioRepository;
        private readonly IWebPageUrlRetriever urlRetriever;
        private readonly IPreferredLanguageRetriever languageRetriever;

        public CompanyAndFeatureProfileCardViewComponent(IPersonBioRepository personBioRepository, IWebPageUrlRetriever urlRetriever, IPreferredLanguageRetriever languageRetriever)
        {
            this.personBioRepository = personBioRepository;
            this.urlRetriever = urlRetriever;
            this.languageRetriever = languageRetriever;
        }

        public async Task<ViewViewComponentResult> InvokeAsync(ComponentViewModel<CompanyAndFeatureProfileCardProperties> widgetProperties)
        {
            var model = CompanyAndFeatureProfileCardViewModel.GetViewModel(widgetProperties.Properties);

            if (widgetProperties != null)
            {
                //Gets the GUIDs from the annotated property
                var pageGuids = widgetProperties?.Properties?.SelectedPerson.Select(i => i.WebPageGuid).ToList();
                if (pageGuids != null && pageGuids.Any())
                {
                    var person = personBioRepository.GetCompanyandFeatureProfileRepository(pageGuids);
                    model.companyAndFeatureProfileCardItem = person;
                    model.companyAndFeatureProfileCardItem.CardType = widgetProperties.Properties.CardType;
                }
            }
            return View("~/Components/Widgets/Cards/CompanyAndFeatureProfileCard/CompanyAndFeatureProfileCard.cshtml", model);
        }
    }
}
