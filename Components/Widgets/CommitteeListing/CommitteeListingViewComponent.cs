using CMS.Membership;
using Convenience.org.Components.Widgets.CommitteeListing;
using Kentico.PageBuilder.Web.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

[assembly: RegisterWidget(identifier: CommitteeListingViewComponent.IDENTIFIER, name: "CommitteeListing",
    viewComponentType: typeof(CommitteeListingViewComponent), Description = "Displays a list of committee members",
    IconClass = "icon-list", AllowCache = true)]

namespace Convenience.org.Components.Widgets.CommitteeListing
{
    public class CommitteeListingViewComponent : ViewComponent
    {
        public const string IDENTIFIER = "CommitteeListing";
        private readonly ICommitteeService _committeeService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public CommitteeListingViewComponent(ICommitteeService committeeService, IHttpContextAccessor httpContextAccessor)
        {
            _committeeService = committeeService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IViewComponentResult> InvokeAsync(ComponentViewModel componentViewModel)
        {
            UserInfo currentUser = MembershipContext.AuthenticatedUser;
            var vm = new CommitteeListingViewModel();
            string _committeeId = _httpContextAccessor.HttpContext.Request.Query["cid"];
            if (!string.IsNullOrEmpty(_committeeId))
            {
                vm.Members = await _committeeService.GetCommitteeMembersAsync(_committeeId);
            }
            return View("~/Components/Widgets/CommitteeListing/_CommitteeListing.cshtml", vm);
        }
    }
}
