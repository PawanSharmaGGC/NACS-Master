using Kentico.PageBuilder.Web.Mvc;
using Kentico.PageBuilder.Web.Mvc.PageTemplates;
//using NACSShow.Components.Widgets.AlumniNACSShowEventRSVPForm;
//using NACSShow.Components.Widgets.ExhibitorSupportTeamQuestionsForm;
//using NACSShow.Components.Widgets.MobileAppAvailableNotificationForm;
//using NACSShow.Components.Widgets.NACSShowNotifyMeForm;
//using NACSShow.Components.Widgets.NACSShowRegistrationAlertForm;
//using NACSShow.Components.Widgets.NACSShowUpdatesForm;
//using NACSShow.Components.Widgets.NSVirtualExperienceAlertForm;
using NACSShow;
//using NACSShow.Components.Widgets;
using NACSShow.Components.Sections;
using NACSShow.Components.Widgets.LeftNavigationWidget;

//Register page templates
[assembly: RegisterPageTemplate("NACSShow.Home", "Post-Show Home", customViewName: "Features/Shared/PageTemplates/_Home.cshtml", Description = "Post-Show Home Page Template")]
[assembly: RegisterPageTemplate("NACSShow.GenericContent", "Generic Content", customViewName: "Features/Shared/PageTemplates/_GenericContent.cshtml", Description = "Default Site Generic Content Template")]
[assembly: RegisterPageTemplate("NACSShow.2021LandingWide", "2021 Landing Wide", customViewName: "Features/Shared/PageTemplates/_2021LandingWide.cshtml", Description = "2021 Landing Wide Template")]
[assembly: RegisterPageTemplate("NACSShow.Page", "Generic Show Page", customViewName: "Features/Shared/PageTemplates/_Page.cshtml", Description = "Generic Page Template")]
[assembly: RegisterPageTemplate("NACSShow.2021EducationSessionsLanding", "2021 Education Sessions Landing", customViewName: "Features/Shared/PageTemplates/_2021EducationSessionsLanding.cshtml", Description = "2021 Education Sessions Landing Template")]
[assembly: RegisterPageTemplate("NACSShow.Video", name: "Nacs Show Video Page", customViewName: "Features/Shared/PageTemplates/_VideoPage.cshtml", Description = "Nacs Show Video Page Template", ContentTypeNames = [Video.CONTENT_TYPE_NAME])]
[assembly: RegisterPageTemplate("NACSShow.NewsArticle", name: "Nacs Show News Article Page", customViewName: "Features/Shared/PageTemplates/_NSNewsArticle.cshtml", Description = "Nacs Show News Article Page Template", ContentTypeNames = [NewsArticlePage.CONTENT_TYPE_NAME, DailyNewsPage.CONTENT_TYPE_NAME])]
[assembly: RegisterPageTemplate("NACSShow.2021Full-WidthTextPage", name: "2021 Full Width Text Page", customViewName: "Features/Shared/PageTemplates/_2021FullWidthTextPage.cshtml", Description = "2021 Full-width Text Page", ContentTypeNames = [Page.CONTENT_TYPE_NAME])]
[assembly: RegisterPageTemplate("NACSShow.2021FullWidthTextPageNoMiddleAd", name: "2021 Full Width Text Page No Middle Ad", customViewName: "Features/Shared/PageTemplates/_2021FullWidthTextPageNoMiddleAd.cshtml", Description = "2021 Full-width Text Page No Middle Ad", ContentTypeNames = [Page.CONTENT_TYPE_NAME])]
[assembly: RegisterPageTemplate("NACSShow.Landing-Wide", name: "Landing Wide", customViewName: "Features/Shared/PageTemplates/_LandingWide.cshtml", Description = "Landing Wide", ContentTypeNames = [Convenience.Page.CONTENT_TYPE_NAME, Convenience.EventPage.CONTENT_TYPE_NAME])]
[assembly: RegisterPageTemplate("NACSShow.2024Landing-MixedColumns", name: "2024 Landing Mixed Columns", customViewName: "Features/Shared/PageTemplates/_2024LandingMixedColumns.cshtml", Description = "2024 Landing Mixed Columns", ContentTypeNames = [Page.CONTENT_TYPE_NAME])]
[assembly: RegisterPageTemplate("NACSShow.SpeakerSearchResults", name: "Speaker Search Results", customViewName: "Features/Shared/PageTemplates/_SpeakerSearchResults.cshtml", Description = "Search results for Speaker page", ContentTypeNames = [Page.CONTENT_TYPE_NAME])]
[assembly: RegisterPageTemplate("NACSShow.ExhibitorAppointedContractors", name: "Exhibitor Appointed Contractors Applications", customViewName: "Features/Shared/PageTemplates/_ExhibitorAppointedContractors.cshtml", Description = "Exhibitor Appointed Contractors Applications", ContentTypeNames = [Page.CONTENT_TYPE_NAME])]
[assembly: RegisterPageTemplate("NACSShow.2024Landing-121-Columns", name: "2024 Landing 1-2-1 Columns", customViewName: "Features/Shared/PageTemplates/_2024Landing-121-Columns.cshtml", Description = "2024 Landing 1-2-1 Columns", ContentTypeNames = [Page.CONTENT_TYPE_NAME])]
[assembly: RegisterPageTemplate("NACSShow.2021SchedulePage", name: "2021 Schedule Page", customViewName: "Features/Shared/PageTemplates/_2021SchedulePage.cshtml", Description = "2021 Schedule Page", ContentTypeNames = [Page.CONTENT_TYPE_NAME])]
[assembly: RegisterPageTemplate("NACSShow.2024NACSShowHomepage", name: "2024 NACS Show Homepage", customViewName: "Features/Shared/PageTemplates/_2024NACSShowHomepage.cshtml", Description = "2024 NACS Show Homepage")]
[assembly: RegisterPageTemplate("NACSShow.Landing", name: "Landing", customViewName: "Features/Shared/PageTemplates/_Landing.cshtml", Description = "Landing", ContentTypeNames = [Page.CONTENT_TYPE_NAME])]
[assembly: RegisterPageTemplate("NACSShow.GeneralContentSidebar", name: "General Content Sidebar", customViewName: "Features/Shared/PageTemplates/_GeneralContentSidebar.cshtml", Description = "General Content Sidebar", ContentTypeNames = [Page.CONTENT_TYPE_NAME])]
//[assembly: RegisterPageTemplate("NACSShow.Workshop", name: "Workshop", customViewName: "Features/Shared/PageTemplates/_Workshop.cshtml", Description = "Workshop Page", ContentTypeNames = [Workshop.CONTENT_TYPE_NAME])]

