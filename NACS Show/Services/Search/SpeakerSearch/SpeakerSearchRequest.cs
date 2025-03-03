using Lucene.Net.Documents;
using Lucene.Net.Search;

using Microsoft.AspNetCore.Http;

using static NACSShow.Services.Search.SpeakerSearch.SpeakerSearchIndexModel;


namespace NACSShow.Services.Search.SpeakerSearch
{
    public class SpeakerSearchRequest
    {
        public const int PAGE_SIZE = 10;

        public SpeakerSearchRequest(HttpRequest request)
        {
            var query = request.Query;

            SearchText = query.TryGetValue("query", out var searchText) ? searchText.ToString() : "";
        }

        public SpeakerSearchRequest(int pageSize)
        {
            PageSize = pageSize;
        }

        public string SearchText { get; set; } = "";
        public int PageSize { get; set; } = PAGE_SIZE;
        public int PageNumber { get; set; } = 0;
        public bool AreFiltersDefault => string.IsNullOrWhiteSpace(SearchText);
    }

    public class SpeakerSearchResultsViewModel
    {
        public IEnumerable<SpeakerSearchIndexModel> Hits { get; } = [];

        public string Query { get; set; } = "";
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = SpeakerSearchRequest.PAGE_SIZE;
        public int TotalPages { get; set; } = 0;
        public int TotalHits { get; set; } = 0;
        public List<string>? Companies { get; set; }

        public static SpeakerSearchResultsViewModel Empty(SpeakerSearchRequest request, List<string> companies) => new()
            {
                Query = request.SearchText,
                Page = request.PageNumber,
                PageSize = request.PageSize,
                Companies = companies
            };

        public SpeakerSearchResultsViewModel(TopDocs topDocs, SpeakerSearchRequest request, Func<ScoreDoc, Document> retrieveDoc, List<string> companies)
        {
            int pageSize = Math.Max(1, request.PageSize);
            int pageNumber = Math.Max(1, request.PageNumber);
            int offset = PageSize * (pageNumber - 1);
            int limit = pageSize;

            Query = request.SearchText ?? "";
            Companies = companies;
            Page = pageNumber;
            TotalPages = topDocs.TotalHits <= 0 ? 0 : (topDocs.TotalHits - 1) / pageSize + 1;
            TotalHits = topDocs.TotalHits;
            Hits = topDocs.ScoreDocs
                .Skip(offset)
                .Take(limit)
                .Select(d => FromDocument(retrieveDoc(d)))
                .ToList();
        }

        private SpeakerSearchResultsViewModel()
        {
        }
    }
}
