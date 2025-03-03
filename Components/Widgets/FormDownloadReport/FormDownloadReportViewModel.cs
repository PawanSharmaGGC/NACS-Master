using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Convenience.org.Components.Widgets.FormDownloadReport
{
    public class FormDownloadReportViewModel
    {
        [MaxLength(50, ErrorMessage = "Maximum allowed length of the firstName is {1}")]
        public string FirstName { get; set; }

        [MaxLength(50, ErrorMessage = "Maximum allowed length of the lastName is {1}")]
        public string LastName { get; set; }

       
        
        [MaxLength(50, ErrorMessage = "Maximum allowed length of the email is {1}")]
        public string Email { get; set; }

        [MaxLength(50, ErrorMessage = "Maximum allowed length of the companyname is {1}")]
        public string CompanyName { get; set; }

        public string UploadFile { get; set; }

       

        public string Placeholder { get; set; }

        [MaxLength(550, ErrorMessage = "Maximum allowed length of the message field is {1}")]
        public string Message { get; set; }

        [MaxLength(50, ErrorMessage = "Maximum allowed length of the DropDown is {1}")]
        public string DropDown { get; set; }





    }
}
