using CMS.DataEngine;
using CMS.FormEngine;

using NACS.Portal.Core.Operations;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NACSMagazine.PageTemplates.MagazineArticlePage
{
    public record ArticleTaxonomiesQuery() : IQuery<ArticleTaxonomiesQueryResponse>;


    public record ArticleTaxonomy(string Value, string DisplayName);
    public record ArticleTaxonomiesQueryResponse(IReadOnlyList<ArticleTaxonomy> Items, int ClassID);
    //public class ArticleTaxonomiesQueryHandler(ContentItemQueryTools tools) : ContentItemQueryHandler<ArticleTaxonomiesQuery, ArticleTaxonomiesQueryResponse>(tools)
    //{
    //    public override Task<ArticleTaxonomiesQueryResponse> Handle(ArticleTaxonomiesQuery request, CancellationToken cancellationToken = default)
    //    {
    //        var dc = DataClassInfoProvider.GetDataClassInfo(Article.CONTENT_TYPE_NAME);

    //        var form = new FormInfo(dc.ClassFormDefinition);

    //        var field = form.GetFormField(nameof(Article.ArticleContentTaxonomy));

    //        if (!field.Settings.ContainsKey("Options") || field.Settings["Options"] is not string options)
    //        {
    //            return Task.FromResult(new ArticleTaxonomiesQueryResponse([], dc.ClassID));
    //        }

    //        var taxonomies = options
    //            .Split("\n")
    //            .Select(o => o.Split(";"))
    //            .Where(kv => kv.Length is > 0 and <= 2)
    //            .Select(kv => kv switch
    //            {
    //            [var key] => new ArticleTaxonomy(key.Trim(), key.Trim()),
    //            [var key, var value] => new ArticleTaxonomy(key.Trim(), value.Trim()),
    //                _ => throw new ArgumentException("Invalid number of elements"),
    //            })
    //                .ToList();

    //        return Task.FromResult(new ArticleTaxonomiesQueryResponse(taxonomies, dc.ClassID));
    //    }

    //    protected override ICacheDependencyKeysBuilder AddDependencyKeys(ArticleTaxonomiesQuery query, ArticleTaxonomiesQueryResponse result, ICacheDependencyKeysBuilder builder) =>
    //        builder.Object(DataClassInfo.OBJECT_TYPE, result.ClassID);
    //}
}
