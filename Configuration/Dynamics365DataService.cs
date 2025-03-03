using CMS.Core;
using CMS.Helpers;
using Convenience.org.Components.Widgets.AlumniDirectory;
using Convenience.org.Components.Widgets.AlumniProfile;
using Convenience.org.Components.Widgets.MemberSearchAllCompanies;
using Convenience.org.Components.Widgets.StateExecDirectory;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.PowerPlatform.Dataverse.Client;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class Dynamics365DataService
{
    private readonly Dynamics365Connectivity _connectivity;
    private readonly OptionSetHelper _optionSetHelper;
    private readonly IEventLogService _eventLogService;

    public Dynamics365DataService(Dynamics365Connectivity connectivity, IEventLogService eventLogService)
    {
        _connectivity = connectivity;
        _optionSetHelper = new OptionSetHelper(_connectivity);
        _eventLogService = eventLogService;
    }
    public string GetOptionSetLabel(string entityLogicalName, string attributeLogicalName, int optionSetValue)
    {
        return _optionSetHelper.GetOptionSetLabel(entityLogicalName, attributeLogicalName, optionSetValue);
    }
    public async Task<Guid?> GetCurrentUserContactIdAsync(string email)
    {
        var client = _connectivity.GetServiceClient();

        // Define the query to find the contact based on the email address
        var query = new QueryExpression("contact")
        {
            ColumnSet = new ColumnSet("contactid") 
        };
        query.Criteria.AddCondition("emailaddress1", ConditionOperator.Equal, email);

        var result = await client.RetrieveMultipleAsync(query);

        if (result.Entities.Count > 0)
        {
            return result.Entities[0].Id;
        }

        return null; 
    }
    public async Task AddUserToListAsync(Guid listId, Guid userId)
    {
        if (!await IsUserInListAsync(listId, userId))
        {
            var addRequest = new AddMemberListRequest
            {
                ListId = listId,
                EntityId = userId
            };

            var client = _connectivity.GetServiceClient();
            await client.ExecuteAsync(addRequest);
        }
    }
    public async Task RemoveUserFromListAsync(Guid listId, Guid userId)
    {
        if (await IsUserInListAsync(listId, userId))
        {
            var removeRequest = new RemoveMemberListRequest
            {
                ListId = listId,
                EntityId = userId
            };

            var client = _connectivity.GetServiceClient();
            await client.ExecuteAsync(removeRequest);
        }
    }
    public async Task<bool> IsUserInListAsync(Guid listId, Guid userId)
    {
        var query = new QueryExpression("listmember")
        {
            ColumnSet = new ColumnSet("listid", "entityid"),
            Criteria = new FilterExpression
            {
                Conditions =
                    {
                        new ConditionExpression("listid", ConditionOperator.Equal, listId),
                        new ConditionExpression("entityid", ConditionOperator.Equal, userId)
                    }
            }
        };

        var client = _connectivity.GetServiceClient();
        var response = await client.RetrieveMultipleAsync(query);
        return response.Entities.Count > 0;
    }
    public async Task<Entity> GetContactDetailsAsync(Guid contactId)
    {
        var client = _connectivity.GetServiceClient();

        try
        {
            return await Task.FromResult(client.Retrieve("contact", contactId, new ColumnSet("address1_country", "address1_line1", "address1_city", "address1_postalcode", "address1_stateorprovince")));
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Unable to retrieve details for contact ID {contactId}.", ex);
        }

        //var query = new QueryExpression("contact")
        //{
        //    ColumnSet = new ColumnSet("address1_country", "address1_line1", "address1_city", "address1_postalcode", "address1_stateorprovince")
        //};
        //query.Criteria.AddCondition("contactid", ConditionOperator.Equal, contactId);

        //// Execute the query asynchronously
        //var result = await client.RetrieveMultipleAsync(query);
        //return result.Entities.FirstOrDefault();
    }
    public async Task<EntityCollection> GetTopicsAsync()
    {
        try
        {
            var client = _connectivity.GetServiceClient();

            string fetchXml = @"
            <fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='true'>
                <entity name='msdynmkt_topic'>
                    <attribute name='msdynmkt_name' />
                    <attribute name='msdynmkt_topicid' />
                    <attribute name='nacs_webgrouping' />
                    <filter type='and'>
                        <condition attribute='msdynmkt_purposeid' operator='eq' value='10000000-0000-0000-0000-000000000003' />
                    </filter>
                    <order attribute='msdynmkt_name' descending='false' />
                </entity>
            </fetch>";

            var topics = await client.RetrieveMultipleAsync(new FetchExpression(fetchXml));

            return topics ?? new EntityCollection();
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while fetching topics.", ex);
        }
    }
    public async Task<EntityCollection> GetPreferencesAsync(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            throw new ArgumentException("Email cannot be null or empty.", nameof(email));
        }

        try
        {
            var client = _connectivity.GetServiceClient();

            var query = new QueryExpression("msdynmkt_contactpointconsent4")
            {
                ColumnSet = new ColumnSet("msdynmkt_topicid", "msdynmkt_value"),
                Criteria = new FilterExpression
                {
                    Conditions =
                {
                    new ConditionExpression("msdynmkt_contactpointvalue", ConditionOperator.Equal, email),
                    new ConditionExpression("statecode", ConditionOperator.Equal, 0) // Active state
                }
                }
            };

            var preferences = await client.RetrieveMultipleAsync(query);

            return preferences ?? new EntityCollection();
        }
        catch (Exception ex)
        {
            throw new Exception($"An error occurred while fetching preferences for email: {email}", ex);
        }
    }
    //public async Task<EntityCollection> GetMemberCompaniesAsync()
    //{
    //    var client = _connectivity.GetServiceClient();

    //    string fetchXml = @"
    //    <fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
    //        <entity name='account'>
    //            <attribute name='name' />
    //            <attribute name='nacs_accounttype' />
    //            <attribute name='nacs_suppliertype' />
    //            <attribute name='address1_city' />
    //            <attribute name='address1_stateorprovince' />
    //            <attribute name='address1_country' />
    //            <attribute name='nacs_region' />
    //            <filter type='and'>
    //                <condition attribute='pa_member' operator='eq' value='1' />
    //            </filter>
    //        </entity>
    //    </fetch>";

    //    return await client.RetrieveMultipleAsync(new FetchExpression(fetchXml));
    //}
    public async Task<List<MemberSearchAllCompaniesViewModel>> GetMemberCompaniesAsync()
    {
        var client = _connectivity.GetServiceClient();
        var companyList = new List<MemberSearchAllCompaniesViewModel>();

        int fetchCount = 50; // Number of records per page
        int pageNumber = 1;
        string pagingCookie = null;

        var optionSetHelper = new OptionSetHelper(_connectivity);

        var accountTypeCache = new Dictionary<int, string>();
        var supplierTypeCache = new Dictionary<int, string>();

        while (true)
        {
            // Build the FetchXML dynamically, encoding the paging-cookie if it exists
            string fetchXml = $@"
        <fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false' count='{fetchCount}' page='{pageNumber}'{(pagingCookie != null ? $" paging-cookie='{System.Security.SecurityElement.Escape(pagingCookie)}'" : "")}>
            <entity name='account'>
                <attribute name='name' />
                <attribute name='nacs_accounttype' />
                <attribute name='nacs_suppliertype' />
                <attribute name='address1_city' />
                <attribute name='address1_stateorprovince' />
                <attribute name='address1_country' />
                <attribute name='nacs_region' />
                <filter type='and'>
                    <condition attribute='pa_member' operator='eq' value='1' />
                </filter>
            </entity>
        </fetch>";

            try
            {
                // Execute the FetchXML query
                var entityCollection = await client.RetrieveMultipleAsync(new FetchExpression(fetchXml));

                foreach (var entity in entityCollection.Entities)
                {
                    var accountTypeOptionSet = entity.GetAttributeValue<OptionSetValue>("nacs_accounttype");
                    var supplierTypeOptionSet = entity.GetAttributeValue<OptionSetValue>("nacs_suppliertype");

                    var accountTypeName = accountTypeOptionSet != null
                        ? accountTypeCache.GetOrAdd(accountTypeOptionSet.Value, value =>
                            optionSetHelper.GetOptionSetLabel("account", "nacs_accounttype", value))
                        : "N/A";

                    var supplierTypeName = supplierTypeOptionSet != null
                        ? supplierTypeCache.GetOrAdd(supplierTypeOptionSet.Value, value =>
                            optionSetHelper.GetOptionSetLabel("account", "nacs_suppliertype", value))
                        : "N/A";

                    companyList.Add(new MemberSearchAllCompaniesViewModel
                    {
                        AccountId = entity.GetAttributeValue<Guid>("accountid"),
                        Name = entity.GetAttributeValue<string>("name"),
                        AccountTypeName = accountTypeName,
                        SupplierTypeName = supplierTypeName,
                        City = entity.GetAttributeValue<string>("address1_city"),
                        StateOrProvince = entity.GetAttributeValue<string>("address1_stateorprovince"),
                        Country = entity.GetAttributeValue<string>("address1_country"),
                        Region = entity.GetAttributeValue<string>("nacs_region"),
                    });
                }

                if (!entityCollection.MoreRecords)
                    break;

                pagingCookie = entityCollection.PagingCookie;
                pageNumber++;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving companies: {ex.Message}", ex);
            }
        }

        return companyList;
    }
    public async Task<Entity> GetCompanyDetailsAsync(Guid accountId)
    {
        var client = _connectivity.GetServiceClient();

        // Retrieve the account entity with the specified fields
        var entity = await client.RetrieveAsync("account", accountId, new ColumnSet(
            "name",
            "nacs_accounttype",
            "nacs_suppliertype",
            "address1_composite",
            "websiteurl",
            "telephone1",
            "nacs_totalstores"));

        // Process and include option set labels
        if (entity.GetAttributeValue<OptionSetValue>("nacs_accounttype") is OptionSetValue accountTypeOptionSet)
        {
            var accountTypeLabel = _optionSetHelper.GetOptionSetLabel("account", "nacs_accounttype", accountTypeOptionSet.Value);
            entity["nacs_accounttype"] = accountTypeLabel; // Replace value with label
        }

        if (entity.GetAttributeValue<OptionSetValue>("nacs_suppliertype") is OptionSetValue supplierTypeOptionSet)
        {
            var supplierTypeLabel = _optionSetHelper.GetOptionSetLabel("account", "nacs_suppliertype", supplierTypeOptionSet.Value);
            entity["nacs_suppliertype"] = supplierTypeLabel; // Replace value with label
        }

        return entity;
    }

    public async Task<EntityCollection> GetContactsByAccountIdAsync(Guid accountId)
    {
        var client = _connectivity.GetServiceClient();

        string fetchXml = $@"
        <fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
            <entity name='contact'>
                <attribute name='contactid' />
                <attribute name='fullname' />
                <attribute name='jobtitle' />
                <attribute name='address1_city' />
                <attribute name='address1_stateorprovince' />
                <filter type='and'>
                    <condition attribute='accountid' operator='eq' value='{accountId}' />
                </filter>
            </entity>
        </fetch>";

        return await client.RetrieveMultipleAsync(new FetchExpression(fetchXml));
    }
    public async Task<Entity> GetPersonDetailsByIdAsync(Guid contactId)
    {
        var client = _connectivity.GetServiceClient();

        string fetchXml = $@"
        <fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
            <entity name='contact'>
                <attribute name='contactid' />
                <attribute name='pa_labelname' />
                <attribute name='jobtitle' />
                <attribute name='parentcustomerid' />
                <attribute name='parentcustomeridname' />
                <attribute name='address1_composite' />
                <attribute name='address1_city' />
                <attribute name='address1_stateorprovince' />
                <attribute name='address1_telephone1' />
                <attribute name='emailaddress1' />
                <filter type='and'>
                    <condition attribute='contactid' operator='eq' value='{contactId}' />
                </filter>
            </entity>
        </fetch>";

        var result = await client.RetrieveMultipleAsync(new FetchExpression(fetchXml));
        return result.Entities.FirstOrDefault();
    }

    public async Task<List<T>> GetMembersFromMarketingListAsync<T>(Guid marketingListId, Func<Entity, T> mapToMember, bool includeAdditionalAttributes = false)
    {
        var client = _connectivity.GetServiceClient();
        var optionSetHelper = new OptionSetHelper(_connectivity);
        var allMembers = new List<T>();

        string pagingCookie = null;
        int pageNumber = 1;

        do
        {
            // Generate FetchXML dynamically with paging information
            var fetchXml = $@"
        <fetch mapping='logical' distinct='false' page='{pageNumber}' count='50' {(pagingCookie != null ? $"paging-cookie='{System.Security.SecurityElement.Escape(pagingCookie)}'" : "")}>
            <entity name='listmember'>
                <attribute name='entityid' />
                <filter>
                    <condition attribute='listid' operator='eq' value='{marketingListId}' />
                </filter>
                <link-entity name='contact' from='contactid' to='entityid' alias='contact'>
                    <attribute name='contactid' />
                    <attribute name='firstname' />
                    <attribute name='lastname' />
                    <attribute name='emailaddress1' />
                    <attribute name='jobtitle' />
                    <attribute name='address1_city' />
                    <attribute name='address1_stateorprovince' />
                    <attribute name='address1_country' />
                    <attribute name='pa_linkedin' />
                    <attribute name='pa_profileimage' />
                    <attribute name='parentcustomerid' />
                    <link-entity name='account' from='accountid' to='parentcustomerid' alias='account'>
                        <attribute name='accountid' />
                        <attribute name='name' />
                    </link-entity>
                </link-entity>
            </entity>
        </fetch>";

            try
            {
                // Execute FetchXML
                var response = await client.RetrieveMultipleAsync(new FetchExpression(fetchXml));
                pagingCookie = response.PagingCookie;

                foreach (var entity in response.Entities)
                {
                    var accountId = entity.GetAttributeValue<AliasedValue>("account.accountid")?.Value as Guid?;
                    var additionalAttributes = includeAdditionalAttributes && accountId.HasValue
                        ? RetrieveMultiSelectOptionSetValues(accountId.Value, new[] { "nacs_countriesserved", "nacs_statesserved" })
                        : new Dictionary<string, List<string>>();

                    // Map entity to the member using the provided mapping function
                    var member = mapToMember(entity);

                    // Attach additional attributes
                    if (includeAdditionalAttributes && member is Executive executiveMember)
                    {
                        executiveMember.CountriesServed = additionalAttributes.GetValueOrDefault("nacs_countriesserved") ?? new List<string>();
                        executiveMember.StatesServed = additionalAttributes.GetValueOrDefault("nacs_statesserved") ?? new List<string>();
                    }

                    allMembers.Add(member);
                }

                pageNumber++;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during FetchXML execution: {ex.Message}");
                throw;
            }
        } while (!string.IsNullOrEmpty(pagingCookie));

        return allMembers;
    }

    private Dictionary<string, List<string>> RetrieveMultiSelectOptionSetValues(Guid accountId, string[] attributeNames)
    {
        var result = new Dictionary<string, List<string>>();
        var optionSetHelper = new OptionSetHelper(_connectivity);

        var fetchXml = $@"
        <fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
          <entity name='account'>
            {string.Join("", attributeNames.Select(attr => $"<attribute name='{attr}' />"))}
            <filter>
              <condition attribute='accountid' operator='eq' value='{accountId}' />
            </filter>
          </entity>
        </fetch>";

        var client = _connectivity.GetServiceClient();
        var response = client.RetrieveMultiple(new FetchExpression(fetchXml));

        if (response.Entities.Any())
        {
            var entity = response.Entities.First();
            foreach (var attributeName in attributeNames)
            {
                if (entity.Attributes.TryGetValue(attributeName, out var value))
                {
                    var optionSet = value as OptionSetValueCollection;
                    result[attributeName] = optionSetHelper.GetMultiSelectOptionSetLabels("account", attributeName, optionSet);
                }
                else
                {
                    result[attributeName] = new List<string>();
                }
            }
        }
        return result;
    }


    //public async Task<List<AlumniMember>> GetMembersFromMarketingListAsync(Guid marketingListId)
    //{
    //    var client = _connectivity.GetServiceClient();
    //    var allContacts = new List<AlumniMember>();

    //    // Define FetchXML query
    //    var fetchXml = $@"
    //    <fetch mapping='logical' distinct='false' page='1' count='50'>
    //        <entity name='listmember'>
    //            <attribute name='entityid' />
    //            <filter>
    //                <condition attribute='listid' operator='eq' value='{marketingListId}' />
    //            </filter>
    //            <link-entity name='contact' from='contactid' to='entityid' alias='contact'>
    //                <attribute name='contactid' />
    //                <attribute name='firstname' />
    //                <attribute name='lastname' />
    //                <attribute name='emailaddress1' />
    //                <attribute name='jobtitle' />
    //                <attribute name='address1_city' />
    //                <attribute name='address1_stateorprovince' />
    //                <attribute name='address1_country' />
    //                <attribute name='pa_linkedin' />
    //                <attribute name='pa_profileimage' />
    //                <attribute name='parentcustomerid' />
    //                <link-entity name='account' from='accountid' to='parentcustomerid' alias='account'>
    //                    <attribute name='name' />
    //                </link-entity>
    //            </link-entity>
    //        </entity>
    //    </fetch>";

    //    string pagingCookie = null;
    //    int pageNumber = 1;

    //    do
    //    {
    //        var response = await client.RetrieveMultipleAsync(new FetchExpression(fetchXml.Replace("page='1'", $"page='{pageNumber}'").Replace("pagingcookie='null'", $"pagingcookie='{pagingCookie}'")));
    //        pagingCookie = response.PagingCookie;

    //        foreach (var entity in response.Entities)
    //        {
    //            var contactDto = new AlumniMember
    //            {
    //                ContactId = entity.GetAttributeValue<EntityReference>("entityid")?.Id ?? Guid.Empty,
    //                FirstName = entity.GetAttributeValue<AliasedValue>("contact.firstname")?.Value as string,
    //                LastName = entity.GetAttributeValue<AliasedValue>("contact.lastname")?.Value as string,
    //                Email = entity.GetAttributeValue<AliasedValue>("contact.emailaddress1")?.Value as string,
    //                Title = entity.GetAttributeValue<AliasedValue>("contact.jobtitle")?.Value as string,
    //                City = entity.GetAttributeValue<AliasedValue>("contact.address1_city")?.Value as string,
    //                StateOrProvince = entity.GetAttributeValue<AliasedValue>("contact.address1_stateorprovince")?.Value as string,
    //                Location = entity.GetAttributeValue<AliasedValue>("contact.address1_country")?.Value as string,
    //                LinkedInURL = entity.GetAttributeValue<AliasedValue>("contact.pa_linkedin")?.Value as string,
    //                ProfileImage = entity.GetAttributeValue<AliasedValue>("contact.pa_profileimage")?.Value as string,
    //                Company = entity.GetAttributeValue<AliasedValue>("account.name")?.Value as string
    //            };

    //            allContacts.Add(contactDto);
    //        }

    //        pageNumber++;

    //    } while (!string.IsNullOrEmpty(pagingCookie));

    //    return allContacts;
    //}

    //public async Task<List<T>> GetMembersFromMarketingListAsync<T>(Guid marketingListId, Func<Entity, T> mapToMember, bool includeAdditionalAttributes = false)
    //{
    //    var client = _connectivity.GetServiceClient();
    //    var allMembers = new List<T>();

    //    // FetchXML query for members
    //    var fetchXml = $@"
    //    <fetch mapping='logical' distinct='false' page='1' count='50'>
    //        <entity name='listmember'>
    //            <attribute name='entityid' />
    //            <filter>
    //                <condition attribute='listid' operator='eq' value='{marketingListId}' />
    //            </filter>
    //            <link-entity name='contact' from='contactid' to='entityid' alias='contact'>
    //                <attribute name='contactid' />
    //                <attribute name='firstname' />
    //                <attribute name='lastname' />
    //                <attribute name='emailaddress1' />
    //                <attribute name='jobtitle' />
    //                <attribute name='address1_city' />
    //                <attribute name='address1_stateorprovince' />
    //                <attribute name='address1_country' />
    //                <attribute name='pa_linkedin' />
    //                <attribute name='pa_profileimage' />
    //                <attribute name='parentcustomerid' />
    //                <link-entity name='account' from='accountid' to='parentcustomerid' alias='account'>
    //                    <attribute name='accountid' />
    //                    <attribute name='name' />
    //                </link-entity>
    //            </link-entity>
    //        </entity>
    //    </fetch>";

    //    string pagingCookie = null;
    //    int pageNumber = 1;

    //    do
    //    {
    //        var response = await client.RetrieveMultipleAsync(
    //            new FetchExpression(fetchXml.Replace("page='1'", $"page='{pageNumber}'")
    //                                         .Replace("pagingcookie='null'", $"pagingcookie='{pagingCookie}'")));
    //        pagingCookie = response.PagingCookie;

    //        foreach (var entity in response.Entities)
    //        {
    //            var accountId = entity.GetAttributeValue<AliasedValue>("account.accountid")?.Value as Guid?;
    //            var additionalAttributes = includeAdditionalAttributes && accountId.HasValue
    //                ? RetrieveMultiSelectOptionSetValues(accountId.Value, new[] { "nacs_countriesserved", "nacs_statesserved" })
    //                : new Dictionary<string, List<string>>();

    //            // Map entity to the member using the provided mapping function
    //            var member = mapToMember(entity);

    //            // Attach additional attributes
    //            if (includeAdditionalAttributes && member is Executive executiveMember)
    //            {
    //                executiveMember.CountriesServed = additionalAttributes.GetValueOrDefault("nacs_countriesserved") ?? new List<string>();
    //                executiveMember.StatesServed = additionalAttributes.GetValueOrDefault("nacs_statesserved") ?? new List<string>();
    //            }

    //            allMembers.Add(member);
    //        }

    //        pageNumber++;

    //    } while (!string.IsNullOrEmpty(pagingCookie));

    //    return allMembers;
    //}

    //public Dictionary<string, List<string>> RetrieveMultiSelectOptionSetValues(Guid accountId, string[] attributeNames)
    //{
    //    var result = new Dictionary<string, List<string>>();
    //    try
    //    {
    //        var serviceClient = _connectivity.GetServiceClient();

    //        var fetchXml = $@"
    //            <fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
    //              <entity name='account'>
    //                {ConstructAttributesXml(attributeNames)}
    //                <filter type='and'>
    //                  <condition attribute='accountid' operator='eq' value='{accountId}' />
    //                </filter>
    //              </entity>
    //            </fetch>";

    //        var fetchExpression = new FetchExpression(fetchXml);
    //        var response = serviceClient.RetrieveMultiple(fetchExpression);

    //        if (response.Entities.Count > 0)
    //        {
    //            var entity = response.Entities.First();
    //            foreach (var attributeName in attributeNames)
    //            {
    //                if (entity.Attributes.Contains(attributeName))
    //                {
    //                    var optionSet = entity.GetAttributeValue<OptionSetValueCollection>(attributeName);
    //                    var optionLabels = new List<string>();

    //                    foreach (var option in optionSet)
    //                    {
    //                        var optionLabel = RetrieveOptionSetLabel(serviceClient, "account", attributeName, option.Value);
    //                        optionLabels.Add(optionLabel);
    //                    }

    //                    // Add to result dictionary
    //                    result.Add(attributeName, optionLabels);
    //                }
    //                else
    //                {
    //                    result.Add(attributeName, new List<string>());
    //                }
    //            }
    //        }
    //        else
    //        {
    //            // Handle case where no entity is found for the given account ID
    //            foreach (var attributeName in attributeNames)
    //            {
    //                result.Add(attributeName, new List<string>());
    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        Console.WriteLine($"Error retrieving multi-select option set values: {ex.Message}");
    //    }

    //    return result;
    //}

    //private string ConstructAttributesXml(string[] attributeNames)
    //{
    //    var attributesXml = "";
    //    foreach (var attributeName in attributeNames)
    //    {
    //        attributesXml += $"<attribute name='{attributeName}' />";
    //    }
    //    return attributesXml;
    //}

    //private string RetrieveOptionSetLabel(ServiceClient serviceClient, string entityName, string attributeName, int optionValue)
    //{
    //    try
    //    {
    //        var attributeRequest = new RetrieveAttributeRequest
    //        {
    //            EntityLogicalName = entityName,
    //            LogicalName = attributeName,
    //            RetrieveAsIfPublished = true
    //        };

    //        var attributeResponse = (RetrieveAttributeResponse)serviceClient.Execute(attributeRequest);
    //        var attributeMetadata = (EnumAttributeMetadata)attributeResponse.AttributeMetadata;
    //        var optionSetMetadata = attributeMetadata.OptionSet;

    //        var option = optionSetMetadata.Options.FirstOrDefault(o => o.Value == optionValue);
    //        return option != null ? option.Label.UserLocalizedLabel.Label : $"Option Set Value {optionValue}";
    //    }
    //    catch (Exception ex)
    //    {
    //        Console.WriteLine($"Error retrieving option set label: {ex.Message}");
    //        return $"Error: {ex.Message}";
    //    }
    //}
    //public string GetOptionSetLabel(string attributeLogicalName, int optionSetValue)
    //{
    //    var client = _connectivity.GetServiceClient();

    //    var retrieveAttributeRequest = new RetrieveAttributeRequest
    //    {
    //        EntityLogicalName = "account", // Replace with your entity logical name if different
    //        LogicalName = attributeLogicalName,
    //        RetrieveAsIfPublished = true
    //    };

    //    var response = (RetrieveAttributeResponse)client.Execute(retrieveAttributeRequest);
    //    var attributeMetadata = (EnumAttributeMetadata)response.AttributeMetadata;

    //    var option = attributeMetadata.OptionSet.Options.FirstOrDefault(opt => opt.Value == optionSetValue);
    //    return option != null ? option.Label.UserLocalizedLabel.Label : "Unknown";
    //}


}
public static class DictionaryExtensions
{
    public static TValue GetOrAdd<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, Func<TKey, TValue> valueFactory)
    {
        if (!dictionary.TryGetValue(key, out var value))
        {
            value = valueFactory(key);
            dictionary[key] = value;
        }
        return value;
    }
}
