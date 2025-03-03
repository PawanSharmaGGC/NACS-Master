using Convenience.org.Components.Widgets.AlumniDirectory;
using Kentico.PageBuilder.Web.Mvc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.Xrm.Sdk;
using System;
using System.Threading.Tasks;

[assembly: RegisterWidget("AlumniDirectoryWidget", typeof(AlumniDirectoryViewComponent), "Alumni Directory", Description = "Displays a list of alumni from the directory.", IconClass = "icon-list")]

namespace Convenience.org.Components.Widgets.AlumniDirectory
{
    public class AlumniDirectoryViewComponent : ViewComponent
    {
        private readonly Dynamics365DataService _dataService;
        private readonly MarketingListIdsConfig _marketListIds;

        public AlumniDirectoryViewComponent(Dynamics365DataService dataService, IOptions<MarketingListIdsConfig> marketListIds)
        {
            _dataService = dataService;
            _marketListIds = marketListIds.Value;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            

            var members = await _dataService.GetMembersFromMarketingListAsync(_marketListIds.AlumniNetworkDirectoryOptIn, MapToAlumniMember, includeAdditionalAttributes: false);

            //// Debugging log to check the size of members list
            //Console.WriteLine($"Retrieved {members.Count} alumni members.");

            //if (members.Count == 0)
            //{
            //    Console.WriteLine("No alumni members found in the directory.");
            //}

            //var viewModel = new AlumniMember
            //{
            //    ContactId = members.FirstOrDefault()?.ContactId ?? Guid.Empty,
            //    FirstName = members.FirstOrDefault()?.FirstName,
            //    LastName = members.FirstOrDefault()?.LastName,
            //    Title = members.FirstOrDefault()?.Title,
            //    Email = members.FirstOrDefault()?.Email,
            //    LinkedInURL = members.FirstOrDefault()?.LinkedInURL,
            //    Company = members.FirstOrDefault()?.Company,
            //    City = members.FirstOrDefault()?.City,
            //    StateOrProvince = members.FirstOrDefault()?.StateOrProvince,
            //    Location = members.FirstOrDefault()?.Location,
            //    ProfileImage = members.FirstOrDefault()?.ProfileImage,
            //    ProgramsAttended = members.FirstOrDefault()?.ProgramsAttended,
            //    MasterOfConvenience = members.FirstOrDefault()?.MasterOfConvenience ?? false
            //};

            return View("~/Components/Widgets/AlumniDirectory/_AlumniDirectory.cshtml", members);
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
}
