using Convenience.org.Components.Sections.CAccordionSection;
using Convenience.org.Components.Sections.ThreeColumnSection;
using Convenience.org.Components.Sections.TwoColumnSection;
using Kentico.PageBuilder.Web.Mvc;
using Kentico.PageBuilder.Web.Mvc.PageTemplates;

// Register Sections
[assembly: RegisterSection("Convenience.AccordionSection", "Convenience Accordion Section",
    typeof(CAccordionSectionProperties), customViewName: "~/Components/Sections/ConvenienceAccordionSection/_CAccordionSection.cshtml", Description = "Convenience Accordion Section", IconClass = "icon-accordion")]

[assembly: RegisterSection("Convenience.Section.TwoColumnSection", "Two Column Section",
    typeof(TwoColumnSectionProperties), customViewName: "~/Components/Sections/TwoColumnSection/_TwoColumnSection.cshtml", Description = "Two Column Section", IconClass = "icon-pause")]

[assembly: RegisterSection("Convenience.Section.ThreeColumnSection", "Three Column Section",
    typeof(ThreeColumnSectionProperties), customViewName: "~/Components/Sections/ThreeColumnSection/_ThreeColumnSection.cshtml", Description = "Three Column Section", IconClass = "icon-l-cols-3")]

// Register PageTemplate
[assembly: RegisterPageTemplate("Convenience.Template.FormPage", "Form Page", customViewName: "PageTemplates/FormPage/_FormPage.cshtml", Description = "Form Page")]
[assembly: RegisterPageTemplate("Convenience.Template.GenericLeadGenPage", "Generic Lead Gen Page", customViewName: "PageTemplates/GenericLeadGenPage/_GenericLeadGenPage.cshtml", Description = "Generic Lead Gen Page")]
[assembly: RegisterPageTemplate("Convenience.Template.TopicPage", "Topic Page", customViewName: "PageTemplates/TopicPage/_TopicPage.cshtml", Description = "Topic Page")]
[assembly: RegisterPageTemplate("Convenience.Template.L1StatisticsPage", "L1Statistics Page", customViewName: "PageTemplates/L1StatisticsPage/_L1StatisticsPage.cshtml", Description = "L1Statistics Page")]
[assembly: RegisterPageTemplate("Convenience.PublishedPage", "Published Page", customViewName: "~/PageTemplates/PublishedPage/_PublishedPage.cshtml", Description = "Published Page Template")]