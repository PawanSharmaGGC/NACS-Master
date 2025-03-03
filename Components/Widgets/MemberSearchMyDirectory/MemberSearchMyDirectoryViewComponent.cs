using CMS.Membership;
using Convenience.org.Components.Widgets.MemberSearchMyDirectory;
using Kentico.PageBuilder.Web.Mvc;
using Microsoft.AspNetCore.Mvc;
using MyNACSSavedItems;
using System;
using System.Linq;

[assembly: RegisterWidget("MemberSearchMyDirectory", typeof(MemberSearchMyDirectoryViewComponent), "My Directory", Description = "Displays saved persons and companies", IconClass = "icon-list")]

namespace Convenience.org.Components.Widgets.MemberSearchMyDirectory
{
    public class MemberSearchMyDirectoryViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            //var userId = MembershipContext.AuthenticatedUser.UserID;
            var userId = 65;

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
                    SavedItemDisplayDescription = x.GetStringValue("SavedItemDisplayDescription", ""),
                    SavedType = "Person"
                }).ToList();

            var companies = savedItems
                .Where(x => x.GetStringValue("SavedType", "") == "Company")
                .Select(x => new SavedItemViewModel
                {
                    ItemId = x.GetGuidValue("NACSOrganizationKey", Guid.Empty),
                    SavedItemDisplayName = x.GetStringValue("SavedItemDisplayName", ""),
                    SavedItemDisplayDescription = x.GetStringValue("SavedItemDisplayDescription", ""),
                    SavedType = "Company"
                }).ToList();

            var model = new MemberSearchMyDirectoryViewModel
            {
                Persons = persons,
                Companies = companies
            };

            return View("~/Components/Widgets/MemberSearchMyDirectory/_MemberSearchMyDirectory.cshtml", model);
        }
    }
}
