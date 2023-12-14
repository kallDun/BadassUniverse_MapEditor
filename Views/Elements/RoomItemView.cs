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

public class RoomItemView : Grid
{
    private static LocalStorageService StorageService
        => ServicesManager.Instance.GetService<LocalStorageService>();
    
    private const int viewSize = 25;
    private Color MainColor => Data.State == StoredPreviewState.Preview 
        ? Color.FromArgb(180, Data.Color.R, Data.Color.G, Data.Color.B)
        : Data.Color;
    
    private readonly int size;
    public RoomItemData Data { get; }
    
    public RoomItemView(int size, RoomItemData data)
    {
        this.size = size;
        Data = data;
        InitializePosition();
        InitializeView();
    }

    private void InitializePosition()
    {
        Room room = GetRoom();
        SetRow(this, room.LeftTopCorner.Y);
        SetColumn(this, room.LeftTopCorner.X);
        Margin = new Thickness(1f * size / Data.LeftTopCorner.X - (viewSize / 2f), 
            1f * size / Data.LeftTopCorner.Y - (viewSize / 2f), 0, 0);
        Width = viewSize;
        Height = viewSize;
    }
    
    private Room GetRoom() => StorageService.World.Rooms.First(x => x.Id == Data.RoomId);

    private void InitializeView()
    {
        Ellipse ellipseOutside = new Ellipse()
        {
            Width = viewSize,
            Height = viewSize,
            Stroke = new SolidColorBrush(MainColor),
            StrokeThickness = 2,
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center
        };
        Children.Add(ellipseOutside);
    }
}