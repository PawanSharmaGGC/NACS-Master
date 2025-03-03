using Convenience.org.Components.Widgets.FAQWidget;
using Convenience.org.Models;
using Convenience.org.Repositories.Interfaces;
using Kentico.PageBuilder.Web.Mvc;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;


[assembly: RegisterWidget(identifier: FAQWidget.IDENTIFIER, name: "FAQ",
    viewComponentType: typeof(FAQWidget),
    propertiesType: typeof(FAQWidgetProperties), Description = "FAQ",
    IconClass = "icon-question-circle", AllowCache = true)]

namespace Convenience.org.Components.Widgets.FAQWidget;

public class FAQWidget : ViewComponent
{
    public const string IDENTIFIER = "ConvenienceFAQ";
    private readonly IFAQRepository faqRepository;

    public FAQWidget(IFAQRepository faqRepository)
    {
        this.faqRepository = faqRepository;
    }
    public IViewComponentResult Invoke(ComponentViewModel<FAQWidgetProperties> widgetProperties)
    {
        // Retrieves the GUIDs of the selected pages from the 'Pages' property
        List<Guid> pageGuids = widgetProperties?.Properties?.faqs?
                                                    .Select(i => i.Identifier)
                                                    .ToList();

        var faqModel = FAQViewModel.GetViewModel(widgetProperties.Properties.Heading);

        if (pageGuids != null && pageGuids.Any())
        {
            var faqs = faqRepository.GetFAQRepository(pageGuids);
            faqModel.FAQItems= faqs;
        }

        return View("~/Components/Widgets/FAQWidget/FAQ.cshtml", faqModel);
    }
}
