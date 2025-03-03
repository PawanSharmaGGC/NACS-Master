using System;

namespace Convenience.org.Components.Widgets.MemberSearchCompanyDetails
{
    public class MemberSearchCompanyDetailsViewModel
    {
        public Guid AccountId { get; set; }
        public string Name { get; set; }
        public string AccountTypeName { get; set; }
        public string SupplierTypeName { get; set; }
        public string Address { get; set; }
        public string WebsiteUrl { get; set; }
        public string Telephone { get; set; }
        public int TotalStores { get; set; }
        public bool IsSaved { get; set; }
        public string Command { get; set; }
    }
}
