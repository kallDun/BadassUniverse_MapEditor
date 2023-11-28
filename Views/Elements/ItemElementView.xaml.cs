using BadassUniverse_MapEditor.Services;
using System;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace BadassUniverse_MapEditor.Views.Elements
{
    public partial class ItemElementView : UserControl
    {
        private ItemData item;

        public ItemElementView(ItemData item)
        {
            InitializeComponent();
            this.item = item;

            string imageName = item.Type switch
            {
                ItemType.Room => "Room.png",
                ItemType.Building => "Building.png",
                ItemType.Mob => "Mob.png",
                ItemType.Item => "Item.png",
                _ => "Room.png"
            };

            ItemIcon.Source = new BitmapImage(new Uri($"pack://application:,,,/Assets/Icons/{imageName}"));
            ItemName.Text = item.Name;
        }

    }
}
