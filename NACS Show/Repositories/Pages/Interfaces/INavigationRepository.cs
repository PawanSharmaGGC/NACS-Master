using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NACSShow;
using NACSShow.Models;
namespace NACSShow.Repositories.Pages.Interfaces
{
    public interface INavigationRepository
    {
        Task <IEnumerable<NavigationMenu>> GetMenuItems();
        Task <IEnumerable<NavigationItem>> GetSocialMenuItems();
    }
}
