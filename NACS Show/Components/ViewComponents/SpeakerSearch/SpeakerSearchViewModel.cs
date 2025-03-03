using Microsoft.AspNetCore.Http;

using NACS.Portal.Core.Components.Pagination;

using NACSShow.Modules;
using NACSShow.Services.Search.SpeakerSearch;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace NACSShow.Components.ViewComponents.SpeakerSearch
{
    public class SpeakerSearchViewModel : IPagedViewModel
    {
        public string? Title { get; set; } = "";
        public IEnumerable<SpeakerSearchIndexModel> Speakers { get; set; } = [];
        public List<string>? Companies { get; set; }
        public string? Query { get; set; } = "";
        public IReadOnlyList<TaxonomyTag> Tracks { get; set; } = [];
        public IQueryCollection? QueryStringValues { get; set; }
        public int Page { get; set; } = 0;
        public int TotalPages { get; set; } = 0;

        public SpeakerSearchViewModel() { }

        public Dictionary<string, string?> GetRouteData(int page) =>
            new()
            {
                { "query", Query },
                { "page", page.ToString() }
            };

        public SpeakerSearchViewModel(SpeakerSearchRequest request, SpeakerSearchResultsViewModel result, IQueryCollection qs, IReadOnlyList<TaxonomyTag> tracks)
        {
            Speakers = result.Hits;
            Companies = result.Companies;
            Page = request.PageNumber;
            Query = request.SearchText;
            Tracks = tracks;
            QueryStringValues = qs;
            TotalPages = result?.TotalPages ?? 0;
        }
    }
}
