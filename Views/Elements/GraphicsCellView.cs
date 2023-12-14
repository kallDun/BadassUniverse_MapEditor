using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using MapEditor.Extensions.Game;
using MapEditor.Models;
using MapEditor.Models.Game;
using MapEditor.Services;
using MapEditor.Services.Manager;

namespace MapEditor.Views.Elements;

public class GraphicsCellView
{
    private static LocalStorageService StorageService
        => ServicesManager.Instance.GetService<LocalStorageService>();
    private static PreviewService PreviewService
        => ServicesManager.Instance.GetService<PreviewService>();
    
    private byte Alpha => isPreview ? (byte)150 : (byte)255;
    private Color MainColor => Color.FromArgb(Alpha, 0, 0, 0);
    
    private readonly MapIndex position;
    private readonly int size;
    private bool isPreview;
    private bool backgroundInitialized;
    private int currentFloor;
    private int hashCodeCache;

    private static World World => StorageService.World;
    private MapCell MapCell => World.Map.GetValue(position);
    public Grid Content { get; }
    
    public GraphicsCellView(MapIndex position, int size)
    {
        this.position = position;
        this.size = size;
        Content = new Grid();
        
        // init view
        hashCodeCache = MapCell.GetHashCode();
        currentFloor = StorageService.CurrentFloor;
        ClearView();
        InitDefaultBackground();
        InitializeView();
        InitClickEvents();
        
        // update view
        StorageService.OnWorldChanged += () =>
        {
            if (hashCodeCache != MapCell.GetHashCode() 
                || currentFloor != StorageService.CurrentFloor)
            {
                hashCodeCache = MapCell.GetHashCode();
                currentFloor = StorageService.CurrentFloor;
                ClearView();
                InitDefaultBackground();
                InitializeView();
                InitClickEvents();
            }
        };
    }

    
    #region Click Events
    
    private void InitClickEvents()
    {
        var rect = new Rectangle()
        {
            Width = size,
            Height = size,
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center,
            Fill = new SolidColorBrush(Colors.Transparent)
        };
        Content.Children.Add(rect);
        rect.MouseMove += MouseMove;
        rect.MouseLeftButtonDown += MouseLeftButtonDown;
    }

    private void MouseMove(object sender, MouseEventArgs e)
    {
        PreviewService.TryToMoveRoomOrFacade(position);
    }

