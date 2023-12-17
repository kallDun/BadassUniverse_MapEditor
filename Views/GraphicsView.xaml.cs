using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using MapEditor.Extensions.Graphics;
using MapEditor.Models.Game;
using MapEditor.Models.Game.Data;
using MapEditor.Models.Server;
using MapEditor.Services;
using MapEditor.Services.Manager;
using MapEditor.Services.Properties;
using MapEditor.Views.Elements;

namespace MapEditor.Views
{
    public partial class GraphicsView : UserControl
    {
        private static LocalStorageService StorageService
            => ServicesManager.Instance.GetService<LocalStorageService>();
        private static PropertiesService PropertiesService
            => ServicesManager.Instance.GetService<PropertiesService>();
        private static PreviewService PreviewService
            => ServicesManager.Instance.GetService<PreviewService>();
        
        private const int cellSize = 25;
        private readonly ScrollViewerDragZoomController controller;
        
        private GraphicsCellView[][]? cells;
        private MapIndex worldSize = new();
        private Grid? itemsContent;
        private List<RoomItemView> itemViews = new();
        private Action<RoomItemData>? onClickRoomItem;

        public GraphicsView()
        {
            InitializeComponent();
            controller = scrollViewer.EnableDragZoom(GraphicsGrid, scaleTransform, MouseButton.Middle);
            
            ClearGrid();
            DrawGrid();
            DrawItems();
            
            StorageService.OnWorldChanged += () =>
            {
                var currentWorldSize = new MapIndex(
                    StorageService.World.Map.GetSizeY(), 
                    StorageService.World.Map.GetSizeX());
                if (currentWorldSize != worldSize)
                {
                    ClearGrid();
                    DrawGrid();
                }
                DrawItems();
            };
            StorageService.OnCurrentFloorChanged += DrawItems;
            
            SubscribeOnClickRoomItem();
            ChangeFloorUIInit();
        }

        private void ClearGrid()
        {
            GraphicsGrid.Children.Clear();
            GraphicsGrid.RowDefinitions.Clear();
            GraphicsGrid.ColumnDefinitions.Clear();
            itemViews.Clear();
            itemsContent?.Children.Clear();
            worldSize = new MapIndex();

            if (cells != null)
            {
                foreach (var cellArray in cells)
                {
                    foreach (var cell in cellArray)
                    {
                        cell.Dispose();
                    }
                }
                cells = null;
            }
        }

        private void DrawGrid()
        {
            worldSize = StorageService.World.Size;
            Map map = StorageService.World.Map;
            cells = new GraphicsCellView[map.GetSizeY()][];

            for (int i = 0; i < map.GetSizeY(); i++)
            {
                cells[i] = new GraphicsCellView[map.GetSizeX()];
                RowDefinition row = new()
                {
                    Height = new GridLength(cellSize)
                };
                GraphicsGrid.RowDefinitions.Add(row);
            }
            for (int i = 0; i < map.GetSizeX(); i++)
            {
                ColumnDefinition col = new()
                {
                    Width = new GridLength(cellSize)
                };
                GraphicsGrid.ColumnDefinitions.Add(col);
            }

            for (int y = 0; y < map.GetSizeY(); y++)
            {
                for (int x = 0; x < map.GetSizeX(); x++)
                {
                    MapIndex index = new(y, x);
                    GraphicsCellView cell = new(index, cellSize);
                    cells[y][x] = cell;
                    Grid.SetRow(cell.Content, y);
                    Grid.SetColumn(cell.Content, x);
                    GraphicsGrid.Children.Add(cell.Content);
                }
            }
            
            itemsContent = new Grid();
            Grid.SetColumnSpan(itemsContent, map.GetSizeX());
            Grid.SetRowSpan(itemsContent, map.GetSizeY());
            GraphicsGrid.Children.Add(itemsContent);
        }
        
        private void DrawItems()
        {
            if (itemsContent == null) return;
            var itemsData = StorageService.World.GetAllRoomItems()
                .Select(RoomItemData.FromARoomItem)
                .Where(x => x.Floor == StorageService.CurrentFloor);
            
            List<RoomItemView> newItemViews = itemsData
                .Select(data => itemViews
                                    .FirstOrDefault(x => x.Data.Equals(data)) 
                                ?? new RoomItemView(data, itemsContent, cellSize, onClickRoomItem))
                .ToList();

            foreach (var oldView in itemViews
                         .Where(oldView => newItemViews
                         .All(x => x.Data != oldView.Data)))
            {
                oldView.RemoveFromField();
            }
            
            itemViews = newItemViews;
        }
        
        private void SubscribeOnClickRoomItem()
        {
            onClickRoomItem = data =>
            {
                if (PreviewService.IsPreviewing) return;
                
                var room = StorageService.WorldDTO.Rooms.FirstOrDefault(x => x.Id == data.RoomId);
                if (room == null) return;
                
                AItemDTO? selectedItem = null;
                switch (data.Type)
                {
                    case RoomItemDataType.PhysicsItem:
                    {
                        var item = room.PhysicsItems.FirstOrDefault(x => x.Id == data.Id);
                        if (item == null) return;
                        selectedItem = item;
                        break;
                    }
                    case RoomItemDataType.Mob:
                    {
                        var item = room.Mobs.FirstOrDefault(x => x.Id == data.Id);
                        if (item == null) return;
                        selectedItem = item;
                        break;
                    }
                    default:
                        return;
                }
                
                PropertiesService.SetActiveItem(selectedItem);
            };
        }
        
        private void ChangeFloorUIInit()
        {
            FloorUpButton.Click += (sender, args) =>
            {
                int floor = StorageService.CurrentFloor;
                StorageService.SetCurrentFloor(floor + 1);
            };
            FloorDownButton.Click += (sender, args) =>
            {
                int floor = StorageService.CurrentFloor;
                StorageService.SetCurrentFloor(floor - 1);
            };
            StorageService.OnCurrentFloorChanged += () =>
            {
                int floor = StorageService.CurrentFloor;
                FloorNumberTextBlock.Text = floor.ToString();
            };
        }
    }
}
