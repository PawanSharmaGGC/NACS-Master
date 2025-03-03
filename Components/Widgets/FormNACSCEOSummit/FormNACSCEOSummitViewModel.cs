using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Convenience.org.Components.Widgets.FormNACSCEOSummit
{
    public class FormNACSCEOSummitViewModel
    {
        
        [MaxLength(50, ErrorMessage = "Maximum allowed length of the firstName is {1}")]
        public string FirstName { get; set; }

        [MaxLength(50, ErrorMessage = "Maximum allowed length of the lastName is {1}")]
        public string LastName { get; set; }

        [MaxLength(50, ErrorMessage = "Maximum allowed length of the email is {1}")]
        public string Email { get; set; }

        [MaxLength(50, ErrorMessage = "Maximum allowed length of the companyname is {1}")]
        public string CompanyName { get; set; }

        public string DropDown { get; set; }


    }
}
