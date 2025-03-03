using CMS.Core;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.PowerPlatform.Dataverse.Client;
using Polly;
using System;

public class Dynamics365Connectivity : IDisposable
{
    private readonly Dynamics365Options _options;
    private readonly IEventLogService _eventLogService;
    private ServiceClient _serviceClient;

    public Dynamics365Connectivity(IOptions<Dynamics365Options> options, IEventLogService eventLogService)
    {
        _options = options.Value ?? throw new ArgumentNullException(nameof(options));
        _eventLogService = eventLogService ?? throw new ArgumentNullException(nameof(eventLogService));

        if (string.IsNullOrEmpty(_options.ClientId) || string.IsNullOrEmpty(_options.ClientSecret) || string.IsNullOrEmpty(_options.OrganizationUrl))
        {
            throw new ArgumentException("Dynamics 365 configuration is missing required values.");
        }

        _serviceClient = InitializeClient();
    }

    private ServiceClient InitializeClient()
    {
        var retryPolicy = Policy
            .Handle<Exception>()
            .WaitAndRetry(
                retryCount: 3,
                sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                onRetry: (exception, timeSpan, retryCount, context) =>
                {
                    _eventLogService.LogWarning("Dynamics365Connectivity", "RetryAttempt", $"Attempt {retryCount} failed with error: {exception.Message}. Retrying...");
                });

        return retryPolicy.Execute(() =>
        {
            var connectionString = $"AuthType=ClientSecret;Url={_options.OrganizationUrl};ClientId={_options.ClientId};ClientSecret={_options.ClientSecret};";
            var client = new ServiceClient(connectionString);
            if (!client.IsReady)
            {
                throw new Exception("Failed to connect to Dynamics 365.");
            }
            _eventLogService.LogInformation("Dynamics365Connectivity", "ConnectionSuccess", "Connected to Dynamics 365 successfully.");
            return client;
        });
    }

    public ServiceClient GetServiceClient()
    {
        if (_serviceClient == null || !_serviceClient.IsReady)
        {
            _serviceClient?.Dispose();
            _serviceClient = InitializeClient();
        }
        return _serviceClient;
    }

    public void Dispose()
    {
        _serviceClient?.Dispose();
        GC.SuppressFinalize(this);
    }
}
