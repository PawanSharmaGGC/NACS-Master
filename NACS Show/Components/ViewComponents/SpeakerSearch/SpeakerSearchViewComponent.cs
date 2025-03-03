using CMS.ContentEngine;

using Kentico.Content.Web.Mvc;

using MediatR;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using NACS.Portal.Core.Components.Pagination;

using NACSShow.Modules;
using NACSShow.Services.Search.Operations;
using NACSShow.Services.Search.SpeakerSearch;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NACSShow.Components.ViewComponents.SpeakerSearch
{
    public class SpeakerSearchViewComponent(IWebPageDataContextRetriever _contextRetriever, SpeakerSearchService _searchService, IMediator mediator) : ViewComponent
    {
        private readonly IWebPageDataContextRetriever contextRetriever = _contextRetriever;
        private readonly SpeakerSearchService searchService = _searchService;
        private readonly IMediator _mediator = mediator;

        public async Task<IViewComponentResult> InvokeAsync()
        {
            if (!contextRetriever.TryRetrieve(out _))
            {
                return Content("");
            }

            var request = new SpeakerSearchRequest(HttpContext.Request);

            var searchResult = await searchService.SearchSpeakersAsync(request);

            var qs = HttpContext.Request.Query;

            var tracksResponse = await _mediator.Send(new SpeakerTaxonomiesQuery());
            var tracks = tracksResponse.Tracks;

            var model = new SpeakerSearchViewModel(request, searchResult, qs, tracks);

            return View("~/Components/ViewComponents/SpeakerSearch/SpeakerSearchResults.cshtml", model);
        }
    }
}
