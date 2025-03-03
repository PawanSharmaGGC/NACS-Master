
using CMS.Membership;
using Convenience.org.Components.Widgets.MemberSearchMyDirectory;
using Convenience.org.Components.Widgets.MemberSearchMyDirectorySidebar;
using Kentico.PageBuilder.Web.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyNACSSavedItems;
using System;
using System.Linq;

[assembly: RegisterWidget("MemberSearchSidebar", typeof(MemberSearchMyDirectorySidebarViewComponent), "Sidebar Directory", Description = "Displays saved persons and companies in a sidebar", IconClass = "icon-list")]

namespace Convenience.org.Components.Widgets.MemberSearchMyDirectorySidebar
{
    public class MemberSearchMyDirectorySidebarViewComponent : ViewComponent
    {
        private readonly RecentViewHelper _recentViewHelper;

        public MemberSearchMyDirectorySidebarViewComponent(IHttpContextAccessor httpContextAccessor)
        {
            _recentViewHelper = new RecentViewHelper(httpContextAccessor);
        }
        public IViewComponentResult Invoke()
        {
            // Replace with authenticated user ID
            //var userId = MembershipContext.AuthenticatedUser?.UserID ?? 0;
            var userId = 65;

            if (userId == 0)
            {
                return Content("User is not authenticated.");
            }

            // Fetch saved items
            var savedItems = MemberItemInfo.Provider
                .Get()
                .WhereEquals("KenticoUserID", userId)
                .ToList();

            // Group by type
            var persons = savedItems
                .Where(x => x.GetStringValue("SavedType", "") == "Person")
                .Select(x => new SavedItemViewModel
                {
                    ItemId = x.GetGuidValue("NACSIndividualKey", Guid.Empty),
                    SavedItemDisplayName = x.GetStringValue("SavedItemDisplayName", ""),
                    SavedItemDisplayDescription = x.GetStringValue("SavedItemDisplayDescription", "")
                }).ToList();

            var companies = savedItems
                .Where(x => x.GetStringValue("SavedType", "") == "Company")
                .Select(x => new SavedItemViewModel
                {
                    ItemId = x.GetGuidValue("NACSOrganizationKey", Guid.Empty),
                    SavedItemDisplayName = x.GetStringValue("SavedItemDisplayName", ""),
                    SavedItemDisplayDescription = x.GetStringValue("SavedItemDisplayDescription", "")
                }).ToList();

            var recentViews = _recentViewHelper.GetRecentViews();

            var model = new MemberSearchMyDirectoryViewModel
            {
                Persons = persons,
                Companies = companies,
                RecentViews = recentViews
            };

            return View("~/Components/Widgets/MemberSearchMyDirectorySidebar/_MemberSearchMyDirectorySidebar.cshtml", model);
        }
    }
}
