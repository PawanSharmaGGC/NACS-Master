using CMS.Membership;
using Convenience.org.Components.Widgets.MemberSearchMyDirectory;
using Convenience.org.Components.Widgets.MemberSearchMyDirectorySidebar;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Xrm.Sdk;
using MyNACSSavedItems;
using System;
using System.Linq;
using System.Threading.Tasks;
using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;

namespace Convenience.org.Components.Widgets.MemberSearchPersonDetails
{
    [Route("MemberSearchPersonDetails")]
    public class MemberSearchPersonDetailsController : Controller
    {
        private readonly Dynamics365DataService _dataService;
        private readonly RecentViewHelper _recentViewHelper;

        public MemberSearchPersonDetailsController(Dynamics365DataService dataService, IHttpContextAccessor httpContextAccessor)
        {
            _dataService = dataService;
            _recentViewHelper = new RecentViewHelper(httpContextAccessor);
        }

        [HttpGet("GetPersonDetails")]
        public async Task<IActionResult> GetPersonDetails(Guid contactId)
        {
            var personDetails = await _dataService.GetPersonDetailsByIdAsync(contactId);
            var parentCustomerId = personDetails.GetAttributeValue<EntityReference>("parentcustomerid");
            var userId = MembershipContext.AuthenticatedUser.UserID;
            var isSaved = IsPersonSaved(contactId, userId);

            // Add to recent views
            _recentViewHelper.AddRecentView(new RecentViewModel
            {
                Title = personDetails.GetAttributeValue<string>("pa_labelname") ?? "Unknown Person",
                URL = Url.Action("GetPersonDetails", "MemberSearchPersonDetails", new { contactId }),
                Type = "Person",
                ItemGUID = contactId
            });

            var model = new MemberSearchPersonDetailsViewModel
            {
                ContactId = personDetails.GetAttributeValue<Guid>("contactid"),
                AccountId = parentCustomerId?.Id ?? Guid.Empty,
                PaLabelName = personDetails.GetAttributeValue<string>("pa_labelname"),
                JobTitle = personDetails.GetAttributeValue<string>("jobtitle"),
                AccountName = parentCustomerId?.Name,
                AddressComposite = personDetails.GetAttributeValue<string>("address1_composite"),
                City = personDetails.GetAttributeValue<string>("address1_city"),
                StateOrProvince = personDetails.GetAttributeValue<string>("address1_stateorprovince"),
                Telephone = personDetails.GetAttributeValue<string>("address1_telephone1"),
                Email = personDetails.GetAttributeValue<string>("emailaddress1"),
                IsSaved = isSaved
            };

            return View("~/Components/Widgets/MemberSearchPersonDetails/_MemberSearchPersonDetails.cshtml", model);
        }
        [HttpPost("AddRemovePerson")]
        public IActionResult AddRemovePerson([FromBody, Bind("Command, PaLabelName, AccountName, AccountId, ContactId")] MemberSearchPersonDetailsViewModel model)
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
                    if (!IsPersonSaved(model.ContactId, userId))
                    {
                        AddSavedItem(model.ContactId.ToString(), model.PaLabelName, model.AccountName, model.AccountId.ToString(), userId);

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
                    if (IsPersonSaved(model.ContactId, userId))
                    {
                        RemoveSavedItem(model.ContactId.ToString(), userId);
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
        public void AddSavedItem(string contactId, string paLabelName, string accountName, string accountId, int userId)
        {
            // Create a new instance of the MemberItem object
            var memberItem = MemberItemInfo.New();

            memberItem.SetValue("NACSIndividualKey", contactId);
            memberItem.SetValue("NACSOrganizationKey", accountId);
            memberItem.SetValue("KenticoUserID", userId);
            memberItem.SetValue("SavedType", "Person");
            memberItem.SetValue("SavedDate", DateTime.UtcNow);
            memberItem.SetValue("SavedItemDisplayName", paLabelName);
            memberItem.SetValue("SavedItemDisplayDescription", accountName);

            // Save the item to the database
            memberItem.Insert();
        }
        public void RemoveSavedItem(string contactId, int userId)
        {
            // Retrieve the saved item
            var savedItem = MemberItemInfo.Provider
                .Get()
                .WhereEquals("NACSIndividualKey", contactId)
                .WhereEquals("KenticoUserID", 65)
                .WhereEquals("SavedType", "Person")
                .FirstOrDefault();

            if (savedItem != null)
            {
                // Delete the saved item
                savedItem.Delete();
            }
        }
        public bool IsPersonSaved(Guid contactId, int userId)
        {
            var savedItem = MemberItemInfo.Provider
                .Get()
                .WhereEquals("NACSIndividualKey", contactId)
                .WhereEquals("KenticoUserID", 65)
                .WhereEquals("SavedType", "Person")
                .FirstOrDefault();

            return savedItem != null;
        }
    }
}
