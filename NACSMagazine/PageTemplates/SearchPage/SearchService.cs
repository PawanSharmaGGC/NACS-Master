using CMS.Core;

using Kentico.Xperience.Lucene.Core.Indexing;
using Kentico.Xperience.Lucene.Core.Search;

using Lucene.Net.Analysis.Standard;
using Lucene.Net.Search;
using Lucene.Net.Facet;
using Lucene.Net.Util;

using Microsoft.AspNetCore.Http;

using static NACSMagazine.PageTemplates.SearchPage.ArticleSearchIndexModel;

namespace NACSMagazine.PageTemplates.SearchPage
{
    public class ArticleSearchRequest
    {
        public const int PAGE_SIZE = 10;

        public ArticleSearchRequest(HttpRequest request)
        {
            var query = request.Query;

            Type = query.TryGetValue("Type", out var facetValues)
                ? facetValues.ToString()
                : "";
            SearchText = query.TryGetValue("query", out var queryValues)
                ? queryValues.ToString()
                : "";
            SortBy = query.TryGetValue("sortBy", out var sortByValues)
                ? sortByValues.ToString()
                : "publishdate";
            PageNumber = query.TryGetValue("page", out var pageValues)
                ? int.TryParse(pageValues, out int p)
                    ? p
                    : 1
                : 1;
        }

        public ArticleSearchRequest(string sortBy, int pageSize)
        {
            SortBy = sortBy;
            PageSize = pageSize;

        }

        public string Type { get; set; } = "";
        public string SearchText { get; set; } = "";
        public string SortBy { get; set; } = "";
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = PAGE_SIZE;
        public bool AreFiltersDefault => string.IsNullOrWhiteSpace(SearchText);
    }

    public class ArticleSearchResultViewModel<T> : LuceneSearchResultModel<T>
    {
        public string SortBy { get; set; } = "";
    }
    public class ArticleSearchService(
        ILuceneSearchService luceneSearchService,
        IEventLogService log,
        ArticleSearchIndexingStrategy articleSearchStrategy,
        ILuceneIndexManager indexManager)
    {
        private const int PHRASE_SLOP = 3;
        private const int MAX_RESULTS = 1000;

        private readonly ILuceneSearchService luceneSearchService = luceneSearchService;
        private readonly IEventLogService log = log;
        private readonly ArticleSearchIndexingStrategy siteSearchStrategy = articleSearchStrategy;
        private readonly ILuceneIndexManager indexManager = indexManager;

        public LuceneSearchResultModel<Article> SearchArticle(ArticleSearchRequest request)
        {
            var index = indexManager.GetRequiredIndex(IndexName);

            var query = GetArticleTermQuery(request);

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
                        var sortOptions = GetSortOption(request.SortBy);
                        //var chosenSubFacets = new List<string>();
                        int pageSize = Math.Max(1, request.PageSize);
                        int pageNumber = Math.Max(1, request.PageNumber);
                        int offset = pageSize * (pageNumber - 1);
                        int limit = pageSize;

                        TopDocs topDocs = sortOptions is null
                            ? topDocs = searcher.Search(combinedQuery, MAX_RESULTS)
                            : topDocs = searcher.Search(combinedQuery, MAX_RESULTS, new Sort(sortOptions));

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

                        return new ArticleSearchResultViewModel<Article>
                        {
                            Query = request.SearchText ?? "",
                            Page = pageNumber,
                            PageSize = pageSize,
                            TotalPages = topDocs.TotalHits <= 0 ? 0 : (topDocs.TotalHits - 1) / pageSize + 1,
                            TotalHits = topDocs.TotalHits,
                            Hits = topDocs.ScoreDocs
                                    .Skip(offset)
                                    .Take(limit)
                                    .Select(d => FromDocument(searcher.Doc(d.Doc)))
                                    .ToList(),
                            //Facet = request.Type,
                            //Facets = facets?.GetTopChildren(10, nameof(TaxonomyFacetField), [.. chosenSubFacets])?.LabelValues.ToArray(),
                            SortBy = request.SortBy
                        };
                    }
                );
            }
            catch (Exception ex)
            {
                log.LogException(nameof(ArticleSearchService), "ARTICLE_SEARCH_FAILURE", ex);

                return new ArticleSearchResultViewModel<Article>
                {
                    //Facet = null,
                    //Facets = [],
                    Hits = [],
                    Page = request.PageNumber,
                    PageSize = request.PageSize,
                    Query = request.SearchText,
                    SortBy = request.SortBy,
                    TotalHits = 0,
                    TotalPages = 0
                };
            }
        }

        public static Query GetArticleTermQuery(ArticleSearchRequest request)
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
                var titleQuery = queryBuilder.CreatePhraseQuery(nameof(ArticleSearchIndexModel.Title), searchText, PHRASE_SLOP);
                booleanQuery = AddToTermQuery(booleanQuery, titleQuery, 5);

                var contentQuery = queryBuilder.CreatePhraseQuery(nameof(ArticleSearchIndexModel.PageContent), searchText, PHRASE_SLOP);
                booleanQuery = AddToTermQuery(booleanQuery, contentQuery, 1);

                var titleShould = queryBuilder.CreateBooleanQuery(nameof(ArticleSearchIndexModel.Title), searchText, Occur.SHOULD);
                booleanQuery = AddToTermQuery(booleanQuery, titleShould, 0.5f);

                var contentShould = queryBuilder.CreateBooleanQuery(nameof(ArticleSearchIndexModel.PageContent), searchText, Occur.SHOULD);
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
                "publishdate" => new SortField(nameof(ArticleSearchIndexModel.IssueDate), FieldCache.NUMERIC_UTILS_INT64_PARSER, true),
                _ => null,
            };
    }
}
