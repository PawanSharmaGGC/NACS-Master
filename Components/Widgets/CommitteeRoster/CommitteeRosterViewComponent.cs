using CMS.Membership;
using Convenience.org.Components.Widgets.CommitteeRoster;
using Kentico.PageBuilder.Web.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

[assembly: RegisterWidget(identifier: CommitteeRosterViewComponent.IDENTIFIER, name: "CommitteeRoster",
    viewComponentType: typeof(CommitteeRosterViewComponent), Description = "Displays a list of committee roster",
    IconClass = "icon-list", AllowCache = true)]

namespace Convenience.org.Components.Widgets.CommitteeRoster
{
    public class CommitteeRosterViewComponent : ViewComponent
    {
        public const string IDENTIFIER = "CommitteeRoster";
        private readonly ICommitteeRosterService _committeeService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public CommitteeRosterViewComponent(ICommitteeRosterService committeeService, IHttpContextAccessor httpContextAccessor)
        {
            _committeeService = committeeService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IViewComponentResult> InvokeAsync(ComponentViewModel componentViewModel)
        {
            UserInfo currentUser = MembershipContext.AuthenticatedUser;
            var vm = new CommitteeRosterViewModel();
            string _committeeId = _httpContextAccessor.HttpContext.Request.Query["cid"];
            if (!string.IsNullOrEmpty(_committeeId))
            {
                vm = await _committeeService.GetCommitteeRosterMembersAsync(_committeeId);
            }
            return View("~/Components/Widgets/CommitteeRoster/_CommitteeRoster.cshtml", vm);
        }
    }
}