//Register widgets
//[assembly: RegisterWidget(identifier: "Alumni2023NACSShowEventRSVPForm", name: "Alumni 2023 nacs show event rsvp form widget", viewComponentType: typeof(AlumniNACSShowEventRSVPFormWidgetViewComponent), propertiesType: typeof(FormWidgetProperties), Description = "Alumni 2023 nacs show event rsvp form widget", IconClass = "icon-form", AllowCache = true)]
//[assembly: RegisterWidget(identifier: "NACSShowRegistrationAlertForm", name: "NACS show registration alert form widget", viewComponentType: typeof(NACSShowRegistrationAlertFormWidgetViewComponent), propertiesType: typeof(FormWidgetProperties), Description = "NACS show registration alert form widget", IconClass = "icon-form", AllowCache = true)]
//[assembly: RegisterWidget(identifier: "NACSShowUpdatesForm", name: "NACS show updates form widget", viewComponentType: typeof(NACSShowUpdatesFormWidgetViewComponent), propertiesType: typeof(FormWidgetProperties), Description = "NACS show updates form widget", IconClass = "icon-form", AllowCache = true)]
//[assembly: RegisterWidget(identifier: "NACSShowNotifyMeForm", name: "2024 nacs show notify me form widget", viewComponentType: typeof(NACSShowNotifyMeFormWidgetViewComponent), propertiesType: typeof(FormWidgetProperties), Description = "2024 nacs show notify me form widget", IconClass = "icon-form", AllowCache = true)]
//[assembly: RegisterWidget(identifier: "NSVirtualExperienceAlertForm", name: "NACS show virtual experience alert form widget", viewComponentType: typeof(NSVirtualExperienceAlertFormWidgetViewComponent), propertiesType: typeof(FormWidgetProperties), Description = "NACS show virtual experience alert form widget", IconClass = "icon-form", AllowCache = true)]
//[assembly: RegisterWidget(identifier: "MobileAppAvailableNotificationForm", name: "Mobile app available notification form widget", viewComponentType: typeof(MobileAppNotificationFormWidgetViewComponent), propertiesType: typeof(FormWidgetProperties), Description = "Mobile app available notification form widget", IconClass = "icon-form", AllowCache = true)]
//[assembly: RegisterWidget(identifier: "ExhibitorSupportTeamQuestionsForm", name: "Exhibitor support team questions form widget", viewComponentType: typeof(ExhibitorSupportTeamQuestionsFormWidgetViewComponent), propertiesType: typeof(FormWidgetProperties), Description = "Exhibitor support team questions form widget", IconClass = "icon-form", AllowCache = true)]
//[assembly: RegisterWidget(identifier: NewsArticleListingViewComponent.IDENTIFIER, name: "News Article Listing Widget", viewComponentType: typeof(NewsArticleListingViewComponent), propertiesType: typeof(NewsArticleListingProperties), Description = "News Article Listing Widget", IconClass = "icon-list", AllowCache = true)]
[assembly: RegisterWidget(identifier: "NSLeftNavigation", name: "Left Navigation", viewComponentType: typeof(LeftNavigationWidgetViewComponent), propertiesType: typeof(LeftNavigationWidgetProperties), Description = "Left Navigation", IconClass = "icon-navigation", AllowCache = true)]

//Register sections
[assembly: RegisterSection("NACSShow.AccordionSection", "Accordion Section", typeof(AccordionSectionProperties), "~/Components/Sections/AccordionSection/_AccordionSection.cshtml", Description = "Accordion Section", IconClass = "icon-accordion")]