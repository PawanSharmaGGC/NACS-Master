using CMS.ContentEngine;
using Convenience.org.Models;
using Convenience.org.Repositories.Interfaces;
using System.Collections.Generic;
using System;
using CMS.Websites;
using System.Linq;
using NACS.Portal.Core.Services;
using Convenience.org.Components.Widgets.Cards;
using CMS.Core;

namespace Convenience.org.Repositories;

public class PersonBioRepository : IPersonBioRepository
{
    private readonly IContentQueryExecutor _contentQueryExecutor;
    private readonly IAssetItemService _itemService;
    private readonly IEventLogService eventLogService;

    public PersonBioRepository(IContentQueryExecutor contentQueryExecutor, IAssetItemService itemService, IEventLogService eventLogService)
    {
        _contentQueryExecutor = contentQueryExecutor;
        _itemService = itemService;
        this.eventLogService = eventLogService;
    }

    public PersonBioItem GetPersonBioRepository(List<Guid> webPageGuids)
    {
        try
        {
            var builder = new ContentItemQueryBuilder()
                    .ForContentTypes(parameters =>
                        parameters.ForWebsite(webPageGuids).OfContentType(PersonPage.CONTENT_TYPE_NAME)
                            .WithLinkedItems(2)
                            .WithContentTypeFields());

            var content = _contentQueryExecutor.GetMappedResult<PersonPage>(builder).Result;

            List<PersonBioItem> model = new List<PersonBioItem>();
            foreach (var item in content.Where(f => f.PersonContentItem.Any()))
            {
                var person = item.PersonContentItem?.FirstOrDefault();
                if (person != null)
                {
                    var images = person.Image != null ?
                            _itemService.RetrieveMediaFileImages(person.Image)?.Result : null;

                    model.Add(new PersonBioItem
                    {
                        Name = person.Name,
                        Designation = person.Designation,
                        ContactNo = person.ContactNo,
                        LinkedInUrl = person.LinkedInUrl,
                        Bio = person.Bio,
                        CompanyName = person.CompanyName,
                        AdditionalInfo = person.AdditionalInfo,
                        ImageUrl = images != null ? images.Select(s => _itemService.BuildFullFileUrl(s.URLData)).FirstOrDefault() : string.Empty,

                    });
                }
            }

            return model.FirstOrDefault() ?? new PersonBioItem();
        }
        catch (Exception ex)
        {
            eventLogService.LogException(nameof(PersonBioRepository), nameof(GetPersonBioRepository), ex);
            return new PersonBioItem();
        }
    }

    public CompanyAndFeatureProfileCardItem GetCompanyandFeatureProfileRepository(List<Guid> webPageGuids)
    {
        try
        {
            var builder = new ContentItemQueryBuilder()
                    .ForContentTypes(parameters =>
                        parameters.ForWebsite(webPageGuids).OfContentType(PersonPage.CONTENT_TYPE_NAME)
                            .WithLinkedItems(2)
                            .WithContentTypeFields());

            var content = _contentQueryExecutor.GetMappedResult<PersonPage>(builder).Result;

            List<CompanyAndFeatureProfileCardItem> model = new List<CompanyAndFeatureProfileCardItem>();
            foreach (var item in content.Where(f => f.PersonContentItem.Any()))
            {
                var person = item.PersonContentItem?.FirstOrDefault();
                if (person != null)
                {
                    var images = person.Image != null ?
                            _itemService.RetrieveMediaFileImages(person.Image)?.Result : null;

                    model.Add(new CompanyAndFeatureProfileCardItem
                    {
                        EyebrowTitle = person.EyebrowTitle,
                        Title = person.Title,
                        SubTitle = person.SubTitle,
                        Description = person.Description,
                        Address = person.Address,
                        ContactNo = person.ContactNo,
                        Website = person.Website,
                        Email = person.Email,
                        CTAText = person.CTAText,
                        CTALink = person.CTALink,
                        ImageUrl = images != null ? images.Select(s => _itemService.BuildFullFileUrl(s.URLData)).FirstOrDefault() : string.Empty,

                    });
                }
            }

            return model.FirstOrDefault() ?? new CompanyAndFeatureProfileCardItem();
        }
        catch (Exception ex)
        {
            eventLogService.LogException(nameof(PersonBioRepository), nameof(GetCompanyandFeatureProfileRepository), ex);
            return new CompanyAndFeatureProfileCardItem();
        }
    }
}
