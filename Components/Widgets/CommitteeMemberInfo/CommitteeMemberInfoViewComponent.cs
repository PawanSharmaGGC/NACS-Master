using CMS.Websites.Routing;
using Kentico.Content.Web.Mvc;
using Kentico.PageBuilder.Web.Mvc;
using Microsoft.AspNetCore.Mvc;
using CMS.Websites;
using System.Threading.Tasks;
using Convenience.org.Components.Widgets.CommitteeMemberInfo;
using System.Text;
using Microsoft.AspNetCore.Http;
using Convenience.org.Components.Widgets.CommitteeListing;

[assembly: RegisterWidget(identifier: CommitteeMemberInfoViewComponent.IDENTIFIER, name: "CommitteeMemberInfo",
    viewComponentType: typeof(CommitteeMemberInfoViewComponent), Description = "Display committee member information",
    IconClass = "icon-list", AllowCache = true)]

namespace Convenience.org.Components.Widgets.CommitteeMemberInfo
{
    public class CommitteeMemberInfoViewComponent : ViewComponent
    {
        public const string IDENTIFIER = "CommitteeMemberInfo";
        private readonly IWebPageDataContextRetriever _webPageDataContextRetriever;
        private readonly IWebPageUrlRetriever _webPageUrlRetriever;
        private readonly IWebsiteChannelContext _channelContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ICommitteeMemberInfoService _committeeMemberInfoService;
        public CommitteeMemberInfoViewComponent(ICommitteeMemberInfoService committeeMemberInfoService, IHttpContextAccessor httpContextAccessor, IWebPageDataContextRetriever webPageDataContextRetriever, IWebsiteChannelContext channelContext, IWebPageUrlRetriever? webPageUrlRetriever)
        {
            _webPageDataContextRetriever = webPageDataContextRetriever;
            _channelContext = channelContext;
            _webPageUrlRetriever = webPageUrlRetriever;
            _httpContextAccessor = httpContextAccessor;
            _committeeMemberInfoService = committeeMemberInfoService;
        }

        public async Task<IViewComponentResult> InvokeAsync(ComponentViewModel componentViewModel)
        {
            var currentPage = await _webPageUrlRetriever!.Retrieve(componentViewModel.Page.WebPageItemID, "en");
            var vm = new CommitteeMemberInfoViewModel();
            string id = "783341"; //_httpContextAccessor.HttpContext.Request.Query["id"];
            string photo = string.Empty;

            //TBD need to get current page data and NACS_ID based on a content
            //string id = currentPage.GetStringValue("NACS_ID", "");
            //photo = currentPage.GetStringValue("RollupImage", "");

            if (id != "")
            {
                var memberships = _committeeMemberInfoService.GetCommitteeMembershipsAsync(id);

                if (memberships != null && memberships.Result != null)
                {

                    StringBuilder sbBio = new StringBuilder();

                    sbBio.Append("<div class='committee-members'>");

                    sbBio.Append("<div class='row'>");

                    sbBio.Append("  <div class='member-image'><img src='" + photo + "'></div>");

                    if (memberships.Result.NACSID != "")
                    {
                        sbBio.Append("  <div class='member-info'>");
                        sbBio.Append("  <a class='button' href='/Convenience.org/applicationpages/PrintCommitteeMemberPDF.aspx?nacsid=" + memberships.Result.NACSID + "' style='float:right' target='blanc'>Print PDF</a>");
                        sbBio.Append("      <h2 class='ArticleTitle'>" + memberships.Result.FirstName + " " + memberships.Result.LastName + "<br /></h2> <span class='ArticleSubTitle'>" + memberships.Result.Title + ",&nbsp;" + memberships.Result.Company + "</span>");
                        sbBio.Append("  </div>");
                    }

                    sbBio.Append("</div>");

                    if (memberships.Result.NACSID != "")
                    {
                        sbBio.Append("<div class='row'>");
                        sbBio.Append("  <div class='col-12 col-sm-4 col-left'>" + memberships.Result.Company + "<br />");
                        if (!string.IsNullOrEmpty(memberships.Result.Stores) && memberships.Result.Stores != "0")
                        {
                            sbBio.Append(memberships.Result.Stores + " stores <br />");
                        }
                        if (!string.IsNullOrEmpty(memberships.Result.Website))
                        {
                            sbBio.Append("<a href='" + memberships.Result.Website + "' target='_blank'>" + memberships.Result.Website.Remove(0, 7) + "</a>");
                        }
                        sbBio.Append("  </div>");
                        sbBio.Append("  <div class='col-12 col-sm-4 col-middle'>" + memberships.Result.Address1 + "<br />");
                        if (!string.IsNullOrEmpty(memberships.Result.Address2))
                        {
                            sbBio.Append("      " + memberships.Result.Address2 + "<br />");
                        }
                        sbBio.Append("      " + memberships.Result.CityStateZip);
                        sbBio.Append("  </div>");
                        sbBio.Append("  <div class='col-12 col-sm-4 col-right'>" + memberships.Result.Phone + "&nbsp;p<br />");
                        if (!string.IsNullOrEmpty(memberships.Result.Fax))
                        {
                            sbBio.Append(memberships.Result.Fax + "&nbsp;f<br />");
                        }
                        sbBio.Append("      <a href='mailto:" + memberships.Result.Email + "'>" + memberships.Result.Email + "</a>");
                        sbBio.Append("  </div>");
                        sbBio.Append("</div>");

                        // serving on
                        sbBio.Append("<div class='row'>");
                        sbBio.Append("  <div class='col-12 serving-label'>");
                        sbBio.Append("      Serving on:");
                        sbBio.Append("  </div>");
                        sbBio.Append("  <div class='col-12 serving-container'>");
                        sbBio.Append("      <div class='row'>");
                        if (memberships.Result.Committees!=null)
                        {
                            foreach (string committee in memberships.Result.Committees)
                            {
                                sbBio.AppendFormat("<div class='committee-item col-12 col-sm-6'>{0}{1}</div>", "<i class='fa fa-users fa-lg'></i>&nbsp;", committee);
                            }
                        }
                        sbBio.Append("      </div>");
                        sbBio.Append("  </div>");
                        sbBio.Append("</div>");
                    }
                    else
                    {
                        sbBio.Append("<br/><br/><p><strong style='color:#CC0000'>NOTE: This person is no longer serving on any committees.</strong></p><br/><br/>");
                    }

                    sbBio.Append("</div>");

                    vm.PrimaryContent = sbBio.ToString();
                }
            }
            return View("~/Components/Widgets/CommitteeMemberInfo/_CommitteeMemberInfo.cshtml", vm);
        }
    }
}