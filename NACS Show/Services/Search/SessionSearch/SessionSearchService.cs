using CMS.Core;

using Kentico.Xperience.Lucene.Core.Indexing;
using Kentico.Xperience.Lucene.Core.Search;

using Lucene.Net.Analysis.Standard;
using Lucene.Net.Search;
using Lucene.Net.Facet;
using Lucene.Net.Util;

using Microsoft.AspNetCore.Http;

using static NACSShow.Services.Search.SessionSearch.SessionSearchIndexModel;
using Lucene.Net.Queries;
using Lucene.Net.Index;
using Lucene.Net.Documents;
using CMS.Base;
using Lucene.Net.Analysis.Core;

namespace NACSShow.Services.Search.SessionSearch
{
    public class SessionSearchRequest
    {
        public const int PAGE_SIZE = 10;

        public SessionSearchRequest(HttpRequest request)
        {
            var query = request.Query;

            DayFilter = query.TryGetValue("day", out var dayFilter)
                ? dayFilter.ToString("MMddyyyy")
                : string.Empty;
            KeywordFilter = query.TryGetValue("track", out var keywordFilter)
                ? keywordFilter.ToString()
                : string.Empty;
            SearchText = query.TryGetValue("query", out var queryValues)
                ? queryValues.ToString()
                : "";
            PageNumber = query.TryGetValue("page", out var pageValues)
                ? int.TryParse(pageValues, out int p)
                    ? p
                    : 1
                : 1;
        }

        public SessionSearchRequest(int pageSize)
        {
            PageSize = pageSize;

        }

        //public string Type { get; set; } = "";
        public string DayFilter { get; set; } = string.Empty;
        public string KeywordFilter { get; set; } = string.Empty;
        public string SearchText { get; set; } = "";
        //public string SortBy { get; set; } = "";
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = PAGE_SIZE;
        public bool AreFiltersDefault => string.IsNullOrWhiteSpace(SearchText) && string.IsNullOrWhiteSpace(DayFilter) && string.IsNullOrWhiteSpace(KeywordFilter);
    }

    public class SessionSearchResultsViewModel
    {
        public string DayFilter { get; set; } = string.Empty;
        public string KeywordFilter { get; set; } = string.Empty;
        public string Query { get; set; } = "";
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = SessionSearchRequest.PAGE_SIZE;
        public int TotalPages { get; set; } = 0;
        public int TotalHits { get; set; } = 0;
        public IEnumerable<SessionSearchIndexModel> Hits { get; } = [];

        public static SessionSearchResultsViewModel Empty(SessionSearchRequest request) => new()
        {
            DayFilter = request.DayFilter,
            KeywordFilter = request.KeywordFilter,
            Query = request.SearchText,
            Page = request.PageNumber,
            PageSize = request.PageSize,
        };

        public SessionSearchResultsViewModel(TopDocs topDocs, SessionSearchRequest request, Func<ScoreDoc, Document> retrieveDoc)
        {
            int pageSize = Math.Max(1, request.PageSize);
            int pageNumber = Math.Max(1, request.PageNumber);
            int offset = pageSize * (pageNumber - 1);
            int limit = pageSize;

            Query = request.SearchText ?? "";
            Page = pageNumber;
            TotalPages = topDocs.TotalHits <= 0 ? 0 : (topDocs.TotalHits - 1) / pageSize + 1;
            TotalHits = topDocs.TotalHits;
            Hits = (IEnumerable<SessionSearchIndexModel>)topDocs.ScoreDocs
                .Skip(offset)
                .Take(limit)
                .Select(d => FromDocument(retrieveDoc(d)))
                .ToList();
        }

