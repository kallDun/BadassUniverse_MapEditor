using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using MapEditor.Models;
using MapEditor.Models.Game;
using MapEditor.Models.Game.Data;
using MapEditor.Services;
using MapEditor.Services.Manager;

namespace MapEditor.Views.Elements;

public class RoomItemView
{
    private static LocalStorageService StorageService
        => ServicesManager.Instance.GetService<LocalStorageService>();
    private static PreviewService PreviewService
        => ServicesManager.Instance.GetService<PreviewService>();

    private byte Alpha => Data.State == StoredPreviewState.Preview ? (byte)125 : (byte)255;
    private Color MainColor => Color.FromArgb(Alpha, Data.Color.R, Data.Color.G, Data.Color.B);
    
    private const int pointsInCell = 100;
    private readonly int cellSize;
    private readonly int viewSize;
    private readonly Grid grid;
    private readonly Action<RoomItemData>? onClick;
    private UIElement? view;
    public RoomItemData Data { get; }

    public RoomItemView(RoomItemData data, Grid itemsGrid, int cellSize, Action<RoomItemData>? onClick)
    {
        this.cellSize = cellSize;
        this.onClick = onClick;
        viewSize = this.cellSize;
        grid = itemsGrid;
        Data = data;
        InitializeView();
    }

    private void InitializeView()
    {
        Room room = StorageService.World.Rooms.First(x => x.Id == Data.RoomId);
        Grid content = new Grid
        {
            Width = viewSize,
            Height = viewSize,
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment = VerticalAlignment.Top,
            Margin = new Thickness(
                room.LeftTopCorner.X * cellSize + 1f * Data.LeftTopCorner.X / pointsInCell * cellSize - (viewSize / 2f),
                room.LeftTopCorner.Y * cellSize + 1f * Data.LeftTopCorner.Y / pointsInCell * cellSize - (viewSize / 2f), 
                0, 
                0)
        };
        
        Image image = new Image()
        {
            Source = ImagesStorage.GetImage(Data.IconName, "Icons/RoomItems"),
            Width = viewSize * 0.7,
            Height = viewSize * 0.7,
            Opacity = Alpha / 255f,
            IsHitTestVisible = false
        };
        content.Children.Add(image);
        Ellipse ellipse = new Ellipse()
        {
            Stroke = new SolidColorBrush(MainColor),
            StrokeThickness = 1,
        };
        content.Children.Add(ellipse);
        if (Data.State == StoredPreviewState.Stored)
        {
            var clickEllipse = new Ellipse()
            {
                Width = viewSize * 0.6,
                Height = viewSize * 0.6,
                Fill = new SolidColorBrush(Colors.Transparent),
                IsHitTestVisible = true
            };
            clickEllipse.Fill = new SolidColorBrush(Colors.Transparent);
            clickEllipse.MouseEnter += (sender, args) =>
            {
                if (!PreviewService.IsPreviewing)
                {
                    content.Opacity = 0.75;
                }
            };
            clickEllipse.MouseLeave += (sender, args) =>
            {
                if (!PreviewService.IsPreviewing)
                {
                    content.Opacity = 1;
                }
            };
            clickEllipse.MouseLeftButtonDown += (sender, args) => onClick?.Invoke(Data);
            content.Children.Add(clickEllipse);
        }
        
        view = content;
        grid.Children.Add(view);
    }

    public void RemoveFromField()
    {
        if (view == null) return;
        grid.Children.Remove(view);
        view = null;
    }
}