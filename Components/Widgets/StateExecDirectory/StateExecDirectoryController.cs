using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.Xrm.Sdk;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Convenience.org.Components.Widgets.StateExecDirectory
{
    public class StateExecDirectoryController : Controller
    {
        private readonly Dynamics365DataService _dataService;
        private readonly MarketingListIdsConfig _marketListIds;

        public StateExecDirectoryController(Dynamics365DataService dataService, IOptions<MarketingListIdsConfig> marketingListIds)
        {
            _dataService = dataService;
            _marketListIds = marketingListIds.Value;
        }

        //[HttpGet]
        //public async Task<IActionResult> GetExecutives(int page = 1, int pageSize = 50, string searchTerm = "")
        //{
        //    var allExecutives = await _dataService.GetMembersFromMarketingListAsync(_marketListIds.StateExecDirectoryOptIn);

        //    searchTerm = System.Web.HttpUtility.HtmlEncode(searchTerm);

        //    //if (!string.IsNullOrWhiteSpace(searchTerm))
        //    //{
        //    //    allExecutives = allExecutives
        //    //        .Where(exec => exec.FirstName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
        //    //                       exec.LastName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
        //    //        .Select(exec =>
        //    //        {
        //    //            if (!string.IsNullOrEmpty(exec.FirstName) && exec.FirstName.IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) >= 0)
        //    //                exec.FirstName = exec.FirstName.Replace(searchTerm, $"<mark>{searchTerm}</mark>", StringComparison.OrdinalIgnoreCase);
        //    //            if (!string.IsNullOrEmpty(exec.LastName) && exec.LastName.IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) >= 0)
        //    //                exec.LastName = exec.LastName.Replace(searchTerm, $"<mark>{searchTerm}</mark>", StringComparison.OrdinalIgnoreCase);
        //    //            return exec;
        //    //        })
        //    //        .ToList();
        //    //}
        //    if (!string.IsNullOrWhiteSpace(searchTerm))
        //    {
        //        allExecutives = allExecutives
        //            .Where(exec => exec.FirstName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
        //                           exec.LastName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
        //            .Select(exec =>
        //            {
        //                if (!string.IsNullOrEmpty(exec.FirstName))
        //                {
        //                    var startIndex = exec.FirstName.IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase);
        //                    if (startIndex >= 0)
        //                    {
        //                        var originalMatch = exec.FirstName.Substring(startIndex, searchTerm.Length); 
        //                        exec.FirstName = exec.FirstName.Replace(originalMatch, $"<mark>{originalMatch}</mark>");
        //                    }
        //                }

        //                if (!string.IsNullOrEmpty(exec.LastName))
        //                {
        //                    var startIndex = exec.LastName.IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase);
        //                    if (startIndex >= 0)
        //                    {
        //                        var originalMatch = exec.LastName.Substring(startIndex, searchTerm.Length); 
        //                        exec.LastName = exec.LastName.Replace(originalMatch, $"<mark>{originalMatch}</mark>");
        //                    }
        //                }
        //                return exec;
        //            })
        //            .ToList();
        //    }

        //    var paginatedExecutives = allExecutives
        //        .Skip((page - 1) * pageSize)
        //        .Take(pageSize)
        //        .ToList();

        //    var hasMorePages = allExecutives.Count > page * pageSize;

        //    return Json(new { executives = paginatedExecutives, hasMorePages });
        //}

        public async Task<IActionResult> GetExecutives(int page = 1, int pageSize = 50, string searchTerm = "", string selectedCountries = "", string selectedStates = "")
        {
            // Split the comma-separated string into arrays if they are not empty
            var countryArray = string.IsNullOrEmpty(selectedCountries) ? new string[0] : selectedCountries.Split(',');
            var stateArray = string.IsNullOrEmpty(selectedStates) ? new string[0] : selectedStates.Split(',');

            var allExecutives = await _dataService.GetMembersFromMarketingListAsync(
                _marketListIds.StateExecDirectoryOptIn,
                MapToStateExecMember,
                includeAdditionalAttributes: true);

            // Sanitize and apply search term filter
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                searchTerm = System.Web.HttpUtility.HtmlEncode(searchTerm);
                allExecutives = allExecutives
                    .Where(exec =>
                        (!string.IsNullOrEmpty(exec.FirstName) && exec.FirstName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)) ||
                        (!string.IsNullOrEmpty(exec.LastName) && exec.LastName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)))
                    .ToList();
            }

            // Apply country filters
            if (countryArray.Any())
            {
                allExecutives = allExecutives
                    .Where(exec => exec.CountriesServed != null &&
                                   exec.CountriesServed.Intersect(countryArray, StringComparer.OrdinalIgnoreCase).Any())
                    .ToList();
            }
            // Apply state filters
            if (stateArray.Any())
            {
                allExecutives = allExecutives
                    .Where(exec => exec.StatesServed != null &&
                                   exec.StatesServed.Intersect(stateArray, StringComparer.OrdinalIgnoreCase).Any())
                    .ToList();
            }

            // Pagination
            var totalExecutives = allExecutives.Count;
            var paginatedExecutives = allExecutives
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var hasMorePages = totalExecutives > page * pageSize;

            // Return paginated results with metadata
            return Json(new
            {
                executives = paginatedExecutives,
                hasMorePages,
                totalExecutives
            });
        }


        private Executive MapToStateExecMember(Entity entity)
        {
            return new Executive
            {
                //ContactId = entity.GetAttributeValue<EntityReference>("entityid")?.Id ?? Guid.Empty,
                FirstName = entity.GetAttributeValue<AliasedValue>("contact.firstname")?.Value as string,
                LastName = entity.GetAttributeValue<AliasedValue>("contact.lastname")?.Value as string,
                Email = entity.GetAttributeValue<AliasedValue>("contact.emailaddress1")?.Value as string,
                Title = entity.GetAttributeValue<AliasedValue>("contact.jobtitle")?.Value as string,
                City = entity.GetAttributeValue<AliasedValue>("contact.address1_city")?.Value as string,
                State = entity.GetAttributeValue<AliasedValue>("contact.address1_stateorprovince")?.Value as string,
                Location = entity.GetAttributeValue<AliasedValue>("contact.address1_country")?.Value as string,
                Company = entity.GetAttributeValue<AliasedValue>("account.name")?.Value as string,
            };
        }


    }
}
