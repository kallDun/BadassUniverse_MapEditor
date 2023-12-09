using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using MapEditor.Extensions.Graphics;
using MapEditor.Models.Game;
using MapEditor.Services;
using MapEditor.Services.Manager;
using MapEditor.Views.Elements;

namespace MapEditor.Views
{
    public partial class GraphicsView : UserControl
    {
        private static LocalStorageService StorageService
            => ServicesManager.Instance.GetService<LocalStorageService>();
        
        private GraphicsCellView[][]? cells;

        public GraphicsView()
        {
            InitializeComponent();
            scrollViewer.EnableDragZoom(GraphicsGrid, scaleTransform, MouseButton.Middle);
            DrawGrid();
        }

        private void DrawGrid()
        {
            GraphicsGrid.Children.Clear();

            const int size = 25;
            Map map = StorageService.World.Map;
            cells = new GraphicsCellView[map.GetSizeY()][];

            for (int i = 0; i < map.GetSizeY(); i++)
            {
                cells[i] = new GraphicsCellView[map.GetSizeX()];
                RowDefinition row = new()
                {
                    Height = new GridLength(size)
                };
                GraphicsGrid.RowDefinitions.Add(row);
            }
            for (int i = 0; i < map.GetSizeX(); i++)
            {
                ColumnDefinition col = new()
                {
                    Width = new GridLength(size)
                };
                GraphicsGrid.ColumnDefinitions.Add(col);
            }

            for (int y = 0; y < map.GetSizeY(); y++)
            {
                for (int x = 0; x < map.GetSizeX(); x++)
                {
                    MapIndex index = new(y, x);
                    GraphicsCellView cell = new(index, size);
                    cells[y][x] = cell;
                    Grid.SetRow(cell.Content, y);
                    Grid.SetColumn(cell.Content, x);
                    GraphicsGrid.Children.Add(cell.Content);
                }
            }
        }
    }
}
