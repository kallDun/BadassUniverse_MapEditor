using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using BadassUniverse_MapEditor.Extensions.Game;
using BadassUniverse_MapEditor.Models.Game;

namespace BadassUniverse_MapEditor.Views.Elements;

public class GraphicsCellView
{
    private readonly World world;
    private readonly MapIndex position;
    private readonly MapCell mapCell;
    private readonly int currentFloor;
    private readonly int size;
    private bool backgroundInitialized;
    
    public Grid Content { get; private set; }
    
    public GraphicsCellView(World world, MapIndex position, 
        MapCell mapCell, int currentFloor, int size)
    {
        this.world = world;
        this.position = position;
        this.mapCell = mapCell;
        this.currentFloor = currentFloor;
        this.size = size;
        
        Content = new Grid();
        
        InitializeView();
        InitDefaultBackground();
    }
    
    private void InitializeView()
    {
        bool hasRoom = TryToInitializeRoom();
        bool hasWalls = TryToInitializeWalls();
        bool hasDoors = TryToInitializeDoors();
        bool hasFacade = TryToInitializeFacade();

        if (hasRoom || hasWalls || hasDoors || hasFacade)
        {
            Debug.WriteLine($"Index=({position.Y};{position.X}), Room={hasRoom}, Walls={hasWalls}, Doors={hasDoors}");
        }
    }

    private bool TryToInitializeRoom()
    {
        Room? room = world.GetRoomFromWorldByCell(mapCell, currentFloor);
        if (room != null)
        {
            InitBackground(room.Color);
            return true;
        }
        return false;
    }

    private bool TryToInitializeWalls()
    {
        List<MapItemWall> walls = mapCell.GetWalls(currentFloor);

        List<(MapIndex Index, Color Color)> values = new();
        foreach (MapItemWall wall in walls)
        {
            Room? room = world.GetRoomFromWall(wall);
            if (room == null) continue;
            foreach (MapIndex index in world.GetClosestNeightborPositionsToRoom(room, position, currentFloor))
            {
                values.Add((position - index, room.Color));
            }
        }

        bool hasAnyWalls = walls.Any();
        if (hasAnyWalls)
        {
            InitGradientBackground(values);
            InitWallBackground();
        }
        return hasAnyWalls;
    }

    private bool TryToInitializeDoors()
    {
        List<MapItemDoor> doors = mapCell.GetDoors(currentFloor);

        List<(MapIndex Index, Color Color)> values = new();
        foreach (MapItemDoor door in doors)
        {
            Room? room = world.GetRoomFromDoor(door);
            if (room == null) continue;
            foreach (MapIndex index in world.GetClosestNeightborPositionsToRoom(room, position, currentFloor))
            {
                values.Add((position - index, room.Color));
            }
        }

        bool hasAnyDoors = doors.Any();
        if (!hasAnyDoors) return hasAnyDoors;
        InitGradientBackground(values);
        InitDoorBackground();
        return hasAnyDoors;
    }

    private bool TryToInitializeFacade()
    {
        var facades = world.GetFacadesFromWorldByCell(mapCell, currentFloor);
        var facadesArray = facades as Facade[] ?? facades.ToArray();
        if (!facadesArray.Any()) return false;
        InitFacadeBackground();
        InitBackground(facadesArray.First().Color);
        return true;
    }


    #region View Change
    
    private void InitDefaultBackground()
    {
        Content.Children.Add(new Border()
        {
            BorderBrush = new SolidColorBrush(Colors.Black),
            BorderThickness = new Thickness(0.05)
        });
    }

    private void InitBackground(Color color)
    {
        if (backgroundInitialized) return;
        Content.Background = new SolidColorBrush(color);
        Content.Visibility = Visibility.Visible;
        backgroundInitialized = true;
    }

    private void InitGradientBackground(List<(MapIndex Index, Color Color)> values)
    {
        if (values.Count == 0) return;
        if (backgroundInitialized) return;

        for (int i = 0; i < values.Count; i+=2)
        {
            byte alpha = (byte)(255 / (values.Count / 2 + values.Count % 2));
            Color color1 = Color.FromArgb(alpha, values[i].Color.R, values[i].Color.G, values[i].Color.B);
            Color color2 = i + 1 < values.Count
                ? Color.FromArgb(alpha, values[i + 1].Color.R, values[i + 1].Color.G, values[i + 1].Color.B)
                : Color.FromArgb(0, 0, 0, 0);

            Point position1 = GetPosition(values[i].Index);
            Point position2 = i + 1 < values.Count 
                ? GetPosition(values[i + 1].Index) 
                : GetInvertedPosition(position1);

            Content.Children.Add(new Rectangle
            {
                Fill = new LinearGradientBrush(color1, color2, position1, position2)
            });
        }

        backgroundInitialized = true;
        return;

        static Point GetPosition(MapIndex index)
        {
            return new Point((index.X + 1) * 0.5, (index.Y + 1) * 0.5);
        }

        static Point GetInvertedPosition(Point position)
        {
            var X = position.X < 0.45 ? position.X + 1 : position.X > 0.55 ? position.X - 1 : position.X;
            var Y = position.Y < 0.45 ? position.Y + 1 : position.Y > 0.55 ? position.Y - 1 : position.Y;
            return new Point(X, Y);
        }
    }

    private void InitWallBackground()
    {
        Border border = new Border()
        {
            BorderBrush = new SolidColorBrush(Colors.Black),
            BorderThickness = new Thickness(1)
        };
        Ellipse ellipse = new Ellipse()
        {
            Fill = new SolidColorBrush(Colors.Black),
            Width = 10,
            Height = 10,
            Stroke = new SolidColorBrush(Colors.Black),
            StrokeThickness = 1,
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center
        };
        Content.Children.Add(border);
        Content.Children.Add(ellipse);
    }

    private void InitDoorBackground()
    {
        Border border = new Border()
        {
            BorderBrush = new SolidColorBrush(Colors.Black),
            BorderThickness = new Thickness(1)
        };
        Ellipse ellipse = new Ellipse()
        {
            Width = 22,
            Height = 22,
            Stroke = new SolidColorBrush(Colors.Black),
            StrokeThickness = 2,
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center
        };
        Content.Children.Add(border);
        Content.Children.Add(ellipse);
    }

    private void InitFacadeBackground()
    {
        Border border = new Border()
        {
            BorderBrush = new SolidColorBrush(Colors.Black),
            BorderThickness = new Thickness(1)
        };
        Content.Children.Add(border);

        AddLineToContent(0 * size, 0.6 * size, 0.4 * size, 1 * size);
        AddLineToContent(0 * size, 0.3 * size, 0.7 * size, 1 * size);
        AddLineToContent(0 * size, 0 * size, 1 * size, 1 * size);
        AddLineToContent(0.3 * size, 0 * size, 1 * size, 0.7 * size);
        AddLineToContent(0.6 * size, 0 * size, 1 * size, 0.4 * size);
    }

    private void AddLineToContent(double x1, double y1, double x2, double y2)
    {
        Content.Children.Add(new Line()
        {
            Stroke = new SolidColorBrush(Colors.Black),
            StrokeThickness = 1,
            X1 = x1, Y1 = y1, X2 = x2, Y2 = y2
        });
    }

    #endregion
}