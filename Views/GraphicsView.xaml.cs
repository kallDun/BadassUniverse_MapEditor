using BadassUniverse_MapEditor.Extensions.Graphics;
using BadassUniverse_MapEditor.Models.Game;
using BadassUniverse_MapEditor.Services;
using BadassUniverse_MapEditor.Services.Manager;
using BadassUniverse_MapEditor.Views.Graphics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace BadassUniverse_MapEditor.Views
{
    public partial class GraphicsView : UserControl
    {
        private static LocalStorageService StorageService
            => ServicesManager.Instance.GetService<LocalStorageService>();

        public GraphicsView()
        {
            InitializeComponent();
            scrollViewer.EnableDragZoom(GraphicsGrid, scaleTransform, MouseButton.Middle);
            StorageService.OnWorldChanged += RedrawGrid;
            RedrawGrid();
        }

        private void RedrawGrid()
        {
            GraphicsGrid.Children.Clear();

            int size = 25;
            int drawingFloor = 0;
            World world = StorageService.World;
            Map map = world.Map;

            for (int i = 0; i < map.GetSizeY(); i++)
            {
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
                    Frame cell = new()
                    {
                        Content = new GraphicsCell(world, index, map.GetValue(index), drawingFloor)
                    };
                    Grid.SetRow(cell, y);
                    Grid.SetColumn(cell, x);
                    GraphicsGrid.Children.Add(cell);
                }
            }
        }
    }
}
