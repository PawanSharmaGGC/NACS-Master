using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using CMS.Membership;
using Convenience.org.Components.Widgets.MemberSearchPersonDetails;
using MyNACSSavedItems;
using System.Linq;
using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;
using Microsoft.Xrm.Sdk;
using Convenience.org.Components.Widgets.MemberSearchMyDirectory;
using Convenience.org.Components.Widgets.MemberSearchMyDirectorySidebar;
using Microsoft.AspNetCore.Http;

namespace Convenience.org.Components.Widgets.MemberSearchCompanyDetails
{
    [Route("MemberSearchCompanyDetails")]
    public class MemberSearchCompanyDetailsController : Controller
    {
        private readonly Dynamics365DataService _dataService;
        private readonly RecentViewHelper _recentViewHelper;

        public MemberSearchCompanyDetailsController(Dynamics365DataService dataService, IHttpContextAccessor httpContextAccessor)
        {
            _dataService = dataService;
            _recentViewHelper = new RecentViewHelper(httpContextAccessor);
        }

        [HttpGet("GetCompanyDetails")]
        public async Task<IActionResult> GetCompanyDetails(Guid accountId)
        {
            // Fetch company details including option set labels
            var companyDetails = await _dataService.GetCompanyDetailsAsync(accountId);

            var userId = MembershipContext.AuthenticatedUser.UserID;
            var isSaved = IsCompanySaved(accountId, userId);

            // Add to recent views
            _recentViewHelper.AddRecentView(new RecentViewModel
            {
                Title = companyDetails.GetAttributeValue<string>("name") ?? "Unknown Company",
                URL = Url.Action("GetCompanyDetails", "MemberSearchCompanyDetails", new { accountId }),
                Type = "Company",
                ItemGUID = accountId
            });

            // Prepare the view model
            var model = new MemberSearchCompanyDetailsViewModel
            {
                AccountId = accountId,
                Name = companyDetails.GetAttributeValue<string>("name"),
                AccountTypeName = companyDetails.GetAttributeValue<string>("nacs_accounttype"),
                SupplierTypeName = companyDetails.GetAttributeValue<string>("nacs_suppliertype"),
                Address = companyDetails.GetAttributeValue<string>("address1_composite"),
                WebsiteUrl = companyDetails.GetAttributeValue<string>("websiteurl"),
                Telephone = companyDetails.GetAttributeValue<string>("telephone1"),
                TotalStores = companyDetails.GetAttributeValue<int>("nacs_totalstores"),
                IsSaved = isSaved
            };

            return View("~/Components/Widgets/MemberSearchCompanyDetails/_MemberSearchCompanyDetails.cshtml", model);
        }


        [HttpPost("AddRemoveCompany")]
        public IActionResult AddRemoveCompany([FromBody, Bind("Command, Name, AccountTypeName, AccountId")] MemberSearchCompanyDetailsViewModel model)
        {
            try
            {
                //var currentUserId = _currentUserService.GetCurrentUserId();
                //if (string.IsNullOrWhiteSpace(currentUserId) || string.IsNullOrWhiteSpace(request.ContactId))
                //{
                //    return BadRequest(new { success = false });
                //}
                var userId = MembershipContext.AuthenticatedUser.UserID;
                if (model.Command == "add")
                {
                    if (!IsCompanySaved(model.AccountId, userId))
                    {
                        AddSavedItem(model.AccountId.ToString(), model.AccountTypeName, model.Name, userId);

                        return Ok(new
                        {
                            success = true,
                            command = "remove",
                            iconClass = "fa fa-bookmark",
                            text = "Remove"
                        });
                    }
                }
                else if (model.Command == "remove")
                {
                    if (IsCompanySaved(model.AccountId, userId))
                    {
                        RemoveSavedItem(model.AccountId.ToString(), userId);
                        return Ok(new
                        {
                            success = true,
                            command = "add",
                            iconClass = "far fa-bookmark",
                            text = "Save"
                        });
                    }
                }

                return BadRequest(new { success = false });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, error = ex.Message });
            }
        }
        public void AddSavedItem(string accountId, string accountType, string accountName, int userId)
        {
            // Create a new instance of the MemberItem object
            var memberItem = MemberItemInfo.New();

            memberItem.SetValue("NACSOrganizationKey", accountId);
            memberItem.SetValue("KenticoUserID", userId);
            memberItem.SetValue("SavedType", "Company");
            memberItem.SetValue("SavedDate", DateTime.UtcNow);
            memberItem.SetValue("SavedItemDisplayName", accountName);
            memberItem.SetValue("SavedItemDisplayDescription", accountType);

            // Save the item to the database
            memberItem.Insert();
        }
        public void RemoveSavedItem(string accountId, int userId)
        {
            // Retrieve the saved item
            var savedItem = MemberItemInfo.Provider
                .Get()
                .WhereEquals("NACSOrganizationKey", accountId)
                .WhereEquals("KenticoUserID", 65)
                .WhereEquals("SavedType", "Company")
                .FirstOrDefault();

            if (savedItem != null)
            {
                // Delete the saved item
                savedItem.Delete();
            }
        }
        public bool IsCompanySaved(Guid accountId, int userId)
        {
            var savedItem = MemberItemInfo.Provider
                .Get()
                .WhereEquals("NACSOrganizationKey", accountId)
                .WhereEquals("KenticoUserID", 65)
                .WhereEquals("SavedType", "Company")
                .FirstOrDefault();

            return savedItem != null;
        }
    }
}
