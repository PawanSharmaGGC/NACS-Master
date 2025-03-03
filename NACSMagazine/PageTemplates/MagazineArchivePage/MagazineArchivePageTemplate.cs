﻿using AngleSharp.Common;

using CMS.ContentEngine;
using CMS.Websites;

using Kentico.Content.Web.Mvc;
using Kentico.Content.Web.Mvc.PageBuilder;
using Kentico.Content.Web.Mvc.Routing;
using Kentico.PageBuilder.Web.Mvc.PageTemplates;

using MediatR;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

using NACS.Portal.Core.Rendering;

using NACSMagazine;
using NACSMagazine.PageTemplates.MagazineArchivePage;

using System.Globalization;

[assembly: RegisterPageTemplate(
    identifier: ArchivePage.CONTENT_TYPE_NAME,
    name: "Magazine Archive Page",
    propertiesType: null,
    customViewName: "~/PageTemplates/MagazineArchivePage/_MagazineArchivePage.cshtml",
    ContentTypeNames = [ArchivePage.CONTENT_TYPE_NAME]
    )]

[assembly: RegisterWebPageRoute(
    contentTypeName: ArchivePage.CONTENT_TYPE_NAME,
    controllerType: typeof(MagazineArchivePageTemplateController))]


namespace NACSMagazine.PageTemplates.MagazineArchivePage
{
    public class MagazineArchivePageTemplateController : Controller
    {
        private readonly IMediator mediator;
        private readonly IWebPageDataContextRetriever contextRetriever;
        private readonly IContentQueryExecutor executor;

        public MagazineArchivePageTemplateController(IMediator _mediator, IWebPageDataContextRetriever _contextRetriever, IContentQueryExecutor _executor)
        {
            mediator = _mediator;
            contextRetriever = _contextRetriever;
            executor = _executor;
        }

        public async Task<IActionResult> Index()
        {
            if (!contextRetriever.TryRetrieve(out var data))
            {
                return NotFound();
            }

            var success = int.TryParse(Request.Query["page"], out int value);

            PagedList<Issue> issues;
            if (success)
            {
                issues = await GetMagazineIssues(string.Empty, string.Empty, value, 9);
            }
            else
            {
                issues = await GetMagazineIssues(string.Empty, string.Empty, 1, 9);
            }

            var page = await mediator.Send(new MagazineArchivePageQuery(data.WebPage));

            page.IssuesList = issues;
            if(success)
            {                
                page.PageNumber = value;
            }
            else
            {
                page.PageNumber = 1;
            }
            page.PageSize = 9;
            page.TotalPages = issues.TotalPages;
            
            return new TemplateResult(page);
        }

        public async Task<PagedList<Issue>> GetMagazineIssues(string month, string year, int pageNumber, int pageSize)
        {
            var idsQuery = new ContentItemQueryBuilder().ForContentType(Issue.CONTENT_TYPE_NAME, config => config.Columns(nameof(Issue.SystemFields.ContentItemID)));

            var contentItemIds = (await executor.GetResult(idsQuery, c => c.ContentItemID)).ToList();

            var contentQuery = new ContentItemQueryBuilder()
                                .ForContentType(
                                    IssuePage.CONTENT_TYPE_NAME,
                                    config => config
                                    .ForWebsite("NACSMagazine")
                                    ).InLanguage("en");

            var issuesList = await executor.GetMappedWebPageResult<IssuePage>(contentQuery);

            var issues = new PagedList<Issue>();

            var issueQuery = new ContentItemQueryBuilder();
            if (string.IsNullOrEmpty(month) && string.IsNullOrEmpty(year))
            {
                issueQuery = new ContentItemQueryBuilder()
                                        .ForContentType(
                                        Issue.CONTENT_TYPE_NAME,
                                        config => config
                                        .TopN(100)
                                        .OrderBy("IssueDate DESC")
                                        ).InLanguage("en");
            }
            else
            {
                issueQuery = new ContentItemQueryBuilder()
                                        .ForContentType(
                                        Issue.CONTENT_TYPE_NAME,
                                        config => config
                                        .TopN(100)
                                        .OrderBy("IssueDate DESC")
                                        .Where(where => where.WhereContains("IssueDate", GetMonthIndex(month) + "-")
                                        .And()
                                        .Where(where => where.WhereContains("IssueDate", year + "-")))
                                        ).InLanguage("en");

            }

            var issueItems = await executor.GetMappedResult<Issue>(issueQuery);

            
            foreach (var issue in issueItems)
            {
                if (issue != null)
                {
                    var items = issuesList.Where(w => w.Title.Equals(issue.Title));
                    
                    if (items != null && items.Count() > 0)
                    {
                        foreach(var item in items)
                        {
                            issues.Add(issue);
                            issue.ParentPageUrl = "~/" + item.SystemFields.WebPageUrlPath;
                        }
                    }
                }
            }
            
            return PagedList<Issue>.ToPagedList(issues, pageNumber, pageSize);
        }

        [HttpPost]
        public async Task<IActionResult> IndexAsync(ArchivePage page)
        {
            if (!contextRetriever.TryRetrieve(out var data))
            {
                return NotFound();
            }

            var success = int.TryParse(Request.Query["page"], out int value);
            if (success)
            {
                page.PageNumber = value;
            }
            else
            {
                page.PageNumber = 1;
            }
            page.PageSize = 9;
            
            var issues = await GetMagazineIssues(page.SelectedMonth, page.SelectedYear, page.PageNumber, page.PageSize);

            page = await mediator.Send(new MagazineArchivePageQuery(data.WebPage));

            page.IssuesList = issues;
            page.TotalPages = issues.TotalPages;
            
            return new TemplateResult(page);
        }

        

        public string GetMonthIndex(string month)
        {
            return string.IsNullOrEmpty(month) ? string.Empty : (Array.IndexOf(CultureInfo.CurrentCulture.DateTimeFormat.MonthNames, month) + 1).ToString("00");
        }

        public static IEnumerable<SelectListItem> Months
        {
            get
            {
                foreach (var month in CultureInfo.CurrentCulture.DateTimeFormat.MonthNames)
                {
                    if (!string.IsNullOrEmpty(month))
                    {
                        yield return new SelectListItem(text: month, value: month);
                    }
                }
            }
            set { }
        }

        public static IEnumerable<SelectListItem> Years
        {
            get
            {
                yield return new SelectListItem(text: "2019", value: "2019");
                yield return new SelectListItem(text: "2020", value: "2020");
                yield return new SelectListItem(text: "2021", value: "2021");
                yield return new SelectListItem(text: "2022", value: "2022");
                yield return new SelectListItem(text: "2023", value: "2023");
                yield return new SelectListItem(text: "2024", value: "2024");
            }
            set { }
        }
    }
}
