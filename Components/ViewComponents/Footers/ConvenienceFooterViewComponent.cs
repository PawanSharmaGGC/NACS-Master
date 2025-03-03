using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CMS.ContentEngine;
using CMS.DataEngine;
using CMS.MediaLibrary;
using CMS.Websites;
using CMS.Websites.Routing;
using Convenience.org.Models;
using Convenience.org.Repositories.Interfaces;
using Kentico.Content.Web.Mvc;
using Microsoft.AspNetCore.Mvc;


namespace Convenience.org.Components.ViewComponents.Footers
{
    public class ConvenienceFooterViewComponent : ViewComponent
    {
        private readonly IContentQueryExecutor executor;
        private readonly IWebsiteChannelContext channelContext;
        private readonly IMediaFileUrlRetriever mediaFileUrlRetriever;
        private readonly INavbarRepository navBarRepository;

        public ConvenienceFooterViewComponent(IContentQueryExecutor executor, IWebsiteChannelContext channelContext,
            IMediaFileUrlRetriever mediaFileUrlRetriever, INavbarRepository navBarRepository)
        {
            this.channelContext = channelContext;
            this.executor = executor;
            this.mediaFileUrlRetriever = mediaFileUrlRetriever;
            this.navBarRepository = navBarRepository;
        }


        public async Task<IViewComponentResult> InvokeAsync()
        {
            FooterViewModel viewModel = await GetFooter();
            return View("/Components/ViewComponents/Footers/ConvenienceFooter.cshtml", viewModel);
        }

        /// <summary>
        /// Get footer details
        /// </summary>
        /// <returns></returns>
        public async Task<FooterViewModel> GetFooter()
        {
            FooterViewModel footerViewModel = new FooterViewModel();
            var query = new ContentItemQueryBuilder()
               .ForContentType(
                       contentTypeName: Footer.CONTENT_TYPE_NAME,
                       configureQuery: config => config
                           .ForWebsite(channelContext.WebsiteChannelName)
                           .OrderBy("WebPageItemOrder"));

            // Materializes the query
            IEnumerable<Footer> footerdata = await executor.GetMappedResult<Footer>(query);

            if (footerdata != null)
            {
                footerViewModel = footerViewModel.GetViewModel(footerdata.FirstOrDefault());
                //Get footer logo
                IEnumerable<MediaFileInfo> mediaFileInfos =
                    new ObjectQuery<MediaFileInfo>().ForAssets(footerdata.FirstOrDefault().FooterLogo).GetEnumerableTypedResult();
                footerViewModel.FooterLogo = mediaFileUrlRetriever.Retrieve(mediaFileInfos.FirstOrDefault())?.RelativePath;

                //Get footer clip image
               MediaFileInfo clipImgMediaFileInfo =
                    new ObjectQuery<MediaFileInfo>().ForAssets(footerdata.FirstOrDefault().BgClipImage).GetEnumerableTypedResult().FirstOrDefault();
                footerViewModel.ClipImageSrc = mediaFileUrlRetriever.Retrieve(clipImgMediaFileInfo).RelativePath;
            }

            if (footerViewModel != null)
            {
                footerViewModel.FooterLinks = navBarRepository.GetNavItems("/Footer");
                footerViewModel.FooterPrivacyLinks = navBarRepository.GetNavItems("/Footer/Terms_and_Privacy");
                footerViewModel.SocialMediaLinks = navBarRepository.GetNavItems("/Footer/Social_Media_Links");
            }

            return footerViewModel;
        }
    }
}
