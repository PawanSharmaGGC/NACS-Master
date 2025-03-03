using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Convenience.org.Components.Widgets.Subscribe
{
    public class SubscribeViewModel
    {
        [Required (ErrorMessage ="Please enter your email")]
        [EmailAddress(ErrorMessage ="Please enter a valid email address")]
        [DisplayName ("Want to stay up to date? Please leave us your email address.")]
        [MaxLength (250,ErrorMessage ="Maximum allowed length of the email is {1}")]
        public string Email { get; set; }
    }
}
