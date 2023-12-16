using System.Windows.Controls;
using BadassUniverse_MapEditor.Views.Elements;
using MapEditor.Services;
using MapEditor.Services.Manager;

namespace MapEditor.Views;

public partial class StoredItemsView : UserControl
{
    private static LocalStorageService StorageService
        => ServicesManager.Instance.GetService<LocalStorageService>();
    
    public StoredItemsView()
    {
        InitializeComponent();
        InitPanel();
        StorageService.OnWorldChanged += InitPanel;
    }

    private void InitPanel()
    {
        ItemsStackPanel.Children.Clear();
        var items = StorageService.LoadStoredItems();
        foreach (var item in items)
        {
            StoredItemElementView view = new(item.Item, item.Type);
            ItemsStackPanel.Children.Add(view);
            ItemsStackPanel.Children.Add(new Separator());
        }
    }
}