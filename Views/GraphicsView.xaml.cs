using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using MapEditor.Extensions.Graphics;
using MapEditor.Models.Game;
using MapEditor.Models.Game.Data;
using MapEditor.Services;
using MapEditor.Services.Manager;
using MapEditor.Views.Elements;

namespace MapEditor.Views
{
    public partial class GraphicsView : UserControl
    {
        private static LocalStorageService StorageService
            => ServicesManager.Instance.GetService<LocalStorageService>();
        
        private const int cellSize = 25;
        private readonly ScrollViewerDragZoomController controller;
        
        private GraphicsCellView[][]? cells;
        private Grid? itemsContent;
        private List<RoomItemView> itemViews = new();

        public GraphicsView()
        {
            InitializeComponent();
            controller = scrollViewer.EnableDragZoom(GraphicsGrid, scaleTransform, MouseButton.Middle);
            
            ClearGrid();
            DrawGrid();
            DrawItems();
            StorageService.OnWorldChanged += DrawItems;
        }

        private void ClearGrid()
        {
            GraphicsGrid.Children.Clear();
            GraphicsGrid.RowDefinitions.Clear();
            GraphicsGrid.ColumnDefinitions.Clear();
            itemViews.Clear();
            itemsContent?.Children.Clear();
            cells = null;
        }

        private void DrawGrid()
        {
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
            var itemsData = StorageService.World.GetAllRoomItems().Select(RoomItemData.FromARoomItem);

            List<RoomItemView> newItemViews = itemsData
                .Select(data => itemViews
                                    .FirstOrDefault(x => x.Data.Equals(data)) 
                                ?? new RoomItemView(data, itemsContent, cellSize)).ToList();

            foreach (var oldView in itemViews
                         .Where(oldView => newItemViews
                         .All(x => x.Data != oldView.Data)))
            {
                oldView.RemoveFromField();
            }
            
            itemViews = newItemViews;
        }
    }
}
