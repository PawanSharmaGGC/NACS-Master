using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NACSShow.Models
{
    public class NavigationMenu
    {
        public NavigationItem Menu { get; set; } = new NavigationItem();
        public IEnumerable<NavigationItem> SubMenu { get; set; } = new List<NavigationItem>();
    }
}
