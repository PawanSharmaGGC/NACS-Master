using Kentico.Forms.Web.Mvc;
using Kentico.PageBuilder.Web.Mvc;
using Kentico.Xperience.Admin.Base.FormAnnotations;

namespace Convenience.org.Components.Widgets.GeneralContactForm
{
    public class GeneralContactFormProperties : IWidgetProperties
    {
        private const string defaultValue = "0;- select -;dspencer@convenience.org\r\n1;Contact Changes & Updates;dspencer@convenience.org\r\n2;Events;dspencer@convenience.org                              \r\n3;NACS Retail Membership;dspencer@convenience.org\r\n4;NACS Supplier Membership;dspencer@convenience.org\r\n5;NACS Products & Ordering;dspencer@convenience.org\r\n6;Login Issues;dspencer@convenience.org\r\n7;Website Issues;dspencer@convenience.org\r\n8;Privacy: My Information;databaseupdates@convenience.org\r\n9;Regulatory and Government Relations Issues;dspencer@convenience.org\r\n10;Research and Publications;dspencer@convenience.org\r\n11;Unsubscribe from NACS Magazine or NACS Daily;dspencer@convenience.org";

        [TextAreaComponent(Label = "Request Type", Order = 0)]
        public string RequestType { get; set; } = defaultValue;
    }
}
