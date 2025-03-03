using Convenience.org.Components.Widgets.MemberSearchMyDirectory;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Convenience.org.Components.Widgets.MemberSearchMyDirectorySidebar
{
    public class RecentViewHelper
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private const string CookieName = "RecentViews";

        public RecentViewHelper(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public List<RecentViewModel> GetRecentViews()
        {
            var cookie = _httpContextAccessor.HttpContext?.Request.Cookies[CookieName];
            if (string.IsNullOrEmpty(cookie))
            {
                return new List<RecentViewModel>();
            }

            return JsonConvert.DeserializeObject<List<RecentViewModel>>(cookie) ?? new List<RecentViewModel>();
        }

        public void AddRecentView(RecentViewModel view)
        {
            var recentViews = GetRecentViews();

            // Avoid duplicates
            recentViews = recentViews.Where(rv => rv.ItemGUID != view.ItemGUID).ToList();

            // Add the new view to the top
            recentViews.Insert(0, view);

            // Limit to 10 views
            if (recentViews.Count > 10)
            {
                recentViews = recentViews.Take(10).ToList();
            }

            // Save back to cookie
            var cookieValue = JsonConvert.SerializeObject(recentViews);
            _httpContextAccessor.HttpContext?.Response.Cookies.Append(
                CookieName,
                cookieValue,
                new CookieOptions { Expires = DateTime.Now.AddDays(30) }
            );
        }
    }
}
