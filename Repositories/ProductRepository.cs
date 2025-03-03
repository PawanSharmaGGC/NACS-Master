using System;
using System.Collections.Generic;
using System.Linq;
using CMS.ContentEngine;
using CMS.DataEngine;
using CMS.Websites;
using CMS.Websites.Routing;
using Convenience.org.Repositories.Interfaces;

namespace Convenience.org.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly IContentQueryExecutor _executor;
        private readonly IWebsiteChannelContext _channelContext;

        public ProductRepository(IContentQueryExecutor executor, IWebsiteChannelContext channelContext)
        {
            _executor = executor;
            _channelContext = channelContext;
        }

        public Product GetProduct(Guid WebPageGuid)
        {
            WhereCondition condition = new WhereCondition();
            condition.WhereCondition = "WebPageItemGUID=" + WebPageGuid;

            // Prepares a query that retrieves products pages matching the selected GUIDs
            var pageQuery = new ContentItemQueryBuilder()
                    .ForContentType(Product.CONTENT_TYPE_NAME,
                query =>
                        query.Where(x=>x.WhereEquals(nameof(WebPageFields.ContentItemGUID),WebPageGuid)));

            IEnumerable<Product> events =
                     _executor.GetMappedWebPageResult<Product>(pageQuery)?.Result;

            return events.FirstOrDefault() ?? null;
        }

        public List<Product> GetProducts(List<Guid> WebPageGuids)
        {
            // Prepares a query that retrieves products pages matching the selected GUIDs
            var pageQuery = new ContentItemQueryBuilder()
                    .ForContentTypes(parameters =>
                        parameters.ForWebsite(WebPageGuids).OfContentType(Product.CONTENT_TYPE_NAME).WithContentTypeFields());

            IEnumerable<Product> events =
                     _executor.GetMappedWebPageResult<Product>(pageQuery)?.Result;

            return events.ToList() ?? new List<Product>();
        }
    }
}
