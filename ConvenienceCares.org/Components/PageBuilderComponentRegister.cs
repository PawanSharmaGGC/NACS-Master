using Convenience;
using ConvenienceCares.Components;
using ConvenienceCares.Components.Widgets.AwarenessCampaignForm;
using ConvenienceCares.Components.Widgets.ContactForm;
using ConvenienceCares.Components.Widgets.CorporateInvolvementForm;
using ConvenienceCares.Components.Widgets.GetInvolvedForm;
using ConvenienceCares.Components.Widgets.NACSFoundation_24_7DayForm;
using ConvenienceCares.Components.Widgets.ScholarshipUpdatesForm;
using ConvenienceCares.Components.Widgets.SponsorNACSFoundationForm;
using Kentico.PageBuilder.Web.Mvc;
using Kentico.PageBuilder.Web.Mvc.PageTemplates;
using NACS.Portal.Core.Models;


// Widgets
[assembly: RegisterWidget(identifier: ComponentIdentifiers.NACS_FOUNDATION_CONTACT_FORM_WIDGET, name: "Contact form widget", viewComponentType: typeof(ContactFormWidgetViewComponent), propertiesType: typeof(FormWidgetProperties), Description = "Contact form widget", IconClass = "icon-form", AllowCache = true)]
[assembly: RegisterWidget(identifier: ComponentIdentifiers.NACS_FOUNDATION_GETINVOLVED_FORM_WIDGET, name: "Get involved form widget", viewComponentType: typeof(GetInvolvedFormWidgetViewComponent), propertiesType: typeof(FormWidgetProperties), Description = "Get involved form widget", IconClass = "icon-form", AllowCache = true)]
[assembly: RegisterWidget(identifier: ComponentIdentifiers.NACS_FOUNDATION_SPONSORNACS_FORM_WIDGET, name: "Sponsor NACS form widget", viewComponentType: typeof(SponsorNACSFormWidgetViewComponent), propertiesType: typeof(FormWidgetProperties), Description = "Sponsor NACS form widget", IconClass = "icon-form", AllowCache = true)]
[assembly: RegisterWidget(identifier: ComponentIdentifiers.NACS_FOUNDATION_CORPORATE_INVOLVEMENT_FORM_WIDGET, name: "Corporate involvement form widget", viewComponentType: typeof(CorporateInvolvementFormWidgetViewComponent), propertiesType: typeof(FormWidgetProperties), Description = "Corporate involvement form widget", IconClass = "icon-form", AllowCache = true)]
[assembly: RegisterWidget(identifier: ComponentIdentifiers.NACS_FOUNDATION_24_7Day_FORM_WIDGET, name: "NACS 24/7 day form widget", viewComponentType: typeof(NACS_24_7DayFormWidgetViewComponent), propertiesType: typeof(FormWidgetProperties), Description = "NACS 24/7 day form widget", IconClass = "icon-form", AllowCache = true)]
[assembly: RegisterWidget(identifier: ComponentIdentifiers.NACS_FOUNDATION_AWARENESS_CAMPAIGN_FORM_WIDGET, name: "Awareness campaign form widget", viewComponentType: typeof(AwarenessCampaignFormWidgetViewComponent), propertiesType: typeof(FormWidgetProperties), Description = "Awareness campaign form widget", IconClass = "icon-form", AllowCache = true)]
[assembly: RegisterWidget(identifier: ComponentIdentifiers.NACS_FOUNDATION_SCHOLARSHIP_UPDATES_FORM_WIDGET, name: "Scholarship updates form widget", viewComponentType: typeof(ScholarshipUpdatesFormWidgetViewComponent), propertiesType: typeof(FormWidgetProperties), Description = "Scholarship updates form widget", IconClass = "icon-form", AllowCache = true)]


