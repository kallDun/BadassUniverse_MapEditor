using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace BadassUniverse_MapEditor.Extensions.Graphics
{
    public class ScrollViewerDragZoomController
    {
        private readonly ScrollViewer scrollViewer;
        private readonly FrameworkElement content;
        private readonly ScaleTransform scaleTransform;
        private readonly MouseButton dragMouseButton;

        private Point? lastCenterPositionOnTarget;
        private Point? lastMousePositionOnTarget;
        private Point? lastDragPoint;
        private double zoomValue = 1;
        private const double zoomMin = 1;
        private const double zoomMax = 10;
        private const double zoomDelta = .5f;
        
        public ScrollViewerDragZoomController(ScrollViewer scrollViewer, 
            FrameworkElement content, ScaleTransform scaleTransform, MouseButton dragMouseButton)
        {
            this.scrollViewer = scrollViewer;
            this.content = content;
            this.scaleTransform = scaleTransform;
            this.dragMouseButton = dragMouseButton;

            scrollViewer.ScrollChanged += OnScrollViewerScrollChanged;
            scrollViewer.PreviewMouseWheel += OnPreviewMouseWheel;
            scrollViewer.PreviewMouseUp += OnMouseButtonUp;
            scrollViewer.MouseUp += OnMouseButtonUp;
            scrollViewer.PreviewMouseDown += OnMouseButtonDown;
            scrollViewer.MouseDown += OnMouseButtonDown;
            scrollViewer.MouseMove += OnMouseMove;
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
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

        private void OnMouseButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == dragMouseButton && e.ButtonState == MouseButtonState.Pressed)
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
        }

        private void OnMouseButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == dragMouseButton && e.ButtonState == MouseButtonState.Released)
            {
                scrollViewer.Cursor = Cursors.Arrow;
                scrollViewer.ReleaseMouseCapture();
                lastDragPoint = null;
            }
        }

        private void OnPreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            lastMousePositionOnTarget = Mouse.GetPosition(content);

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

        private void ChangeZoom(double value)
        {
            double newValue = Math.Clamp(value, zoomMin, zoomMax);
            if (newValue.Equals(zoomValue)) return;
            zoomValue = newValue;
            scaleTransform.ScaleX = zoomValue;
            scaleTransform.ScaleY = zoomValue;

            var centerOfViewport = new Point(scrollViewer.ViewportWidth / 2, scrollViewer.ViewportHeight / 2);
            lastCenterPositionOnTarget = scrollViewer.TranslatePoint(centerOfViewport, content);

            scrollViewer.ScrollToHorizontalOffset(scrollViewer.HorizontalOffset);
            scrollViewer.ScrollToVerticalOffset(scrollViewer.VerticalOffset);
        }

        private void OnScrollViewerScrollChanged(object sender, ScrollChangedEventArgs e)
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
                        Point centerOfTargetNow = scrollViewer.TranslatePoint(centerOfViewport, content);

                        targetBefore = lastCenterPositionOnTarget;
                        targetNow = centerOfTargetNow;
                    }
                }
                else
                {
                    targetBefore = lastMousePositionOnTarget;
                    targetNow = Mouse.GetPosition(content);

                    lastMousePositionOnTarget = null;
                }

                if (targetBefore.HasValue && targetNow.HasValue)
                {
                    double dXInTargetPixels = targetNow.Value.X - targetBefore.Value.X;
                    double dYInTargetPixels = targetNow.Value.Y - targetBefore.Value.Y;

                    double multiplicationX = e.ExtentWidth / content.ActualWidth;
                    double multiplicationY = e.ExtentHeight / content.ActualHeight;

                    double newOffsetX = scrollViewer.HorizontalOffset - dXInTargetPixels * multiplicationX;
                    double newOffsetY = scrollViewer.VerticalOffset - dYInTargetPixels * multiplicationY;

                    if (double.IsNaN(newOffsetX) || double.IsNaN(newOffsetY)) return;

                    scrollViewer.ScrollToHorizontalOffset(newOffsetX);
                    scrollViewer.ScrollToVerticalOffset(newOffsetY);
                }
            }
        }
    }

    public static class ScrollViewerDragZoomControllerExtensions
    {
        public static ScrollViewerDragZoomController EnableDragZoom(this ScrollViewer scrollViewer, 
            FrameworkElement content, ScaleTransform contentScaleTransform, MouseButton dragMouseButton)
        {
            return new ScrollViewerDragZoomController(scrollViewer, content, contentScaleTransform, dragMouseButton);
        }
    }
}
