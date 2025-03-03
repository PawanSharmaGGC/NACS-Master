using Convenience.org.Components.Widgets.StateExecDirectory;
using Kentico.PageBuilder.Web.Mvc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

[assembly: RegisterWidget("StateExecDirectoryWidget", typeof(StateExecDirectoryViewComponent), "State Executive Directory", Description = "Displays a list of state exec from the directory.", IconClass = "icon-list")]
namespace Convenience.org.Components.Widgets.StateExecDirectory
{
    public class StateExecDirectoryViewComponent : ViewComponent
    {
        private readonly Dynamics365DataService _dataService;
        private readonly MarketingListIdsConfig _marketListIds;

        public StateExecDirectoryViewComponent(Dynamics365DataService dataService, IOptions<MarketingListIdsConfig> marketingListIds)
        {
            _dataService = dataService;
            _marketListIds = marketingListIds.Value;
        }

        //public async Task<IViewComponentResult> InvokeAsync()
        //{
        //    string searchTerm = "";
        //    int page = 1;
        //    int pageSize = 50;

        //    var allExecutives = await _dataService.GetMembersFromMarketingListAsync(_marketListIds.StateExecDirectoryOptIn);
            
        //    searchTerm = System.Web.HttpUtility.HtmlEncode(searchTerm);
        //    if (!string.IsNullOrWhiteSpace(searchTerm))
        //    {
        //        allExecutives = allExecutives.Where(exec =>
        //            exec.FirstName.Contains(searchTerm, System.StringComparison.OrdinalIgnoreCase) ||
        //            exec.LastName.Contains(searchTerm, System.StringComparison.OrdinalIgnoreCase)).ToList();
        //    }

        //    var paginatedExecutives = allExecutives
        //        .Skip((page - 1) * pageSize)
        //        .Take(pageSize)
        //        .ToList();

        //    var model = new StateExecDirectoryViewModel
        //    {
        //        Executives = paginatedExecutives.Select(exec => new Executive
        //        {
        //            FirstName = exec.FirstName,
        //            LastName = exec.LastName,
        //            Title = exec.Title,
        //            Company = exec.Company,
        //            State = exec.StateOrProvince,
        //            Location = exec.Location,
        //            City = exec.City,
        //            Email = exec.Email
        //        }).ToList(),
        //        SearchTerm = searchTerm,
        //        CurrentPage = page,
        //        TotalCount = allExecutives.Count(),
        //        HasMorePages = allExecutives.Count > page * pageSize
        //    };

        //    return View("~/Components/Widgets/StateExecDirectory/_StateExecDirectory.cshtml", model);
        //}


        public async Task<IViewComponentResult> InvokeAsync()
        {
            string searchTerm = "";
            int page = 1;
            int pageSize = 50;
            string[] selectedCountries = null;
            string[] selectedStates = null;

            var allExecutives = await _dataService.GetMembersFromMarketingListAsync(_marketListIds.StateExecDirectoryOptIn, MapToStateExecMember, includeAdditionalAttributes: true);

            // Extract unique countries and states for filters
            var availableCountries = allExecutives.SelectMany(exec => exec.CountriesServed ?? new List<string>()).Distinct().ToList();
            var availableStates = allExecutives.SelectMany(exec => exec.StatesServed ?? new List<string>()).Distinct().ToList();
            // Filter by search term
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                searchTerm = System.Web.HttpUtility.HtmlEncode(searchTerm);
                allExecutives = allExecutives
                    .Where(exec =>
                        exec.FirstName.Contains(searchTerm, System.StringComparison.OrdinalIgnoreCase) ||
                        exec.LastName.Contains(searchTerm, System.StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            // Filter by selected countries
            if (selectedCountries != null && selectedCountries.Any())
            {
                allExecutives = allExecutives
                    .Where(exec => exec.CountriesServed != null &&
                                   exec.CountriesServed.Intersect(selectedCountries, System.StringComparer.OrdinalIgnoreCase).Any())
                    .ToList();
            }

            // Filter by selected states
            if (selectedStates != null && selectedStates.Any())
            {
                allExecutives = allExecutives
                    .Where(exec => exec.StatesServed != null &&
                                   exec.StatesServed.Intersect(selectedStates, System.StringComparer.OrdinalIgnoreCase).Any())
                    .ToList();
            }

            // Pagination
            var paginatedExecutives = allExecutives
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            // Build the ViewModel
            var model = new StateExecDirectoryViewModel
            {
                Executives = paginatedExecutives.Select(exec => new Executive
                {
                    FirstName = exec.FirstName,
                    LastName = exec.LastName,
                    Title = exec.Title,
                    Company = exec.Company,
                    State = exec.State,
                    Location = exec.Location,
                    City = exec.City,
                    Email = exec.Email,
                    CountriesServed = exec.CountriesServed,
                    StatesServed = exec.StatesServed
                }).ToList(),
                SearchTerm = searchTerm,
                CurrentPage = page,
                TotalCount = allExecutives.Count,
                HasMorePages = allExecutives.Count > page * pageSize,
                AvailableCountries = availableCountries,
                AvailableStates = availableStates,
                SelectedCountries = selectedCountries,
                SelectedStates = selectedStates
            };

            return View("~/Components/Widgets/StateExecDirectory/_StateExecDirectory.cshtml", model);
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
                Company = entity.GetAttributeValue<AliasedValue>("account.name")?.Value as string
            };
        }

    }
}
