using System;

namespace Convenience.org.Components.Widgets.MemberSearchAllCompanies
{
    public class MemberSearchAllCompaniesViewModel
    {
        public Guid AccountId { get; set; }
        public string Name { get; set; }
        public string AccountTypeName { get; set; }
        public string SupplierTypeName { get; set; }
        public string City { get; set; }
        public string StateOrProvince { get; set; }
        public string Country { get; set; }
        public string Region { get; set; }
    }
}