        private SessionSearchResultsViewModel()
        {
        }

    }
    public class SessionSearchService(
        ILuceneSearchService luceneSearchService,
        IEventLogService log,
        SessionSearchIndexingStrategy sessionSearchStrategy,
        ILuceneIndexManager indexManager)
    {
        private const int PHRASE_SLOP = 3;
        private const int MAX_RESULTS = 1000;

        private readonly ILuceneSearchService luceneSearchService = luceneSearchService;
        private readonly IEventLogService log = log;
        private readonly SessionSearchIndexingStrategy siteSearchStrategy = sessionSearchStrategy;
        private readonly ILuceneIndexManager indexManager = indexManager;

        public SessionSearchResultsViewModel SearchSessions(SessionSearchRequest request)
        {
            var index = indexManager.GetRequiredIndex(IndexName);

            var query = GetSessionTermQuery(request);

            var combinedQuery = new BooleanQuery
            {
                { query, Occur.MUST }
            };

            //if (request.Type is string facet)
            //{
            //    var drillDownQuery = new DrillDownQuery(articleSearchStrategy.FacetsConfigFactory());

            //    string[] subFacets = facet.Split(';', StringSplitOptions.RemoveEmptyEntries);

            //    foreach (string subFacet in subFacets)
            //    {
            //        drillDownQuery.Add(nameof(TaxonomyFacetField), subFacet);
            //    }

            //    combinedQuery.Add(drillDownQuery, Occur.MUST);
            //}

            try
            {
                return luceneSearchService.UseSearcher(
                    index,
                    searcher =>
                    {
                        //var sortOptions = GetSortOption(request.SortBy);
                        //var chosenSubFacets = new List<string>();

                        //TermFilter(new Term("Day", request.DayFilter ?? string.Empty));
                        int pageSize = Math.Max(1, request.PageSize);
                        int pageNumber = Math.Max(1, request.PageNumber);
                        int offset = pageSize * (pageNumber - 1);
                        int limit = pageSize;

                        TopDocs topDocs = topDocs = searcher.Search(combinedQuery, MAX_RESULTS);/*string.IsNullOrEmpty(request.DayFilter) ? topDocs = searcher.Search(combinedQuery, MAX_RESULTS) : */

                        //This is my attempt / test at getting the search highlighting working, but it breaks the search results from returning altogether so it is commented out.

                        //var htmlFormatter = new SimpleHTMLFormatter();
                        //var queryScorer = new QueryScorer(query);
                        //var highlighter = new Highlighter(htmlFormatter, queryScorer);
                        //foreach (var found in topDocs.ScoreDocs)
                        //{
                        //    var document = searcher.Doc(found.Doc);
                        //    var pc = document.Get("PageContent");
                        //    var pcFragment = highlighter.GetBestFragment(new StandardAnalyzer(LuceneVersion.LUCENE_48), "PageContent", pc);
                        //    Console.WriteLine(pcFragment);
                        //}




                        return new SessionSearchResultsViewModel(topDocs, request, d => searcher.Doc(d.Doc));
                    }
                );
            }
            catch (Exception ex)
            {
                log.LogException(nameof(SessionSearchService), "SESSION_SEARCH_FAILURE", ex);

                return SessionSearchResultsViewModel.Empty(request);
            }
        }

        public static Query GetSessionTermQuery(SessionSearchRequest request)
        {
            string searchText = request.SearchText.Trim();

            if (request.AreFiltersDefault)
            {
                return new MatchAllDocsQuery();
            }

            var booleanQuery = new BooleanQuery();

            if (!string.IsNullOrWhiteSpace(searchText) || !string.IsNullOrWhiteSpace(request.DayFilter) || !string.IsNullOrWhiteSpace(request.KeywordFilter))
            {
                var analyzer = new StandardAnalyzer(LuceneVersion.LUCENE_48);
                var queryBuilder = new QueryBuilder(analyzer);

                if (!string.IsNullOrWhiteSpace(searchText))
                {
                    var titleQuery = queryBuilder.CreatePhraseQuery(nameof(SessionSearchIndexModel.Title), searchText, PHRASE_SLOP);
                    booleanQuery = AddToTermQuery(booleanQuery, titleQuery, 5);

                    var contentQuery = queryBuilder.CreatePhraseQuery(nameof(SessionSearchIndexModel.PageContent), searchText, PHRASE_SLOP);
                    booleanQuery = AddToTermQuery(booleanQuery, contentQuery, 1);

                    var titleShould = queryBuilder.CreateBooleanQuery(nameof(SessionSearchIndexModel.Title), searchText, Occur.SHOULD);
                    booleanQuery = AddToTermQuery(booleanQuery, titleShould, 0.5f);

                    var contentShould = queryBuilder.CreateBooleanQuery(nameof(SessionSearchIndexModel.PageContent), searchText, Occur.SHOULD);
                    booleanQuery = AddToTermQuery(booleanQuery, contentShould, 0.1f);
                }

                if (!string.IsNullOrEmpty(request.DayFilter))
                {
                    var dayFilter = new TermQuery(new Term("StartTime", request.DayFilter));
                    booleanQuery = AddToTermQuery(booleanQuery, dayFilter, 5);
                }

                if (!string.IsNullOrEmpty(request.KeywordFilter))
                {
                    var trackQuery = queryBuilder.CreatePhraseQuery(nameof(SessionSearchIndexModel.Keyword), request.KeywordFilter, PHRASE_SLOP);
                    booleanQuery = AddToTermQueryMust(booleanQuery, trackQuery, 5);

                    //var trackFilter = new TermQuery(new Term("Keyword", request.KeywordFilter));
                    //booleanQuery = AddToTermQuery(booleanQuery, trackFilter, 5);
                }
            }

            return booleanQuery;
        }

        private static BooleanQuery AddToTermQuery(BooleanQuery query, Query textQueryPart, float boost)
        {
            textQueryPart.Boost = boost;
            query.Add(textQueryPart, Occur.SHOULD);

            return query;
        }

        private static BooleanQuery AddToTermQueryMust(BooleanQuery query, Query textQueryPart, float boost)
        {
            textQueryPart.Boost = boost;
            query.Add(textQueryPart, Occur.MUST);

            return query;
        }

        //private static SortField? GetSortOption(string? sortBy = null) =>
        //    sortBy switch
        //    {
        //        "publishdate" => new SortField(nameof(SessionSearchIndexModel.IssueDate), FieldCache.NUMERIC_UTILS_INT64_PARSER, true),
        //        _ => null,
        //    };
    }
}
