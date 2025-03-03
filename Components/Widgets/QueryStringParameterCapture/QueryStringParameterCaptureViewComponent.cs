using Convenience.org.Components.Widgets.QueryStringParameterCapture;
using Kentico.PageBuilder.Web.Mvc;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using NACS_Classes;
using Microsoft.AspNetCore.Http;
using System.Text;
using CMS.ContentEngine;
using CMS.Membership;
using CMS.Websites.Routing;
using CMS.Websites;
using Kentico.Content.Web.Mvc;
using NACS.Protech.Entities;
using NACS.Protech.Framework;

[assembly: RegisterWidget(identifier: QueryStringParameterCaptureViewComponent.IDENTIFIER, name: "QueryStringParameterCapture",
    viewComponentType: typeof(QueryStringParameterCaptureViewComponent), Description = "QueryStringParameterCapture",
    IconClass = "icon-list", AllowCache = true)]

namespace Convenience.org.Components.Widgets.QueryStringParameterCapture
{
    public class QueryStringParameterCaptureViewComponent : ViewComponent
    {
        public const string IDENTIFIER = "QueryStringParameterCapture";
        private readonly IHttpContextAccessor httpContextAccessor;

        public QueryStringParameterCaptureViewComponent(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            QueryStringParameterCaptureViewModel vm = new QueryStringParameterCaptureViewModel();

            //generic source codes from wherever and whenever you need them
            //used in small event registration forms
            if (!string.IsNullOrEmpty(NACSUtilities.GetQueryStringValue("src")))
            {
                httpContextAccessor.HttpContext.Session.SetString("QSSourceCode", NACSUtilities.GetQueryStringValue("src"));
            }
            else if (!string.IsNullOrEmpty(NACSUtilities.GetQueryStringValue("utm_source")))
            {
                httpContextAccessor.HttpContext.Session.SetString("QSSourceCode", NACSUtilities.GetQueryStringValue("utm_source"));
            }

            //Registration source code specifically to connect e-marketing - MUST be entered into Reg system before use
            if (!string.IsNullOrEmpty(NACSUtilities.GetQueryStringValue("nsregsrc")))
            {
                httpContextAccessor.HttpContext.Session.SetString("QSNACSShowRegSourceCode", NACSUtilities.GetQueryStringValue("nsregsrc"));
            }
            else if (!string.IsNullOrEmpty(NACSUtilities.GetQueryStringValue("srccode")))
            {
                httpContextAccessor.HttpContext.Session.SetString("QSNACSShowRegSourceCode", NACSUtilities.GetQueryStringValue("srccode"));
            }
            else
            {
                //set to UTM_CONTENT code
                if (!string.IsNullOrEmpty(NACSUtilities.GetQueryStringValue("utm_content")))
                {
                    httpContextAccessor.HttpContext.Session.SetString("QSNACSShowRegSourceCode", NACSUtilities.GetQueryStringValue("utm_content"));
                }
                else
                {
                    //set to UTM_CAMPAIGN code
                    if (!string.IsNullOrEmpty(NACSUtilities.GetQueryStringValue("utm_campaign")))
                    {
                        httpContextAccessor.HttpContext.Session.SetString("QSNACSShowRegSourceCode", NACSUtilities.GetQueryStringValue("utm_campaign"));
                    }
                }
            }

            //NACS ID for reference
            if (!string.IsNullOrEmpty(NACSUtilities.GetQueryStringValue("nacsid")))
            {
                httpContextAccessor.HttpContext.Session.SetString("QSNACSID", NACSUtilities.GetQueryStringValue("nacsid"));
            }
            else
            {
                if (!string.IsNullOrEmpty(NACSUtilities.GetQueryStringValue("utm_term")))
                {
                    if (NACSUtilities.GetQueryStringValue("utm_term").Trim().Length == 10 && NACSUtilities.GetQueryStringValue("utm_term").StartsWith("C-"))
                    {
                        httpContextAccessor.HttpContext.Session.SetString("QSNACSID", NACSUtilities.GetQueryStringValue("utm_term"));
                    }
                }
            }

            //NACS Key for reference
            if (!string.IsNullOrEmpty(NACSUtilities.GetQueryStringValue("nacskey")))
            {
                httpContextAccessor.HttpContext.Session.SetString("QSNACSKEY", NACSUtilities.GetQueryStringValue("nacskey"));
            }

            if (!string.IsNullOrEmpty(NACSUtilities.GetQueryStringValue("showsrc")))
            {
                if (NACSUtilities.GetQueryStringValue("showsrc") == "true")
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append("Hello. ");

                    if (httpContextAccessor.HttpContext.Session.GetString("QSNACSID") != null)
                    {
                        sb.Append("<span>ID: " + httpContextAccessor.HttpContext.Session.GetString("QSNACSID") + "&nbsp;&nbsp;&nbsp;</span>");
                    }
                    if (httpContextAccessor.HttpContext.Session.GetString("QSNACSKEY") != null)
                    {
                        sb.Append("<span>KEY: " + httpContextAccessor.HttpContext.Session.GetString("QSNACSKEY") + "&nbsp;&nbsp;&nbsp;</span>");
                    }
                    if (httpContextAccessor.HttpContext.Session.GetString("QSNACSShowRegSourceCode") != null)
                    {
                        sb.Append("<span>REGSRC: " + httpContextAccessor.HttpContext.Session.GetString("QSNACSShowRegSourceCode") + "&nbsp;&nbsp;&nbsp;</span>");
                    }
                    vm.Content = sb.ToString();
                }
            }
            return View("~/Components/Widgets/QueryStringParameterCapture/_QueryStringParameterCapture.cshtml",vm);
        }
    }
}
