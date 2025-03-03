using System.Collections.Generic;
using Convenience.org.Models;

namespace Convenience.org.Repositories.Interfaces
{
    public interface INavbarRepository
    {
        IEnumerable<NavBarMenuViewModel> GetNavItems(string navItemPath, int nestingLevel = 1);
    }
}
