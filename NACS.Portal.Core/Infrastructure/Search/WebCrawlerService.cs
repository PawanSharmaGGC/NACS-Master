﻿using CMS.Core;
using CMS.Websites;

using Microsoft.Extensions.Options;

using Microsoft.Net.Http.Headers;

using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace NACS.Portal.Core.Infrastructure.Search
{
    public class WebCrawlerService
    {
        private readonly HttpClient httpClient;
        private readonly IWebPageUrlRetriever urlRetriever;
        private readonly IEventLogService log;

        public WebCrawlerService(HttpClient httpClient, IWebPageUrlRetriever urlRetriever, IEventLogService log, IOptions<NACSLuceneSearchOptions> searchOptions)
        {
            this.httpClient = httpClient;
            this.httpClient.DefaultRequestHeaders.Add(HeaderNames.UserAgent, "SearchCrawler");
            this.httpClient.BaseAddress = new System.Uri("https://localhost:65491");
            
            this.urlRetriever = urlRetriever;
            this.log = log;
        }

        public async Task<string> CrawlWebPage(IWebPageFieldsSource page)
        {
            try
            {
                var url = await urlRetriever.Retrieve(page);
                string path = url.RelativePath.TrimStart('~').TrimStart('/');

                return await CrawlPage(path);
            }
            catch (Exception ex)
            {

                log.LogException(nameof(WebCrawlerService), nameof(CrawlWebPage), ex, $"Tree Path: {page.SystemFields.WebPageItemTreePath}");
            }
            return "";
        }

        public async Task<string> CrawlPage(string url)
        {
            try
            {
                var response = await httpClient.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsStringAsync();
                }
            }
            catch (Exception ex)
            {
                log.LogException(nameof(WebCrawlerService), nameof(CrawlPage), ex, $"Url: {url}");
            }
            return "";
        }
    }
}
