using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using System.Linq;
using Microsoft.Extensions.Options;
using Convenience.org.Components.Widgets.AlumniDirectory;
using Microsoft.Xrm.Sdk;

public class AlumniDirectoryController : Controller
{
    private readonly Dynamics365DataService _dataService;
    private readonly MarketingListIdsConfig _marketListIds;

    public AlumniDirectoryController(Dynamics365DataService dataService, IOptions<MarketingListIdsConfig> marketingListIds)
    {
        _dataService = dataService;
        _marketListIds = marketingListIds.Value;
    }

    public async Task<IActionResult> GetMembers(int page = 1, int pageSize = 50, string searchTerm = "")
    {
        var allMembers = await _dataService.GetMembersFromMarketingListAsync(_marketListIds.AlumniNetworkDirectoryOptIn, MapToAlumniMember, includeAdditionalAttributes: false);
        
        // Sanitize the input (covered in security below)
        searchTerm = System.Web.HttpUtility.HtmlEncode(searchTerm);

        // Filter and highlight search terms
        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            allMembers = allMembers
                .Where(m =>
                    m.FirstName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                    m.LastName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                .Select(m =>
                {
                    if (!string.IsNullOrEmpty(m.FirstName) && m.FirstName.IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) >= 0)
                        m.FirstName = m.FirstName.Replace(searchTerm, $"<mark>{searchTerm}</mark>", StringComparison.OrdinalIgnoreCase);
                    if (!string.IsNullOrEmpty(m.LastName) && m.LastName.IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) >= 0)
                        m.LastName = m.LastName.Replace(searchTerm, $"<mark>{searchTerm}</mark>", StringComparison.OrdinalIgnoreCase);
                    return m;
                })
                .ToList();
        }

        var paginatedMembers = allMembers
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        return Json(new { members = paginatedMembers });
    }
    private AlumniMember MapToAlumniMember(Entity entity)
    {
        return new AlumniMember
        {
            ContactId = entity.GetAttributeValue<EntityReference>("entityid")?.Id ?? Guid.Empty,
            FirstName = entity.GetAttributeValue<AliasedValue>("contact.firstname")?.Value as string,
            LastName = entity.GetAttributeValue<AliasedValue>("contact.lastname")?.Value as string,
            Email = entity.GetAttributeValue<AliasedValue>("contact.emailaddress1")?.Value as string,
            Title = entity.GetAttributeValue<AliasedValue>("contact.jobtitle")?.Value as string,
            City = entity.GetAttributeValue<AliasedValue>("contact.address1_city")?.Value as string,
            StateOrProvince = entity.GetAttributeValue<AliasedValue>("contact.address1_stateorprovince")?.Value as string,
            Location = entity.GetAttributeValue<AliasedValue>("contact.address1_country")?.Value as string,
            LinkedInURL = entity.GetAttributeValue<AliasedValue>("contact.pa_linkedin")?.Value as string,
            ProfileImage = entity.GetAttributeValue<AliasedValue>("contact.pa_profileimage")?.Value as string,
            Company = entity.GetAttributeValue<AliasedValue>("account.name")?.Value as string
        };
    }

}
