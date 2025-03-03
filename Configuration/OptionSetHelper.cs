using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

public class OptionSetHelper
{
    private readonly Dynamics365Connectivity _connectivity;
    private readonly ConcurrentDictionary<string, Dictionary<int, string>> _cache = new();

    public OptionSetHelper(Dynamics365Connectivity connectivity)
    {
        _connectivity = connectivity;
    }

    public string GetOptionSetLabel(string entityLogicalName, string attributeLogicalName, int optionSetValue)
    {
        var key = $"{entityLogicalName}:{attributeLogicalName}";
        var optionSetDict = _cache.GetOrAdd(key, _ => FetchOptionSetLabels(entityLogicalName, attributeLogicalName));

        return optionSetDict.TryGetValue(optionSetValue, out var label) ? label : "Unknown";
    }

    public List<string> GetMultiSelectOptionSetLabels(string entityLogicalName, string attributeLogicalName, OptionSetValueCollection optionSetValues)
    {
        if (optionSetValues == null || !optionSetValues.Any())
            return new List<string>();

        var key = $"{entityLogicalName}:{attributeLogicalName}";
        var optionSetDict = _cache.GetOrAdd(key, _ => FetchOptionSetLabels(entityLogicalName, attributeLogicalName));

        return optionSetValues.Select(option => optionSetDict.TryGetValue(option.Value, out var label) ? label : $"Unknown ({option.Value})").ToList();
    }

    private Dictionary<int, string> FetchOptionSetLabels(string entityLogicalName, string attributeLogicalName)
    {
        var client = _connectivity.GetServiceClient();

        var retrieveAttributeRequest = new RetrieveAttributeRequest
        {
            EntityLogicalName = entityLogicalName,
            LogicalName = attributeLogicalName,
            RetrieveAsIfPublished = true
        };

        var response = (RetrieveAttributeResponse)client.Execute(retrieveAttributeRequest);
        var metadata = (EnumAttributeMetadata)response.AttributeMetadata;

        return metadata.OptionSet.Options
            .Where(option => option.Value.HasValue)
            .ToDictionary(option => option.Value.Value, option => option.Label.UserLocalizedLabel.Label);
    }
}