    private void MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        PreviewService.TryToSave();
    }
    
    #endregion

    #region Initialiaze View
    
    private void InitializeView()
    {
        isPreview = MapCell.GetState(currentFloor) == StoredPreviewState.Preview;
        bool hasRoom = TryToInitializeRoom();
        bool hasWalls = TryToInitializeWalls();
        bool hasDoors = TryToInitializeDoors();
        bool hasFacade = TryToInitializeFacade();
    }

    private bool TryToInitializeRoom()
    {
        Room? room = World.GetRoomFromWorldByCell(MapCell, currentFloor);
        if (room != null)
        {
            InitBackground(room.Color);
            return true;
        }
        return false;
    }

    private bool TryToInitializeWalls()
    {
        List<MapItemWall> walls = MapCell.GetWalls(currentFloor);

        List<Color> colors = new();
        List<List<MapIndex>> lists = new();
        foreach (MapItemWall wall in walls)
        {
            Room? room = World.GetRoomFromWall(wall);
            if (room == null) continue;

            colors.Add(room.Color);
            lists.Add(new List<MapIndex>());
            foreach (MapIndex index in World.GetClosestNeightborPositionsToRoom(room, position, currentFloor))
            {
                lists.Last().Add(index - position);
            }
        }

        bool hasAnyWalls = walls.Any();
        if (hasAnyWalls)
        {
            List<(bool notNull, MapIndex Index, Color Color)> values = new();
            if (colors.Count == 1)
            {
                foreach (var item in lists[0])
                {
                    values.Add((true, item, colors[0]));
                    values.Add((false, new MapIndex(), new Color()));
                }
            }
            else
            {
                int maxCount = lists.Max(x => x.Count);
                for (int i = 0; i < maxCount; i++)
                {
                    foreach (var list in lists)
                    {
                        if (i < list.Count)
                        {
                            values.Add((true, list[i], colors[lists.IndexOf(list)]));
                        }
                        else
                        {
                            values.Add((false, new MapIndex(), new Color()));
                        }
                    }
                }
            }
            
            
            InitGradientBackground(values);
            InitWallBackground();
        }
        return hasAnyWalls;
    }

    private bool TryToInitializeDoors()
    {
        List<MapItemDoor> doors = MapCell.GetDoors(currentFloor);
        bool hasAnyDoors = doors.Any();
        if (!hasAnyDoors) return hasAnyDoors;
        bool hasDoorRelations = doors.Any(x => World.GetDoorRelation(x, position, currentFloor) != null);
        InitDoorBackground(hasDoorRelations);
        return hasAnyDoors;
    }

    private bool TryToInitializeFacade()
    {
        var facades = World.GetFacadesFromWorldByCell(MapCell, currentFloor);
        var facadesArray = facades as Facade[] ?? facades.ToArray();
        if (!facadesArray.Any()) return false;
        InitFacadeBackground(facadesArray.Select(x => x.Color).ToList());
        return true;
    }

    #endregion

    #region View Change
    
    private void ClearView()
    {
        Content.Children.Clear();
        backgroundInitialized = false;
    }
    
    private void InitDefaultBackground()
    {
        Content.Children.Add(new Border()
        {
            BorderBrush = new SolidColorBrush(MainColor),
            BorderThickness = new Thickness(0.05)
        });
    }

    private void InitBackground(Color color)
    {
        if (backgroundInitialized) return;
        Content.Children.Add(new Rectangle() { Fill = new SolidColorBrush(color) });
        backgroundInitialized = true;
    }

    private void InitGradientBackground(IReadOnlyList<(bool notNull, MapIndex Index, Color Color)> values)
    {
        if (values.Count == 0) return;
        if (backgroundInitialized) return;

        if (position.Y == 32 && position.X == 25)
        {
            var a = 5;
        }

        for (int i = 0; i < values.Count; i+=2)
        {
            byte alpha = (byte)(Alpha / (values.Count / 2 + values.Count % 2));
            Color color1 = Color.FromArgb(alpha, values[i].Color.R, values[i].Color.G, values[i].Color.B);
            Color color2 = i + 1 < values.Count && values[i + 1].notNull
                ? Color.FromArgb(alpha, values[i + 1].Color.R, values[i + 1].Color.G, values[i + 1].Color.B)
                : Color.FromArgb(0, 0, 0, 0);

            Point position1 = GetPosition(values[i].Index);
            Point position2 = i + 1 < values.Count && values[i + 1].notNull
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
        AddBorderToContent();
    }

    private void InitDoorBackground(bool hasDoorRelations)
    {
        AddBorderToContent();
        
        Ellipse ellipseOutside = new Ellipse()
        {
            Width = 22,
            Height = 22,
            Stroke = new SolidColorBrush(MainColor),
            StrokeThickness = 2,
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center
        };
        Content.Children.Add(ellipseOutside);

        if (hasDoorRelations)
        {
            Ellipse ellipseInside = new Ellipse()
            {
                Fill = new SolidColorBrush(MainColor),
                Width = 10,
                Height = 10,
                Stroke = new SolidColorBrush(MainColor),
                StrokeThickness = 1,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };
            Content.Children.Add(ellipseInside);
        }
    }

    private void InitFacadeBackground(IReadOnlyList<Color> colors)
    {
        const int linesCount = 5; // NOT EVEN NUMBER
        
        Color GetColor(int lineIndex)
        {
            int index = lineIndex * colors.Count / linesCount;
            return colors[index];
        }

        int dividedInto = (linesCount - 1) / 2;
        float part = 1f / (dividedInto + 1);
        for (int i = 0; i < dividedInto; i++)
        {
            var currPart = part * (i + 1);
            var currPart2 = 1 - currPart;
            AddLineToContent(0 * size, currPart2 * size, currPart * size, 1 * size, GetColor(i));
        }
        AddLineToContent(0 * size, 0 * size, 1 * size, 1 * size, GetColor(dividedInto));
        for (int i = 0; i < dividedInto; i++)
        {
            var currPart = part * (i + 1);
            var currPart2 = 1 - currPart;
            AddLineToContent(currPart * size, 0 * size, 1 * size, currPart2 * size, GetColor(i + dividedInto + 1));
        }

        AddBorderToContent();
    }

    private void AddBorderToContent()
    {
        Border border = new Border()
        {
            BorderBrush = new SolidColorBrush(MainColor),
            BorderThickness = new Thickness(1),
            Margin = new Thickness(-0.5)
        };
        Content.Children.Add(border);
    }

    private void AddLineToContent(double x1, double y1, double x2, double y2, Color color)
    {
        Content.Children.Add(new Line()
        {
            Stroke = new SolidColorBrush(color),
            StrokeThickness = 1,
            X1 = x1, Y1 = y1, X2 = x2, Y2 = y2
        });
    }

    #endregion
}