using CMS.Core;

using Kentico.Xperience.Lucene.Core.Indexing;
using Kentico.Xperience.Lucene.Core.Search;

using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Facet;
using Lucene.Net.Search;
using Lucene.Net.Util;

using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;

using static NACSShow.Services.Search.SiteSearch.SiteSearchIndexModel;

namespace NACSShow.Services.Search.SiteSearch
{
    public class SiteSearchRequest
    {
        public const int PAGE_SIZE = 10;

        public SiteSearchRequest(HttpRequest request)
        {
            var query = request.Query;

            SearchText = query.TryGetValue("query", out var queryValues)
                ? queryValues.ToString()
                : "";
            Types = query.TryGetValue("types", out var typeValue)
                ? (!string.IsNullOrWhiteSpace(typeValue) ? [.. typeValue] : new List<string>()) : [];
            //SortBy = query.TryGetValue("sortBy", out var sortByValues)
            //    ? sortByValues.ToString()
            //    : "publisheddate";
            PageNumber = query.TryGetValue("page", out var pageValues)
                ? int.TryParse(pageValues, out int p)
                    ? p
                    : 1
                : 1;
        }

        public SiteSearchRequest(string sortBy, int pageSize)
        {
            //SortBy = sortBy;
            PageSize = pageSize;
            
        }

        public IEnumerable<string> Types { get; } = [];
        public string SearchText { get; set; } = "";
        //public string SortBy { get; set; } = "";
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = PAGE_SIZE;
        public bool AreFiltersDefault => string.IsNullOrWhiteSpace(SearchText);
    }

    public class SiteSearchResults
    {
        
        public string Query { get; set; } = "";
        public LabelAndValue[] Types { get; } = [];
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = SiteSearchRequest.PAGE_SIZE;
        public int TotalPages { get; set; } = 0;
        public int TotalHits { get; set; } = 0;
        public IEnumerable<SiteSearchIndexModel> Hits { get; } = [];
        //public string SortBy { get; init; } = "";

        public static SiteSearchResults Empty(SiteSearchRequest request) => new()
        {
            Query = request.SearchText,
            //SortBy = request.SortBy,
            Page = request.PageNumber,
            PageSize = request.PageSize,
        };

        public SiteSearchResults(TopDocs topDocs, SiteSearchRequest request, Func<ScoreDoc, Document> retrieveDoc) //MultiFacets facets,
        {
            int pageSize = Math.Max(1, request.PageSize);
            int pageNumber = Math.Max(1, request.PageNumber);
            int offset = pageSize * (pageNumber - 1);
            int limit = pageSize;

            Query = request.SearchText ?? "";
            
            //Types = facets.GetTopChildren(100, nameof(SiteSearchIndexModel.TypeFacet), [])?.LabelValues.ToArray() ?? [];
            Page = pageNumber;
            TotalPages = topDocs.TotalHits <= 0 ? 0 : (topDocs.TotalHits - 1) / pageSize + 1;
            TotalHits = topDocs.TotalHits;
            Hits = topDocs.ScoreDocs
                .Skip(offset)
                .Take(limit)
                .Select(d => FromDocument(retrieveDoc(d)))
                .ToList();
            //SortBy = request.SortBy;
        }

