using System.Collections.Generic;

namespace Convenience.org.Models
{
    public class NavBarMenuViewModel
    {
        public NavbarItemViewModel Menu { get; set; } = new NavbarItemViewModel();
        public IEnumerable<NavbarItemViewModel> SubMenu { get; set; } = new List<NavbarItemViewModel>();
    }
}
