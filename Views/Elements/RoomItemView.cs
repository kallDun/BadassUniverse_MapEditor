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
    
    private const int viewSize = 25;
    private Color MainColor => Data.State == StoredPreviewState.Preview 
        ? Color.FromArgb(180, Data.Color.R, Data.Color.G, Data.Color.B)
        : Data.Color;
    
    private const int pointsInCell = 100;
    private readonly int cellSize;
    private readonly Grid grid;
    private UIElement? view;
    public RoomItemData Data { get; }

    public RoomItemView(RoomItemData data, Grid itemsGrid, int cellSize)
    {
        this.cellSize = cellSize;
        grid = itemsGrid;
        Data = data;
        InitializeView();
    }
    
    private Room GetRoom() => StorageService.World.Rooms.First(x => x.Id == Data.RoomId);

    private void InitializeView()
    {
        Room room = GetRoom();
        Ellipse ellipseOutside = new Ellipse()
        {
            Width = viewSize,
            Height = viewSize,
            Stroke = new SolidColorBrush(MainColor),
            StrokeThickness = 2,
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment = VerticalAlignment.Top,
            Margin = new Thickness(
                room.LeftTopCorner.X * cellSize + 1f * Data.LeftTopCorner.X / pointsInCell * cellSize - (viewSize / 2f),
                room.LeftTopCorner.Y * cellSize +1f * Data.LeftTopCorner.Y / pointsInCell * cellSize - (viewSize / 2f), 
                0, 
                0)
        };
        grid.Children.Add(ellipseOutside);
        view = ellipseOutside;
    }

    public void RemoveFromField()
    {
        if (view == null) return;
        grid.Children.Remove(view);
        view = null;
    }
}