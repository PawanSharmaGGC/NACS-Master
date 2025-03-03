using Microsoft.Extensions.DependencyInjection;
using NACSShow.Repositories.Pages;
using NACSShow.Repositories.Pages.Interfaces;

namespace NACSShow.Helpers
{
    public static class IServiceCollectionExtensions
    {
        public static void AddDIServices(this IServiceCollection services) 
        {
            AddRepositories(services);
        }

        /// <summary>
        /// Method to add repository scope
        /// </summary>
        /// <param name="services"></param>
        private static void AddRepositories(IServiceCollection services)
        {
            services.AddSingleton<IContentRepository,ContentRepository>();
            services.AddSingleton<INavigationRepository,NavigationRepository>();
        }
    }
}
