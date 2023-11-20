using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace BadassUniverse_MapEditor.Views
{
    public partial class GraphicsView : UserControl
    {
        Point? lastCenterPositionOnTarget;
        Point? lastMousePositionOnTarget;
        Point? lastDragPoint;
        double zoomValue = 1;
        double zoomMin = 1;
        double zoomMax = 10;
        double zoomDelta = .5f;

        public GraphicsView()
        {
            InitializeComponent();
            InitGrid();

            scrollViewer.ScrollChanged += OnScrollViewerScrollChanged;
            scrollViewer.MouseLeftButtonUp += OnMouseLeftButtonUp;
            scrollViewer.PreviewMouseLeftButtonUp += OnMouseLeftButtonUp;
            scrollViewer.PreviewMouseWheel += OnPreviewMouseWheel;

            scrollViewer.PreviewMouseLeftButtonDown += OnMouseLeftButtonDown;
            scrollViewer.MouseMove += OnMouseMove;
        }

        private void InitGrid()
        {
            int rows = 40;
            int cols = 80;
            int size = 24;
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


        void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (lastDragPoint.HasValue)
            {
                Point posNow = e.GetPosition(scrollViewer);

                double dX = posNow.X - lastDragPoint.Value.X;
                double dY = posNow.Y - lastDragPoint.Value.Y;

                lastDragPoint = posNow;

                scrollViewer.ScrollToHorizontalOffset(scrollViewer.HorizontalOffset - dX);
                scrollViewer.ScrollToVerticalOffset(scrollViewer.VerticalOffset - dY);
            }
        }

        void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var mousePos = e.GetPosition(scrollViewer);
            if (mousePos.X <= scrollViewer.ViewportWidth && mousePos.Y <
                scrollViewer.ViewportHeight) //make sure we still can use the scrollbars
            {
                scrollViewer.Cursor = Cursors.SizeAll;
                lastDragPoint = mousePos;
                Mouse.Capture(scrollViewer);
            }
        }

        void OnPreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            lastMousePositionOnTarget = Mouse.GetPosition(GraphicsGrid);

            if (e.Delta > 0)
            {
                ChangeZoom(zoomValue + zoomDelta);
            }
            if (e.Delta < 0)
            {
                ChangeZoom(zoomValue - zoomDelta);
            }

            e.Handled = true;
        }

        void OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            scrollViewer.Cursor = Cursors.Arrow;
            scrollViewer.ReleaseMouseCapture();
            lastDragPoint = null;
        }

        void ChangeZoom(double value)
        {
            double newValue = Math.Clamp(value, zoomMin, zoomMax);
            if (newValue == zoomValue) return;
            zoomValue = newValue;
            scaleTransform.ScaleX = zoomValue;
            scaleTransform.ScaleY = zoomValue;

            var centerOfViewport = new Point(scrollViewer.ViewportWidth / 2, scrollViewer.ViewportHeight / 2);
            lastCenterPositionOnTarget = scrollViewer.TranslatePoint(centerOfViewport, GraphicsGrid);

            scrollViewer.ScrollToHorizontalOffset(scrollViewer.HorizontalOffset);
            scrollViewer.ScrollToVerticalOffset(scrollViewer.VerticalOffset);
        }

        void OnScrollViewerScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if (e.ExtentHeightChange != 0 || e.ExtentWidthChange != 0)
            {
                Point? targetBefore = null;
                Point? targetNow = null;

                if (!lastMousePositionOnTarget.HasValue)
                {
                    if (lastCenterPositionOnTarget.HasValue)
                    {
                        var centerOfViewport = new Point(scrollViewer.ViewportWidth / 2, scrollViewer.ViewportHeight / 2);
                        Point centerOfTargetNow = scrollViewer.TranslatePoint(centerOfViewport, GraphicsGrid);

                        targetBefore = lastCenterPositionOnTarget;
                        targetNow = centerOfTargetNow;
                    }
                }
                else
                {
                    targetBefore = lastMousePositionOnTarget;
                    targetNow = Mouse.GetPosition(GraphicsGrid);

                    lastMousePositionOnTarget = null;
                }

                if (targetBefore.HasValue)
                {
                    double dXInTargetPixels = targetNow.Value.X - targetBefore.Value.X;
                    double dYInTargetPixels = targetNow.Value.Y - targetBefore.Value.Y;

                    double multiplicatorX = e.ExtentWidth / GraphicsGrid.ActualWidth;
                    double multiplicatorY = e.ExtentHeight / GraphicsGrid.ActualHeight;

                    double newOffsetX = scrollViewer.HorizontalOffset - dXInTargetPixels * multiplicatorX;
                    double newOffsetY = scrollViewer.VerticalOffset - dYInTargetPixels * multiplicatorY;

                    if (double.IsNaN(newOffsetX) || double.IsNaN(newOffsetY)) return;

                    scrollViewer.ScrollToHorizontalOffset(newOffsetX);
                    scrollViewer.ScrollToVerticalOffset(newOffsetY);
                }
            }
        }
    }
}
