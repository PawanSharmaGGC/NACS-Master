using Microsoft.Extensions.Configuration;
using NACS.Protech.Framework;
using System.Collections.Specialized;

public static class ConfigurationManagerSimulator
{
    public static NameValueCollection AppSettings { get; private set; }

    public static void Initialize(IConfiguration configuration)
    {
        AppSettings = new NameValueCollection
        {
            ["ProtechAPI.BaseUrl"] = configuration["ProtechAPI:BaseUrl"],
            ["ProtechAPI.Key"] = configuration["ProtechAPI:Key"],
            ["ProtechAPI.ClientId"] = configuration["ProtechAPI:ClientId"],
            ["ProtechAPI.MxBaseUrl"] = configuration["ProtechAPI:MxBaseUrl"],
            ["ProtechAPI.MxPassword"] = configuration["ProtechAPI:MxPassword"],
            ["D365.OrganizationUrl"] = configuration["D365:OrganizationUrl"],
            ["D365.ClientId"] = configuration["D365:ClientId"],
            ["D365.ClientSecret"] = configuration["D365:ClientSecret"]
        };

        ApiValues.Initialize(AppSettings);
    }
}