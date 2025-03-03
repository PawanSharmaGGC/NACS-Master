using Convenience.org.Repositories;
using Convenience.org.Repositories.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using NACSShow.Repositories.Pages;
using NACSShow.Repositories.Pages.Interfaces;

using System;

namespace NACSShow.Helpers
{
    public static class IServiceCollectionExtensions
    {
        public static void AddDIServices(this IServiceCollection services) 
        {
            AddRepositories(services);
            AddSessions(services);
        }

        /// <summary>
        /// Method to add repository scope
        /// </summary>
        /// <param name="services"></param>
        private static void AddRepositories(IServiceCollection services)
        {
            services.AddSingleton<IContentRepository,ContentRepository>();
            services.AddSingleton<INavigationRepository,NavigationRepository>();
            services.AddSingleton<INavbarRepository, NavbarRepository>();
            services.AddSingleton<ITestimonialsRepository, TestimonialsRepository>();
            services.AddSingleton<ISponsoresRepository, SponsoresRepository>();
            services.AddSingleton<IEventDetailsRepository, EventDetailsRepository>();
            services.AddSingleton<IFAQRepository, FAQRepository>();
            services.AddSingleton<IEventPageRepository, EventPageRepository>();
            services.AddSingleton<IProductRepository, ProductRepository>();
            services.AddSingleton<IPersonBioRepository, PersonBioRepository>();
            services.AddSingleton<IFeaturedCardRepository, FeaturedCardRepository>();
            services.AddSingleton<IDeepDiveRepository, DeepDiveRepository>();
        }

        private static void AddSessions(IServiceCollection services)
        {
            services.AddDistributedMemoryCache();
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });
        }
    }
}
