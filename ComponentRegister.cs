using Kentico.PageBuilder.Web.Mvc;
using Convenience.org.Components.Widgets;
using Convenience.org.Components.Widgets.Cards;
using Convenience.org.Components.Widgets.Subscribe;
using Convenience.org.Components.Widgets.FormDownloadReport;
using Convenience.org.Components.Widgets.FormNACSCEOSummit;
using Convenience.org.Components.Widgets.FormSignUp;
using Convenience.org.Components;

//Register widgets
[assembly: RegisterWidget(identifier: "TierThreeContentCard", name: "Tier Three Content Card", viewComponentType: typeof(TierThreeContentCardViewComponent), propertiesType: typeof(TierThreeContentCardProperties), Description = "Tier Three Content Card", IconClass = "icon-id-cards", AllowCache = true)]
[assembly: RegisterWidget(identifier: "VideoPlayer", name: "Video Player", viewComponentType: typeof(VideoPlayerViewComponent), propertiesType: typeof(VideoPlayerProperties), Description = "Video Player", IconClass = "icon-media-player", AllowCache = true)]
[assembly: RegisterWidget(identifier: "NACSCTACardNoImage", name: "NACS CTA Card No Image", viewComponentType: typeof(NacsCtaCardNoImageViewComponent), propertiesType: typeof(NacsCtaCardNoImageProperties), Description = "Nacs Cta Card No Image", IconClass = "icon-id-card", AllowCache = true)]
[assembly: RegisterWidget(identifier: "NACSCTACardWithImage", name: "NACS CTA Card With Image", viewComponentType: typeof(NACSCTACardWithImageViewComponent), propertiesType: typeof(NACSCTACardWithImageProperties), Description = "Nacs Cta Card With Image", IconClass = "icon-id-cards", AllowCache = true)]
[assembly: RegisterWidget(identifier: "EventVideoCard", name: "Event Video Card", viewComponentType: typeof(EventVideoCardViewComponent), propertiesType: typeof(EventVideoCardProperties), Description = "Event Video Card", IconClass = "icon-media-player", AllowCache = true)]
[assembly: RegisterWidget(identifier: "ExternalSiteCard", name: "External Site Card", viewComponentType: typeof(ExternalSiteCardViewComponent), propertiesType: typeof(ExternalSiteCardProperties), Description = "External Site Card", IconClass = "icon-id-cards", AllowCache = true)]
[assembly: RegisterWidget(identifier: "PullQuote", name: "Pull Quote", viewComponentType: typeof(PullQuoteViewComponent), propertiesType: typeof(PullQuoteProperties), Description = "Pull Quote", IconClass = "icon-right-double-quotation-mark", AllowCache = true)]
[assembly: RegisterWidget(identifier: "Subscribe", name: "Subscribe", viewComponentType: typeof(SubscribeViewComponent), propertiesType: typeof(SubscribeProperties), Description = "Subscribe", IconClass = "icon-newspaper", AllowCache = true)]
[assembly: RegisterWidget(identifier: "CompanyAndFeatureProfileCard", name: "Company And Feature Profile Card", viewComponentType: typeof(CompanyAndFeatureProfileCardViewComponent), propertiesType: typeof(CompanyAndFeatureProfileCardProperties), Description = "Company And Feature Profile Card", IconClass = "icon-id-card", AllowCache = true)]
[assembly: RegisterWidget(identifier: "Table", name: "Table", viewComponentType: typeof(TableViewComponent), propertiesType: typeof(TableProperties), Description = "Table", IconClass = "icon-table", AllowCache = true)]
[assembly: RegisterWidget(identifier: "FormDownloadReport", name: "Form Download Report", viewComponentType: typeof(FormDownloadReportViewComponent), propertiesType: typeof(FormDownloadReportProperties), Description = "Form Download Report", IconClass = "icon-form", AllowCache = true)]
[assembly: RegisterWidget(identifier: "FormNACSCEOSummit", name: "Form NACS CEO Summit", viewComponentType: typeof(FormNACSCEOSummitViewComponent), propertiesType: typeof(FormNACSCEOSummitProperties), Description = "Form NACS CEO Summit", IconClass = "icon-form", AllowCache = true)]
[assembly: RegisterWidget(identifier: "FormSignUp", name: "Form Sign Up", viewComponentType: typeof(FormSignUpViewComponent), propertiesType: typeof(FormSignUpProperties), Description = "Form Sign Up", IconClass = "icon-form", AllowCache = true)]
[assembly: RegisterWidget(identifier: "ProrityPointsDisplay", name: "Prority Points Display", viewComponentType: typeof(ProrityPointsDisplayViewComponent), Description = "Prority Points Display", IconClass = "icon-star-semi", AllowCache = true)]
[assembly: RegisterWidget(identifier: "CopyBlock", name: "Copy Block", viewComponentType: typeof(CopyBlockViewComponent), propertiesType: typeof(CopyBlockProperties), Description = "Copy Block", IconClass = "icon-dialog-window", AllowCache = true)]
//[assembly: RegisterWidget(identifier: "ProrityPointsDisplay", name: "Prority Points Display", viewComponentType: typeof(ProrityPointsDisplayViewComponent), Description = "Prority Points Display", IconClass = "icon-star-semi", AllowCache = true)]
[assembly: RegisterWidget(identifier: "RolesRefresh", name: "Roles Refresh", viewComponentType: typeof(RolesRefreshViewComponent), Description = "Roles Refresh", IconClass = "icon-engage-users", AllowCache = true)]