// Page templates
[assembly: RegisterPageTemplate(ComponentIdentifiers.NACS_FOUNDATION_DEFAULT_PAGE_TEMPLATE, "NACS Foundation Landing Page Template", customViewName: "~/PageTemplates/NACSFoundationLandingPage/_NACSFoundationLandingPage.cshtml", ContentTypeNames = new string[] { Page.CONTENT_TYPE_NAME }, Description = "A default NACS foundation landing page template.", IconClass = "xp-l-header-text")]
[assembly: RegisterPageTemplate(ComponentIdentifiers.NACS_FOUNDATION_HOME_PAGE_TEMPLATE, "NACS Foundation Home Page Template", customViewName: "~/PageTemplates/NACSFoundationHomePage/_NACSFoundationHomePage.cshtml", ContentTypeNames = new string[] { Page.CONTENT_TYPE_NAME }, Description = "NACS foundation home page template.", IconClass = "xp-l-header-text")]
[assembly: RegisterPageTemplate(ComponentIdentifiers.NACS_FOUNDATION_CONTACT_PAGE_TEMPLATE, "NACS Foundation Contact Page Template", customViewName: "~/PageTemplates/NACSFoundationContactPage/_NACSFoundationContactPage.cshtml", ContentTypeNames = new string[] { Page.CONTENT_TYPE_NAME }, Description = "NACS foundation contact page template.", IconClass = "xp-l-header-text")]
[assembly: RegisterPageTemplate(ComponentIdentifiers.NACS_FOUNDATION_WIDE_PAGE_TEMPLATE, "NACS Foundation Wide Page Template", customViewName: "~/PageTemplates/NACSFoundationWide/_NACSFoundationWide.cshtml", ContentTypeNames = new string[] { Page.CONTENT_TYPE_NAME }, Description = "NACS foundation wide page template.", IconClass = "xp-l-header-text")]
[assembly: RegisterPageTemplate(ComponentIdentifiers.NACS_FOUNDATION_PROGRAM_LANDING_PAGE_TEMPLATE, "NACS Foundation Program Landing Page Template", customViewName: "~/PageTemplates/NACSFoundationProgramLandingPage/_NACSFoundationProgramLanding.cshtml", ContentTypeNames = new string[] { Page.CONTENT_TYPE_NAME }, Description = "NACS foundation program landing page template.", IconClass = "xp-l-header-text")]
[assembly: RegisterPageTemplate(ComponentIdentifiers.NACS_FOUNDATION_SCHOLARSHIP_PAGE_TEMPLATE, "NACS Foundation Scholarship Page Template", customViewName: "~/PageTemplates/NACSFoundationScholarshipPage/_NACSFoundationScholarshipPage.cshtml", ContentTypeNames = new string[] { Page.CONTENT_TYPE_NAME }, Description = "NACS foundation scholarship page template.", IconClass = "xp-l-header-text")]
[assembly: RegisterPageTemplate(ComponentIdentifiers.NACS_FOUNDATION_24_7Day_PAGE_TEMPLATE, "NACS Foundation 24/7 Day Page Template", customViewName: "~/PageTemplates/NACSFoundation24_7DayPage/_NACSFoundation24_7DayPage.cshtml", ContentTypeNames = new string[] { Page.CONTENT_TYPE_NAME }, Description = "NACS foundation 24_7 day page template.", IconClass = "xp-l-header-text")]
[assembly: RegisterPageTemplate(ComponentIdentifiers.NACS_LANDING_WIDE_NO_SIDEBAR_ADS_PAGE_TEMPLATE, "NACS Landing Wide No Sidebar-ads Page Template", customViewName: "~/PageTemplates/NACSLandingWideNoSidebarAdsPage/_NACSLandingWideNoSidebarAdsPage.cshtml", ContentTypeNames = new string[] { Page.CONTENT_TYPE_NAME }, Description = "NACS landing wide no sidebar ads page template.", IconClass = "xp-l-header-text")]
[assembly: RegisterPageTemplate(ComponentIdentifiers.NACS_FOUNDATION_AWARENESS_CAMPAIGN_PAGE_TEMPLATE, "NACS Foundation Awareness Campaign Page Template", customViewName: "~/PageTemplates/NACSFoundationAwarenessCampaign/_NACSFoundationAwarenessCampaign.cshtml", ContentTypeNames = new string[] { Page.CONTENT_TYPE_NAME }, Description = "NACS foundation awareness campaign page template.", IconClass = "xp-l-header-text")]
[assembly: RegisterPageTemplate(ComponentIdentifiers.CONVENIENCE_CARE_FILE_PAGE_TEMPLATE, "Convenience Care File Page Template", customViewName: "~/PageTemplates/ConvenienceCareFilePage/_ConvenienceCareFilePage.cshtml", ContentTypeNames = new string[] { ConvenienceCare.File.CONTENT_TYPE_NAME }, Description = "Convenience Care File Page Template.", IconClass = "xp-file")]



