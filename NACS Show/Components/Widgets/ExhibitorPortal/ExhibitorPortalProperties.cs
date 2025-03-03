using CMS.Helpers;
using Kentico.PageBuilder.Web.Mvc;
using Kentico.Xperience.Admin.Base.FormAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NACSShow.Components.Widgets.ExhibitorPortal
{
    public class ExhibitorPortalProperties : IWidgetProperties
    {
        //General Settings
        [TextInputComponent(Label = "netFORUM Exhibit Key", Order = 0)]
        [RequiredValidationRule]
        public string ExbKey { get; set; } = "14EF78B3-D0FC-4B19-83DF-4E818B959193";
      
        [TextInputComponent(Label = "Show Year", Order = 1)]
        [RequiredValidationRule]
        public string ShowYear { get; set; } = "2018";

        //Map Your Show Dashboard
        [CheckBoxComponent(Label = "Show Panel Dashboard", Order = 2)]
        public bool ShowPanel_MYSDashboard { get; set; }

        [RadioGroupComponent(Label = "Dashboard Status", Inline = false, Order = 3, Options = $"open;Open\r\nclosed;Closed\r\n")]
        public string Status_MYSDashboard { get; set; } = "closed";

        [TextAreaComponent(Label = "Dashboard Description", Order = 4)]
        public string Description_MYSDashboard { get; set; }

        [TextInputComponent(Label = "Map Your Show Dashboard URL(Production)", Order = 5)]
        [RequiredValidationRule]
        public string MapYourShowDashboardURL_Production { get; set; } = "https://nacs24.exh.mapyourshow.com/7_0/main/ssoLogin";

        [TextInputComponent(Label = "Map Your Show Dashboard URL(Staging)", Order = 6)]
        public string MapYourShowDashboardURL_Staging { get; set; } = "https://nacs24.exh.mysstaging.com/7_0/main/ssoLogin";

        //Registration
        [CheckBoxComponent(Label = "Show Panel Registration", Order = 7)]
        public bool ShowPanel_ExperientRegistration { get; set; } = true;

        [RadioGroupComponent(Label = "Registration Status", Inline = false, Order = 8, Options = $"open;Open\r\nclosed;Closed\r\n")]
        public string Status_ExperientRegistration { get; set; } = "closed";

        [TextInputComponent(Label = "Registration Status Message", Order = 9)]
        public string StatusMsg_ExperientRegistration { get; set; } = "Now Open";

        [TextAreaComponent(Label = "Registration Description", Order = 10)]
        public string Description_ExperientRegistration { get; set; } = "Booth personnel registrations, badge changes, full conference upgrades, etc.";

        [TextInputComponent(Label = "Registration URL", Order = 11)]
        [RequiredValidationRule]
        public string NACSShowRegURL { get; set; } = "https://registration.experientevent.com/ShowNAC181/FLOW/EXH/";

        //Housing
        [CheckBoxComponent(Label = "Show Panel Housing", Order = 12)]
        public bool ShowPanel_Housing { get; set; } = true;

        [RadioGroupComponent(Label = "Housing Status", Inline = false, Order = 13, Options = $"open;Open\r\nclosed;Closed\r\n")]
        public string Status_Housing { get; set; } = "closed";

        [TextInputComponent(Label = "Housing Status Message", Order = 14)]
        public string StatusMsg_Housing { get; set; } = "Now Open";

        [TextAreaComponent(Label = "Housing Description", Order = 15)]
        public string Description_Housing { get; set; } = "Book your hotel rooms for the NACS Show through the official housing agency, Connections Housing.";

        [TextInputComponent(Label = "Housing URL", Order = 16)]
        public string NACSHousingURL { get; set; } = "https://book.passkey.com/gt/218649627?gtid=7cff6d57c813c4f2cd55a1c5ae48ffc5";

        //Lead Retrieval
        [CheckBoxComponent(Label = "Show Panel Lead Retrieval", Order = 17)]
        public bool ShowPanel_ExperientLeadRetrieval { get; set; } = true;

        [RadioGroupComponent(Label = "Lead Retrieval Status", Inline = false, Order = 18, Options = $"open;Open\r\nclosed;Closed\r\n")]
        public string Status_ExperientLeadRetrieval { get; set; } = "closed";

        [TextInputComponent(Label = "Lead Retrieval Status Message", Order = 19)]
        public string StatusMsg_ExperientLeadRetrieval { get; set; } = "Now Open";

        [TextAreaComponent(Label = "Lead Retrieval Description", Order = 20)]
        public string Description_ExperientLeadRetrieval { get; set; } = "Access leads generated with onsite badge scans (pre-show purchase required).";

        [TextInputComponent(Label = "Lead Retrieval URL", Order = 21)]
        public string NACSShowLeadRetrievalURL { get; set; } = "https://exhibitor.experientswap.com/";

        //Exhibitor Service Kit
        [CheckBoxComponent(Label = "Show Panel Exhibitor Service Kit", Order = 22)]
        public bool ShowPanel_ExhibitorServiceKit { get; set; } = true;

        [RadioGroupComponent(Label = "Exhibitor Service Kit Status", Inline = false, Order = 23, Options = $"open;Open\r\nclosed;Closed\r\n")]
        public string Status_ExhibitorServiceKit { get; set; } = "closed";
        
        [TextInputComponent(Label = "Exhibitor Service Kit Status Message", Order = 24)]
        public string StatusMsg_ExhibitorServiceKit { get; set; } = "Now Available";

        [TextAreaComponent(Label = "Exhibitor Service Kit Description", Order = 25)]
        public string Description_ExhibitorServiceKit { get; set; } = "Prepare for the NACS Show. Access move-in/out details, shipping labels and information, utility orders, furnishings, and marketing options.";

        //Marketing
        [CheckBoxComponent(Label = "Show Panel Marketing", Order = 26)]
        public bool ShowPanel_MarketingOrders { get; set; } = true;

        [RadioGroupComponent(Label = "Marketing Status", Inline = false, Order = 27, Options = $"open;Open\r\nclosed;Closed\r\n")]
        public string Status_MarketingOrders { get; set; } = "closed";

        [TextInputComponent(Label = "Marketing Status Message", Order = 28)]
        public string StatusMsg_MarketingOrders { get; set; } = "Now Open";

        [TextAreaComponent(Label = "Marketing Description", Order = 29)]
        public string Description_MarketingOrders { get; set; } = "Advertising, Cool New Products Preview Room, onsite marketing, etc. <a href='/marketing' target='_blank'>Learn about marketing opportunities...</a>.";

        //Ambassadors
        [CheckBoxComponent(Label = "Show Panel Ambassadors", Order = 30)]
        public bool ShowPanel_Ambassadors { get; set; } = true;

        [RadioGroupComponent(Label = "Ambassadors Status", Inline = false, Order = 31, Options = $"open;Open\r\nclosed;Closed\r\n")]
        public string Status_Ambassadors { get; set; } = "closed";

        [TextInputComponent(Label = "Ambassadors Status Message", Order = 32)]
        public string StatusMsg_Ambassadors { get; set; } = "Now Open";

        [TextInputComponent(Label = "Ambassadors Description", Order = 33)]
        public string Description_Ambassadors { get; set; } = "<a href='/Exhibit/Ambassadors/'>Ambassadors</a> volunteer for time slots to help around the NACS Show.";

        [DateTimeInputComponent(Label = "Ambassador Registration Open Date", Order = 34)]
        [RequiredValidationRule]
        public DateTime AmbassadorRegOpenDate { get; set; } = new DateTime(2018, 7, 10, 00, 00, 00);

        //Contractors
        [CheckBoxComponent(Label = "Show Panel Contractors", Order = 35)]
        public bool ShowPanel_Contractors { get; set; } = true;

        [RadioGroupComponent(Label = "Contractors Status", Inline = false, Order = 36, Options = $"open;Open\r\nclosed;Closed\r\n")]
        public string Status_Contractors { get; set; } = "closed";

        [TextInputComponent(Label = "Contractors Status Message", Order = 37)]
        public string StatusMsg_Contractors { get; set; } = "Deadline: Sep 18, 2019";

        [TextAreaComponent(Label = "Contractors Description", Order = 38)]
        public string Description_Contractors { get; set; } = "Non-official, <strong>exhibitor-appointed contractors</strong> not listed on the <a href='/Exhibit/ResourceCenter/PubsContractors' target='_blank'>NACS Show Official List of Contractors</a> must be registered here.";


        //Attendee Lists
        [CheckBoxComponent(Label = "Show Panel Attendee Lists", Order = 39)]
        public bool ShowPanel_AttendeeLists { get; set; } = true;

        [RadioGroupComponent(Label = "Attendee Lists Status", Inline = false, Order = 40, Options = $"open;Open\r\nclosed;Closed\r\n")]
        public string Status_AttendeeLists { get; set; } = "closed";

        [TextInputComponent(Label = "Attendee Lists Status Message", Order = 41)]
        public string StatusMsg_AttendeeLists { get; set; } = "2020 Pre-Show List Available this summer";

        [TextAreaComponent(Label = "Attendee Lists Description", Order = 42)]
        public string Description_AttendeeLists { get; set; } = "NACS Show mailing lists offer exhibitors an opportunity to amplify on-site lead collection. Attendees' mailing addresses are provided for your direct pre- or post-Show marketing initiatives. Final Attendee List will be available October 18, 2019.";


        [TextInputComponent(Label = "Attendee List SKUs Available", Order = 43)]
        [RequiredValidationRule]
        public string AttendeeListSKUs_Available { get; set; } = "E9000070";


        [TextInputComponent(Label = "Attendee List SKUs Purchased", Order = 44)]
        [RequiredValidationRule]
        public string AttendeeListSKUs_Purchased { get; set; } = "E9000071";
    }
}
