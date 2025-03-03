using CMS.DataEngine;
using CMS.DocumentEngine;
using CMS.Helpers;
using CMS.Membership;
using CMS.SiteProvider;

using Microsoft.AspNetCore.Http;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace NACS.Components.GlobalNavigation
{
    public partial class CMSWebParts_NACS_NACSShow_GlobalNav : NACSWebPart
    {
        public string NavFolderPath
        {
            get
            {
                return ValidationHelper.GetString(GetValue("NavFolderPath"), "/Global-Nav");
            }
            set
            {
                SetValue("NavFolderPath", value);
            }
        }

        public class NavItem
        {
            public string Title { get; set; }
            public string URL { get; set; }
            public string Class { get; set; }
            public bool Featured { get; set; }
            public string Description { get; set; }
            public string ImageURL { get; set; }
            public bool ShowOnlySecondLevel { get; set; }
            public List<NavItem> ChildItems { get; set; }
        }

        private static object lockObj = new object();

        protected void Page_Load(object sender, EventArgs e)
        {
            string cacheKey = "NACSGlobalNav" + Regex.Replace(this.NavFolderPath, "\\W", "");

            List<NavItem> navItems = HttpContext.Current.Cache[cacheKey] as List<NavItem>;

            if (navItems == null)
            {
                lock (lockObj)
                {
                    navItems = HttpContext.Current.Cache[cacheKey] as List<NavItem>;
                    if (navItems == null)
                    {
                        navItems = new List<NavItem>();
                        TreeProvider tree = new TreeProvider(MembershipContext.AuthenticatedUser);
                        CMS.DocumentEngine.TreeNode navFolder = tree.SelectNodes()
                                        .Path(this.NavFolderPath, PathTypeEnum.Single)
                                        .OnSite(SiteContext.CurrentSiteName)
                                        .FirstOrDefault();

                        if (navFolder != null)
                        {
                            var children = DocumentHelper.GetDocuments()
                                            .OnSite(SiteContext.CurrentSiteName)
                                            .Path(navFolder.NodeAliasPath + "/%")
                                            .Types("Show.NavTopLevel")
                                            .OrderBy("NodeOrder")
                                            .Published(true);

                            foreach (var topLevel in children)
                            {
                                NavItem topLevelItem = new NavItem();
                                topLevelItem.Title = topLevel.GetStringValue("Title", "");
                                topLevelItem.URL = topLevel.GetStringValue("URL", "");
                                topLevelItem.ShowOnlySecondLevel = topLevel.GetBooleanValue("ShowOnlySecondLevel", false);
                                topLevelItem.ChildItems = new List<NavItem>();

                                topLevelItem.ChildItems.Add(new NavItem()
                                {
                                    Title = topLevelItem.Title,
                                    URL = topLevelItem.URL,
                                    Featured = false,
                                    Class = " duplicate-top-link"
                                });

                                var subChildren = DocumentHelper.GetDocuments()
                                            .OnSite(SiteContext.CurrentSiteName)
                                            .Path(topLevel.NodeAliasPath + "/%")
                                            .Types("Show.NavSecondLevel")
                                            .OrderBy("NodeOrder")
                                            .Published(true);
                                int index = 0;

                                string feature = topLevel.GetStringValue("Feature", "");
                                if (!string.IsNullOrEmpty(feature))
                                {
                                    NavItem secondLevelItem = new NavItem();
                                    secondLevelItem.Title = "Feature";
                                    secondLevelItem.URL = string.Empty;
                                    secondLevelItem.Featured = true;
                                    secondLevelItem.Class = " second-level-" + index.ToString();
                                    secondLevelItem.Description = feature;

                                    secondLevelItem.ChildItems = new List<NavItem>();
                                    topLevelItem.ChildItems.Add(secondLevelItem);
                                    index++;
                                }

                                foreach (var secondLevel in subChildren)
                                {
                                    NavItem secondLevelItem = new NavItem();
                                    secondLevelItem.Title = secondLevel.GetStringValue("Title", "");
                                    secondLevelItem.URL = secondLevel.GetStringValue("URL", "");
                                    secondLevelItem.Featured = false;
                                    secondLevelItem.Class = " second-level-" + index.ToString();
                                    //if (secondLevel.ClassName.Contains("Feature"))
                                    //{
                                    //    secondLevelItem.Featured = true;
                                    //    secondLevelItem.Description = secondLevel.GetStringValue("Description", "");
                                    //    secondLevelItem.ImageURL = secondLevel.GetStringValue("Image", "");
                                    //    if (!string.IsNullOrEmpty(secondLevelItem.ImageURL) && secondLevelItem.ImageURL != Guid.Empty.ToString("D"))
                                    //        secondLevelItem.ImageURL = "<img src='/CMSPages/GetFile.aspx?guid=" + secondLevelItem.ImageURL + "' />";
                                    //}

                                    secondLevelItem.ChildItems = new List<NavItem>();

                                    if (!topLevelItem.ShowOnlySecondLevel)
                                    {
                                        secondLevelItem.ChildItems.Add(new NavItem()
                                        {
                                            Title = secondLevelItem.Title,
                                            URL = secondLevelItem.URL,
                                            Featured = false,
                                            Class = " duplicate-second-link"
                                        });

                                        var thirdLevelChildren = DocumentHelper.GetDocuments()
                                                .OnSite(SiteContext.CurrentSiteName)
                                                .Path(secondLevel.NodeAliasPath + "/%")
                                                .Types("Show.NavThirdLevel")
                                                .OrderBy("NodeOrder")
                                                .Published(true);

                                        foreach (var thirdLevel in thirdLevelChildren)
                                        {
                                            NavItem thirdLevelItem = new NavItem();
                                            thirdLevelItem.Title = thirdLevel.GetStringValue("Title", "");
                                            thirdLevelItem.URL = thirdLevel.GetStringValue("URL", "");
                                            thirdLevelItem.Featured = false;
                                            secondLevelItem.ChildItems.Add(thirdLevelItem);
                                        }

                                        feature = secondLevel.GetStringValue("Feature", "");
                                        if (secondLevelItem.ChildItems.Count <= 1 && !string.IsNullOrEmpty(feature))
                                        {
                                            NavItem thirdLevelItem = new NavItem();
                                            thirdLevelItem.Title = "Feature";
                                            thirdLevelItem.URL = string.Empty;
                                            thirdLevelItem.Description = feature;
                                            thirdLevelItem.Featured = true;
                                            secondLevelItem.ChildItems.Add(thirdLevelItem);
                                        }
                                        else if (secondLevelItem.ChildItems.Count > 1)
                                        {
                                            secondLevelItem.Class += " contains-children";
                                        }
                                    }

                                    topLevelItem.ChildItems.Add(secondLevelItem);
                                    index++;
                                }
                                navItems.Add(topLevelItem);
                            }

                            HttpContext.Current.Cache.Add(cacheKey, navItems, null, DateTime.Now.AddMinutes(30), System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.High, null);
                        }
                    }
                }
            }

            if (navItems != null)
            {
                if (CMS.DocumentEngine.DocumentContext.CurrentDocument != null)
                {
                    foreach (NavItem ni in navItems)
                    {
                        if (string.IsNullOrEmpty(ni.URL))
                            continue;

                        string currUrl = MakeRelative(CMS.DocumentEngine.DocumentContext.CurrentDocument.NodeAliasPath).ToLower();
                        string itemUrl = MakeRelative(ni.URL).ToLower();

                        if (currUrl == itemUrl || currUrl == itemUrl + "/")
                        {
                            ni.Class = " current";
                        }
                        else if (currUrl.StartsWith(itemUrl.EndsWith("/") ? itemUrl : itemUrl + "/"))
                        {
                            ni.Class = " current";
                        }
                        else
                        {
                            ni.Class = "";
                        }
                    }
                }
            }

            rptNavigation.DataSource = navItems;
            rptNavigation.DataBind();

            rptNavigationMobile.DataSource = navItems;
            rptNavigationMobile.DataBind();
        }

        private string MakeRelative(string url)
        {
            url = url.ToLower();
            if (url.StartsWith("http://") || url.StartsWith("https://"))
                url = url.Substring(url.IndexOf("/", url.IndexOf("://") + 3));

            return url;
        }

        // Desktop
        protected void rptNavigation_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                NavItem navItem = e.Item.DataItem as NavItem;

                Repeater rptFeaturedNavigation = e.Item.FindControl("rptFeaturedNavigation") as Repeater;
                rptFeaturedNavigation.DataSource = navItem.ChildItems.Where(n => n.Featured);
                rptFeaturedNavigation.DataBind();

                Repeater rptSubNavigation = e.Item.FindControl("rptSubNavigation") as Repeater;
                rptSubNavigation.DataSource = navItem.ChildItems.Where(n => !n.Featured);
                rptSubNavigation.DataBind();

                Repeater rptSubNavigationThird = e.Item.FindControl("rptSubNavigationThird") as Repeater;
                if (navItem.ShowOnlySecondLevel)
                {
                    rptSubNavigationThird.Visible = false;
                    HtmlGenericControl ulSecondLevel = e.Item.FindControl("ulSecondLevel") as HtmlGenericControl;
                    string cssClasses = ulSecondLevel.Attributes["class"];
                    cssClasses = cssClasses.Replace("col-sm-4", "col-sm-8 second-level-only");
                    ulSecondLevel.Attributes["class"] = cssClasses;
                }
                else
                {
                    rptSubNavigationThird.DataSource = navItem.ChildItems.Where(n => !n.Featured && n.ChildItems != null && n.ChildItems.Count > 0);
                    rptSubNavigationThird.DataBind();
                }
            }
        }

        protected void rptSubNavigationThird_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                NavItem navItem = e.Item.DataItem as NavItem;

                Repeater rptFeaturedThirdLevelNavigation = e.Item.FindControl("rptFeaturedThirdLevelNavigation") as Repeater;
                rptFeaturedThirdLevelNavigation.DataSource = navItem.ChildItems.Where(n => n.Featured);
                rptFeaturedThirdLevelNavigation.DataBind();

                Repeater rptThirdLevelNavigation = e.Item.FindControl("rptThirdLevelNavigation") as Repeater;
                rptThirdLevelNavigation.DataSource = navItem.ChildItems.Where(n => !n.Featured);
                rptThirdLevelNavigation.DataBind();
            }
        }

        // Mobile
        protected void rptNavigationMobile_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                NavItem navItem = e.Item.DataItem as NavItem;
                if (navItem != null && navItem.ChildItems != null)
                {
                    Repeater rptSubNavigation = e.Item.FindControl("rptSubNavigation") as Repeater;
                    rptSubNavigation.DataSource = navItem.ChildItems.Where(n => !n.Featured);
                    rptSubNavigation.DataBind();

                    Repeater rptFeaturedNavigation = e.Item.FindControl("rptFeaturedNavigation") as Repeater;
                    rptFeaturedNavigation.DataSource = navItem.ChildItems.Where(n => n.Featured);
                    rptFeaturedNavigation.DataBind();
                }
            }
        }

        protected void rptSubNavigationMobile_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                NavItem navItem = e.Item.DataItem as NavItem;

                if (navItem != null && navItem.ChildItems != null)
                {
                    Repeater rptThirdLevelNavigation = e.Item.FindControl("rptThirdLevelNavigation") as Repeater;
                    rptThirdLevelNavigation.DataSource = navItem.ChildItems.Where(n => !n.Featured);
                    rptThirdLevelNavigation.DataBind();
                }
            }
        }
    }
}
