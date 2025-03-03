using Kentico.Content.Web.Mvc;
using Kentico.PageBuilder.Web.Mvc;
using Kentico.Web.Mvc;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;

using Slugify;

using System.Globalization;

namespace NACSMagazine.Rendering
{
    public class ViewService(
        ISlugHelper slugHelper,
        IWebHostEnvironment env,
        IHttpContextAccessor contextAccessor)
    {
        private readonly IWebHostEnvironment env = env;
        private readonly IHttpContextAccessor contextAccessor = contextAccessor;

        public CultureInfo Culture => CultureInfo.CurrentUICulture;

        public ISlugHelper SlugHelper { get; } = slugHelper;

        public PageBuilderMode PageBuilderMode
        {
            get
            {
                var ctx = contextAccessor.HttpContext;

                if (ctx.Kentico().PageBuilder().EditMode)
                {
                    return PageBuilderMode.Edit;
                }

                if (ctx.Kentico().Preview().Enabled)
                                    {
                    return PageBuilderMode.Preview;
                }

                return PageBuilderMode.Live;
            }
        }

        public bool CacheEnabled =>
            !env.IsDevelopment();
    }

    public enum PageBuilderMode
    {
        Live,
        Edit,
        Preview
    }
}
