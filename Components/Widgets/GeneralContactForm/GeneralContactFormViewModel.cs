using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace Convenience.org.Components.Widgets.GeneralContactForm
{
    public class GeneralContactFormViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CompanyName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string PhoneExtension { get; set; }
        public string ErrorMessage { get; set; }
        public string _hdnCiD { get; set; }
        public string ManageProfileText { get; set; }
        public bool ShowMainPanel { get; set; } = false;
        public bool ShowFinishPanel { get; set; } = false;
        public Dictionary<string, string> ListEmails  { get; set; }
        public List<SelectListItem> ListRequestType { get; set; }
    }
}
