using System;
using System.Windows.Controls;
using MapEditor.Models;
using MapEditor.Models.Server;
using MapEditor.Services.Manager;
using MapEditor.Services.Properties;
using MapEditor.Views.Elements;

namespace BadassUniverse_MapEditor.Views.Elements;

public partial class StoredItemElementView : UserControl
{
    private static PropertiesService PropertiesService
        => ServicesManager.Instance.GetService<PropertiesService>();
    
    public StoredItemElementView(AItemDTO item, ItemType type)
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
        IdTextBlock.Text = type switch
        {
            ItemType.Room => $"ID: {((RoomDTO) item).Id}",
            ItemType.Building => $"ID: {((FacadeDTO) item).Id}",
            ItemType.Mob => $"ID: {((MobDTO) item).Id}",
            ItemType.PhysicsItem => $"ID: {((PhysicsItemDTO) item).Id}",
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };
        
        // animation on hover
        MainGrid.MouseEnter += (sender, e) =>
        {
            MainGrid.Opacity = 0.65;
        };
        MainGrid.MouseLeave += (sender, e) =>
        {
            MainGrid.Opacity = 1;
        };
        
        MainGrid.MouseLeftButtonDown += (sender, e) =>
        {
            PropertiesService.SetActiveItem(item);
        };
    }
}