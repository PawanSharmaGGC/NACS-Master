using System.Collections.Generic;

namespace Convenience.org.Models
{
    public class GlobalNavbarViewModel
    {
        public IEnumerable<NavBarMenuViewModel> TopNavBarMenu { get; set; } = new List<NavBarMenuViewModel>();
        public IEnumerable<NavBarMenuViewModel> MainNavBarMenu { get; set; } = new List<NavBarMenuViewModel>();
    }
}