        private SiteSearchResults()
        {
        }

    }
    public class SiteSearchService(
        ILuceneSearchService luceneSearchService,
        IEventLogService log,
        SiteSearchIndexingStrategy siteSearchStrategy,
        ILuceneIndexManager indexManager)
    {
        private const int PHRASE_SLOP = 3;
        private const int MAX_RESULTS = 1000;

        private readonly ILuceneSearchService luceneSearchService = luceneSearchService;
        private readonly IEventLogService log = log;
        private readonly SiteSearchIndexingStrategy siteSearchStrategy = siteSearchStrategy;
        private readonly ILuceneIndexManager indexManager = indexManager;

        public SiteSearchResults SearchSite(SiteSearchRequest request)
        {
            var index = indexManager.GetRequiredIndex(IndexName);

            var query = GetSiteTermQuery(request);

            var combinedQuery = AddFacetsToQuery(
                request,
                new BooleanQuery
            {
                { query, Occur.MUST }
            });

            try
            {
                return luceneSearchService.UseSearcher( //.UseSearcherWithFacets(
                    index,
                    //combinedQuery, 20,
                    (searcher => //, facets) =>
                    {
                        //var sortOptions = GetSortOption(request.SortBy);

                        //int pageSize = Math.Max(1, request.PageSize);
                        //int pageNumber = Math.Max(1, request.PageNumber);
                        //int offset = pageSize * (pageNumber - 1);
                        //int limit = pageSize;

                        //TopDocs topDocs =  sortOptions is null
                        //? 
                        TopDocs topDocs = searcher.Search(combinedQuery, MAX_RESULTS);
                        //: searcher.Search(combinedQuery, MAX_RESULTS, new Sort(sortOptions));
                        
                        /*string.IsNullOrEmpty(request.DayFilter) ? topDocs = searcher.Search(combinedQuery, MAX_RESULTS) : */

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

                        return new SiteSearchResults(topDocs, request, d => searcher.Doc(d.Doc)); //facets, 
                    }
                ));
            }
            catch (Exception ex)
            {
                log.LogException(nameof(SiteSearchService), "SITE_SEARCH_FAILURE", ex);

                return SiteSearchResults.Empty(request);
            }
        }

        private Query AddFacetsToQuery(SiteSearchRequest request, Query baseQuery)
        {
            if (!request.Types.Any())
            {
                return baseQuery;
            }

            var drillDownQuery = new DrillDownQuery(siteSearchStrategy.FacetsConfigFactory(), baseQuery);
            
            foreach(string type in request.Types)
            {
                drillDownQuery.Add(nameof(SiteSearchIndexModel.TypeFacet), type.ToLowerInvariant());
            };

            return drillDownQuery;
        }

        public static Query GetSiteTermQuery(SiteSearchRequest request)
        {
            string searchText = request.SearchText.Trim();

            if (request.AreFiltersDefault)
            {
                return new MatchAllDocsQuery();
            }

            var booleanQuery = new BooleanQuery();

            if (!string.IsNullOrWhiteSpace(searchText))
            {
                var analyzer = new StandardAnalyzer(LuceneVersion.LUCENE_48);
                var queryBuilder = new QueryBuilder(analyzer);

                var titleQuery = queryBuilder.CreatePhraseQuery(nameof(SiteSearchIndexModel.Title), searchText, PHRASE_SLOP);
                booleanQuery = AddToTermQuery(booleanQuery, titleQuery, 5);

                var contentQuery = queryBuilder.CreatePhraseQuery(nameof(SiteSearchIndexModel.Content), searchText, PHRASE_SLOP);
                booleanQuery = AddToTermQuery(booleanQuery, contentQuery, 1);

                var titleShould = queryBuilder.CreateBooleanQuery(nameof(SiteSearchIndexModel.Title), searchText, Occur.SHOULD);
                booleanQuery = AddToTermQuery(booleanQuery, titleShould, 0.5f);

                var contentShould = queryBuilder.CreateBooleanQuery(nameof(SiteSearchIndexModel.Content), searchText, Occur.SHOULD);
                booleanQuery = AddToTermQuery(booleanQuery, contentShould, 0.1f);
                
            }

            return booleanQuery;
        }

        private static BooleanQuery AddToTermQuery(BooleanQuery query, Query textQueryPart, float boost)
        {
            textQueryPart.Boost = boost;
            query.Add(textQueryPart, Occur.SHOULD);

            return query;
        }

        private static SortField? GetSortOption(string? sortBy = null) =>
            sortBy switch
            {
                "publisheddate" => new SortField(nameof(SiteSearchIndexModel.PublishedDate), FieldCache.NUMERIC_UTILS_INT64_PARSER, true),
                _ => null,
            };
    }
}
