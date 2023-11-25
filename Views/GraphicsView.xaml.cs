using BadassUniverse_MapEditor.Extensions.Graphics;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace BadassUniverse_MapEditor.Views
{
    public partial class GraphicsView : UserControl
    {
        public GraphicsView()
        {
            InitializeComponent();
            InitGrid();
            scrollViewer.EnableDragZoom(GraphicsGrid, scaleTransform, MouseButton.Middle);
        }

        private void InitGrid()
        {
            int rows = 40;
            int cols = 80;
            int size = 25;
            for (int i = 0; i < rows; i++)
            {
                RowDefinition row = new RowDefinition();
                row.Height = new GridLength(size);
                GraphicsGrid.RowDefinitions.Add(row);
            }
            for (int i = 0; i < cols; i++)
            {
                ColumnDefinition col = new ColumnDefinition();
                col.Width = new GridLength(size);
                GraphicsGrid.ColumnDefinitions.Add(col);
            }
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    Rectangle rect = new Rectangle();
                    //rect.Fill = Brushes.DarkGray;


                    // random color
                    Random rnd = new();
                    byte[] bytes = new byte[3];
                    rnd.NextBytes(bytes);
                    Color randomColor = Color.FromRgb(bytes[0], bytes[1], bytes[2]);
                    rect.Fill = new SolidColorBrush(randomColor);


                    rect.Stroke = Brushes.White;
                    rect.StrokeThickness = 1;
                    rect.Width = size;
                    rect.Height = size;
                    Grid.SetRow(rect, i);
                    Grid.SetColumn(rect, j);
                    GraphicsGrid.Children.Add(rect);
                }
            }
        }


        
    }
}
