using ConvenienceCares.Models;

namespace ConvenienceCares.Interface.Services;

public interface IMenuItemService
{
    Task<List<MenuItemViewModel>> GetMenuItemViewModels(string languageName, string navigationMenuFolderPath, CancellationToken cancellationToken = default);

}
