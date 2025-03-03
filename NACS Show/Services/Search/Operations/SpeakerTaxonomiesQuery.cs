using CMS.ContentEngine;

using NACS.Portal.Core.Operations;

using NACSShow.Modules;

using System.Collections.Frozen;
using System.Collections.Immutable;

namespace NACSShow.Services.Search.Operations
{
    public record SpeakerTaxonomiesQuery() : IQuery<SpeakerTaxonomiesQueryResponse>;

    public record SpeakerTaxonomiesQueryResponse(IReadOnlyList<TaxonomyTag> Tracks);

    public class SpeakerTaxonomiesQueryHandler(DataItemQueryTools tools, ITaxonomyRetriever taxonomyRetriever) : DataItemQueryHandler<SpeakerTaxonomiesQuery, SpeakerTaxonomiesQueryResponse>(tools)
    {
        private readonly ITaxonomyRetriever taxonomyRetriever = taxonomyRetriever;

        public override async Task<SpeakerTaxonomiesQueryResponse> Handle(SpeakerTaxonomiesQuery request, CancellationToken cancellationToken = default)
        {
            var typeTaxonomy = await taxonomyRetriever.RetrieveTaxonomy("NSSpeakerTracks", NACSShowWebsiteChannel.DEFAULT_LANGUAGE, cancellationToken);
            var childTypeTags = new Dictionary<int, ImmutableList<CMS.ContentEngine.Tag>>().ToFrozenDictionary();
            var typeTags = typeTaxonomy
                .Tags
                .OrderBy(t => t.Title)
                .Select(t => new TaxonomyTag(t, childTypeTags))
                .ToList();

            return new SpeakerTaxonomiesQueryResponse(typeTags);
        }

        protected override ICacheDependencyKeysBuilder AddDependencyKeys(SpeakerTaxonomiesQuery query, SpeakerTaxonomiesQueryResponse result, ICacheDependencyKeysBuilder builder) =>
            builder.Object(TaxonomyInfo.OBJECT_TYPE, "NSSpeakerTracks")
            .Collection(result.Tracks, i => builder.Object(TagInfo.OBJECT_TYPE, i.Name));

    }
}
