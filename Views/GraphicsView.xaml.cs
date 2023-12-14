using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
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
        
        private GraphicsCellView[][]? cells;
        private List<RoomItemView> itemViews = new();

        public GraphicsView()
        {
            InitializeComponent();
            scrollViewer.EnableDragZoom(GraphicsGrid, scaleTransform, MouseButton.Middle);
            
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
        }
        
        private void DrawItems()
        {
            List<RoomItemView> newItemViews = new();
            var items = StorageService.World.GetAllRoomItems().Select(RoomItemData.FromARoomItem);

            foreach (var item in items)
            {
                var itemView = itemViews.FirstOrDefault(x => x.Data.Id == item.Id);
                if (itemView == null)
                {
                    itemView = new RoomItemView(cellSize, item);
                    GraphicsGrid.Children.Add(itemView);
                }
                newItemViews.Add(itemView);
            }
            
            var itemsToRemove = itemViews.Except(newItemViews).ToList();
            foreach (var itemView in itemsToRemove)
            {
                GraphicsGrid.Children.Remove(itemView);
            }
            
            itemViews = newItemViews;
        }
    }
}
