using System;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using MapEditor.Models;
using MapEditor.Models.Server;
using MapEditor.Services;
using MapEditor.Services.Manager;

namespace MapEditor.Views.Elements
{
    public partial class ItemElementView : UserControl
    {
        public ItemElementView(AItemDTO item, ItemType type)
        {
            InitializeComponent();

            string imageName = type switch
            {
                ItemType.Room => "Room",
                ItemType.Building => "Facade",
                ItemType.Mob => "Mob",
                ItemType.PhysicsItem => "Item",
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };
            ItemIcon.Source = ImagesStorage.GetImage(imageName, "Icons");
            
            ItemName.Text = type switch
            {
                ItemType.Room => ((RoomDTO) item).Name,
                ItemType.Building => ((FacadeDTO) item).Name,
                ItemType.Mob => ((MobDTO) item).Name,
                ItemType.PhysicsItem => ((PhysicsItemDTO) item).Name,
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };
            
            ItemButton.Click += (sender, e) =>
            {
                ServicesManager.Instance.GetService<PreviewService>().SetPreviewItem(item, type);
            };
        }
    }
}
