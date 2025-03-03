namespace ConvenienceCares.Models;

public record MenuItemViewModel(int ItemId, string MenuItemName, string MenuLink, bool OpenInNewTab, int ParentId, 
    int ItemOrder, List<MenuItemViewModel> Childern
    )
{
}
