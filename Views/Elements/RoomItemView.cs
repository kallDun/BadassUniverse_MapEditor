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
    
    private Color MainColor => Data.State == StoredPreviewState.Preview 
        ? Color.FromArgb(180, Data.Color.R, Data.Color.G, Data.Color.B)
        : Color.FromArgb(255, Data.Color.R, Data.Color.G, Data.Color.B);
    
    private const int pointsInCell = 100;
    private readonly int cellSize;
    private readonly int viewSize;
    private readonly Grid grid;
    private UIElement? view;
    public RoomItemData Data { get; }

    public RoomItemView(RoomItemData data, Grid itemsGrid, int cellSize)
    {
        this.cellSize = cellSize;
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
            Width = viewSize * 0.75,
            Height = viewSize * 0.75,
            IsHitTestVisible = false
        };
        content.Children.Add(image);
        Ellipse ellipse = new Ellipse()
        {
            Stroke = new SolidColorBrush(MainColor),
            StrokeThickness = 1,
        };
        content.Children.Add(ellipse);
        
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