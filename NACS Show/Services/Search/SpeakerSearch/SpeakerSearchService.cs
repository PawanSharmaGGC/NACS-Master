using CMS.ContentEngine;
using CMS.Core;
using CMS.DataEngine;

using Kentico.Xperience.Lucene.Core.Indexing;
using Kentico.Xperience.Lucene.Core.Search;

using Lucene.Net.Analysis.Standard;
using Lucene.Net.Search;
using Lucene.Net.Util;

using NACSShow;

using NACSShow.Services.Search.SessionSearch;

using static NACSShow.Services.Search.SpeakerSearch.SpeakerSearchIndexModel;


namespace NACSShow.Services.Search.SpeakerSearch
{
    public class SpeakerSearchService(
        ILuceneSearchService luceneSearchService,
        IEventLogService log,
        SpeakerSearchIndexingStrategy speakerSearchStrategy,
        ILuceneIndexManager indexManager,
        IContentQueryExecutor executor)
    {
        private const int PHRASE_SLOP = 3;
        private const int MAX_RESULTS = 1000;

        private readonly ILuceneSearchService luceneSearchService = luceneSearchService;
        private readonly IEventLogService log = log;
        private readonly SpeakerSearchIndexingStrategy speakerSearchStrategy = speakerSearchStrategy;
        private readonly ILuceneIndexManager indexManager = indexManager;
        private readonly IContentQueryExecutor executor = executor;


        public async Task<SpeakerSearchResultsViewModel> SearchSpeakersAsync(SpeakerSearchRequest request)
        {
            var index = indexManager.GetRequiredIndex(IndexName);
            var query = GetSpeakerTermQuery(request);

            //Get companies from speakers to display in dropdown for filtering
            var builder = new ContentItemQueryBuilder()
                            .ForContentType("NACSShow.Speaker",
                            c => c
                            .Columns("Company"))
                            .InLanguage("en");                            


            var speakers = await executor.GetMappedResult<Speaker>(builder);

            var companies = new List<string>();
            foreach (var speaker in speakers)
            {
                if (!companies.Contains(speaker.Company))
                {
                    companies.Add(speaker.Company);
                }
            }
            
            
            //var combinedQuery = new BooleanQuery
            //{
            //    { query, Occur.MUST }
            //};

            try
            {
                return luceneSearchService.UseSearcher(
                    index, searcher =>
                        {
                            var topDocs = searcher.Search(query, MAX_RESULTS);
                            return new SpeakerSearchResultsViewModel(topDocs, request, doc => searcher.Doc(doc.Doc), companies);
                        });
            }
            catch (Exception ex)
            {

                log.LogException(nameof(SpeakerSearchService), "SPEAKER_SEARCH_FAILURE", ex);

                return SpeakerSearchResultsViewModel.Empty(request, companies);
            }
        }

        private Query GetSpeakerTermQuery(SpeakerSearchRequest request)
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
            }

            return booleanQuery;
        }

        private static BooleanQuery AddToTermQuery(BooleanQuery query, Query textQueryPart, float boost)
        {
            textQueryPart.Boost = boost;
            query.Add(textQueryPart, Occur.SHOULD);

            return query;
        }
    }
}
