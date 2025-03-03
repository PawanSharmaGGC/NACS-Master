using System;

namespace Convenience.org.Components.Widgets.SubscriptionsNACSMagazine
{
    public class SubscriptionNACSMagazineViewModel
    {
        public Guid UserId { get; set; }
        public string Country { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string SelectedOption { get; set; }
        public bool IsPrintDigital { get; set; }
        public bool IsPrintOnly { get; set; }
        public bool IsDigitalOnly { get; set; }
        public bool IsUnsubscribeBoth { get; set; }
    }
}
