using CMS.ContentEngine;

using NACS.Portal.Core.Operations;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NACSMagazine.Infrastructure
{
    public record WebsiteSettingsContentQuery : NACS.Portal.Core.Operations.IQuery<WebsiteSettingsContent>;
    public record WEbsiteSettingsContentQueryResponse(WebsiteSettingsContent Settings);
    
    public class WebsiteSettingsContentQueryHandler(ContentItemQueryTools tools) : ContentItemQueryHandler<WebsiteSettingsContentQuery, WebsiteSettingsContentQuery>(tools)
    {
        public override async Task<WebsiteSettingsContent> Handle(WebsiteSettingsContentQuery request, CancellationToken cancellationToken)
        {
            var b = new ContentItemQueryBuilder().ForContentType(WebsiteSettingsContent.CONTENT_TYPE_NAME, p => p
            .TopN(1)
            .Columns([
                nameof(WebsiteSettingsContent.WebsiteSettingsContentAlertBoxContentHTML),
                nameof(WebsiteSettingsContent.WebsiteSettingsContentAlertBoxCookieExpirationDays),
                nameof(WebsiteSettingsContent.WebsiteSettingsContentCookiebannerContentHTML),
                nameof(WebsiteSettingsContent.WebsiteSettingsContentCookieBannerHeading),
                nameof(WebsiteSettingsContent.WebsiteSettingsContentExceptionContentHTML),
                nameof(WebsiteSettingsContent.WebsiteSettingsContentFallbackOGMediaFileImage),
                nameof(WebsiteSettingsContent.WebsiteSettingsContentFormsConfigurationJSON),
                nameof(WebsiteSettingsContent.WebsiteSettingsContentIsAlertBoxEnabled),
                nameof(WebsiteSettingsContent.WebsiteSettingsContentNotFoundContentHTML),
                nameof(WebsiteSettingsContent.WebsiteSettingsContentPageTitleFormat),
                nameof(WebsiteSettingsContent.WebsiteSettingsContentRobotsTxt),
                nameof(WebsiteSettingsContent.WebsiteSettingsContentWebsiteDisplayName),
            ]));

            var r = await Executor.GetMappedResult<WebsiteSettingsContent>(b, DefaultQueryOptions, cancellationToken);

            return r.First();
        }

        protected override ICacheDependencyKeysBuilder AddDependencyKeys(WebsiteSettingsContentQuery query, WebsiteSettingsContentQuery result, ICacheDependencyKeysBuilder builder) =>
            builder.AllContentItems(WebsiteSettingsContent.CONTENT_TYPE_NAME);
    }
}
