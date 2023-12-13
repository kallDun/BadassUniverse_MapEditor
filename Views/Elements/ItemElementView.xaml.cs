﻿using System;
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
                ItemType.Room => "Room.png",
                ItemType.Building => "Facade.png",
                ItemType.Mob => "Mob.png",
                ItemType.PhysicsItem => "Item.png",
                _ => "Room.png"
            };
            ItemIcon.Source = new BitmapImage(new Uri($"pack://application:,,,/Assets/Icons/{imageName}"));
            
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
