using System.Windows.Controls;
using MapEditor.Services;
using MapEditor.Services.Manager;
using MapEditor.Views.Elements;

namespace MapEditor.Views
{
    public partial class ItemsView : UserControl
    {
        private static ItemsListService ItemsService
            => ServicesManager.Instance.GetService<ItemsListService>();

        public ItemsView()
        {
            InitializeComponent();
            InitPanel();
            ItemsService.OnItemsListChanged += InitPanel;
        }

        private void InitPanel()
        {
            ItemsStackPanel.Children.Clear();
            var items = ItemsService.LoadItems();
            foreach (var item in items)
            {
                ItemElementView view = new(item.Item, item.Type);
                ItemsStackPanel.Children.Add(view);
            }
        }
    }
}
