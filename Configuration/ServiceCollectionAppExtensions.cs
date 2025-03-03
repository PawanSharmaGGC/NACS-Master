using ConvenienceCares.Interface.Services;
using ConvenienceCares.Repository;
using ConvenienceCares.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NACS.Portal.Core.Services;
using System.Reflection;

namespace ConvenienceCares.Configuration;

public static class ServiceCollectionAppExtensions
{
    public static IServiceCollection AddAppServices(this IServiceCollection services, IConfiguration config) =>
        services
        .AddRepositories()
        .AddServices()
        .AddOperations(config)
        ;


    private static IServiceCollection AddRepositories(this IServiceCollection services) =>
    services
    .AddSingleton<SocialLinkRepository>()
        .AddSingleton<MenuItemRepository>()
        ;

    private static IServiceCollection AddServices(this IServiceCollection services) =>
        services
        .AddSingleton<IAssetItemService, AssetItemService>()
        .AddSingleton<IMenuItemService, MenuItemService>()
        .AddSingleton<SocialMetaTagService>()
        .AddSingleton<IFormProcessingService, FormProcessingService>()
        ;

    private static IServiceCollection AddOperations(this IServiceCollection services, IConfiguration config) =>
    services
        .AddMediatR(c =>
        {
            c.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
        })
        ;

}

