using CMS.Core;
using Convenience;
using Convenience.org.Models;
using Convenience.org.PageTemplates.BioDetail;
using Convenience.org.PageTemplates.BioDetail.Operations;
using Convenience.org.Repositories.Interfaces;
using Kentico.Content.Web.Mvc;
using Kentico.Content.Web.Mvc.Routing;
using Kentico.PageBuilder.Web.Mvc.PageTemplates;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NACS.Portal.Core.Services;
using System;
using System.Linq;
using System.Threading.Tasks;


[assembly: RegisterPageTemplate(
    identifier: PersonPage.CONTENT_TYPE_NAME,
    name: "Bio detail page template",
    propertiesType: null,
    customViewName: "~/PageTemplates/BioDetail/_BioDetail.cshtml",
    ContentTypeNames = [PersonPage.CONTENT_TYPE_NAME]
    )]

[assembly: RegisterWebPageRoute(
    PersonPage.CONTENT_TYPE_NAME,
    typeof(BioDetailTemplateController))]


namespace Convenience.org.PageTemplates.BioDetail;

public class BioDetailTemplateController : Controller
{
    private readonly IPersonBioRepository personBioRepository;
    private readonly IMediator mediator;
    private readonly IWebPageDataContextRetriever contextRetriever;
    private readonly IAssetItemService itemService;
    private readonly IEventLogService _eventLogService;

    public BioDetailTemplateController(IPersonBioRepository personBioRepository, IMediator mediator,
        IWebPageDataContextRetriever contextRetriever, IAssetItemService itemService)
    {
        this.personBioRepository = personBioRepository;
        this.mediator = mediator;
        this.contextRetriever = contextRetriever;
        this.itemService = itemService;
    }

    public async Task<IActionResult> Index()
    {
        PersonBioItem bioViewModel = new PersonBioItem();

        try
        {
            if (!contextRetriever.TryRetrieve(out var data))
            {
                return NotFound();
            }

            if (data.WebPage.WebPageItemGUID != Guid.Empty)
            {
                var page = await mediator.Send(new PersonPageQuery(data.WebPage));

                if (page.PersonContentItem != null && page.PersonContentItem.Any())
                {
                    var person = page.PersonContentItem.FirstOrDefault();

                    var images = person.Image != null ?
                            itemService.RetrieveMediaFileImages(person.Image)?.Result : null;

                    bioViewModel = new PersonBioItem
                    {
                        Name = person.Name,
                        Designation = person.Designation,
                        ContactNo = person.ContactNo,
                        LinkedInUrl = person.LinkedInUrl,
                        Bio = person.Bio,
                        CompanyName = person.CompanyName,
                        AdditionalInfo = person.AdditionalInfo,
                        ImageUrl = images != null ? images.Select(s => itemService.BuildFullFileUrl(s.URLData)).FirstOrDefault() : string.Empty,
                    };
                }
            }
        }
        catch (Exception ex)
        {
            _eventLogService.LogException(nameof(BioDetailTemplateController), nameof(Index), ex);
        }
        return new TemplateResult(bioViewModel);
    }
}
