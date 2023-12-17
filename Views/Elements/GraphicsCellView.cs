using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using MapEditor.Extensions.Game;
using MapEditor.Models;
using MapEditor.Models.Game;
using MapEditor.Models.Server;
using MapEditor.Services;
using MapEditor.Services.Manager;
using MapEditor.Services.Properties;

namespace MapEditor.Views.Elements;

public class GraphicsCellView : IDisposable
{
    private static LocalStorageService StorageService
        => ServicesManager.Instance.GetService<LocalStorageService>();
    private static PreviewService PreviewService
        => ServicesManager.Instance.GetService<PreviewService>();
    private static PropertiesService PropertiesService
        => ServicesManager.Instance.GetService<PropertiesService>();
    
    private byte Alpha => isPreview ? (byte)125 : (byte)255;
    private Color MainColor => Color.FromArgb(Alpha, 0, 0, 0);
    
    private readonly MapIndex position;
    private readonly int size;
    private bool isPreview;
    private bool backgroundInitialized;
    private int currentFloor;
    private int hashCodeCache;
    
    private Room? room;
    private List<Facade> facades = new();

    private static World World => StorageService.World;
    private MapCell MapCell => World.Map.GetValue(position);
    public Grid Content { get; }
    
    public GraphicsCellView(MapIndex position, int size)
    {
        this.position = position;
        this.size = size;
        Content = new Grid();
        
        // init events
        StorageService.OnWorldChanged += WorldChangedEventHandler;
        StorageService.OnCurrentFloorChanged += WorldChangedEventHandler;

        // init view
        hashCodeCache = MapCell.GetHashCode();
        currentFloor = StorageService.CurrentFloor;
        ClearView();
        InitDefaultBackground();
        InitializeView();
        InitClickEvents();
    }

    public void Dispose()
    {
        StorageService.OnWorldChanged -= WorldChangedEventHandler;
        StorageService.OnCurrentFloorChanged -= WorldChangedEventHandler;
    }
    
    private void WorldChangedEventHandler()
    {
        if (position.Y >= World.Size.Y || position.X >= World.Size.X) return;
        
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
        if (PreviewService.IsPreviewing)
        {
            PreviewService.TryToMoveRoomOrFacade(position);
        
            Point mousePosition = e.GetPosition(Content);
            mousePosition = new Point(mousePosition.X / size, mousePosition.Y / size);
            PreviewService.TryToMoveRoomItem(position, mousePosition, room);
        }
    }

    private void MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        if (PreviewService.IsPreviewing)
        {
            PreviewService.ChangeNotMovingState();
        }
        else
        {
            AItemDTO? selectedItem = null;
            if (facades.Any())
            {
                selectedItem = StorageService.WorldDTO.Facades.FirstOrDefault(x => x.Id == facades.First().Id);
            }
            else if (room != null && selectedItem == null)
            {
                selectedItem = StorageService.WorldDTO.Rooms.FirstOrDefault(x => x.Id == room.Id);
            }
            else if (selectedItem == null)
            {
                selectedItem = StorageService.WorldDTO;
            }
            if (selectedItem != null)
            {
                PropertiesService.SetActiveItem(selectedItem);
            }
        }
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
            this.room = room;
            InitBackground(room.Color);
            return true;
        }
        this.room = null;
        return false;
    }

    private bool TryToInitializeWalls()
    {
        List<MapItemWall> walls = MapCell.GetWalls(currentFloor);
        bool hasAnyWalls = walls.Any();
        if (!hasAnyWalls) return hasAnyWalls;
        
        Dictionary<MapIndex,Color> values = GetValuesForGradientBackground(walls);
        InitGradientBackground(values);
        InitWallBackground();
        return hasAnyWalls;
    }

    private Dictionary<MapIndex, Color> GetValuesForGradientBackground(List<MapItemWall> walls)
    {
        Dictionary<MapIndex, Color> values = new();
        
        foreach (MapItemWall wall in walls)
        {
            Room? room = World.GetRoomFromWall(wall);
            if (room == null) continue;

            foreach (MapIndex index in World.GetClosestNeightborPositionsToRoom(room, position, currentFloor))
            {
                var localPosition = index - position;
                if (values.ContainsKey(localPosition)) continue;
                values.Add(localPosition, room.Color);
            }
        }

        if (!values.ContainsKey(new MapIndex(-1, -1))) values.Add(new MapIndex(-1, -1), Color.FromArgb(0,0,0,0));
        if (!values.ContainsKey(new MapIndex(-1, 0))) values.Add(new MapIndex(-1, 0), Color.FromArgb(0,0,0,0));
        if (!values.ContainsKey(new MapIndex(-1, 1))) values.Add(new MapIndex(-1, 1), Color.FromArgb(0,0,0,0));
        if (!values.ContainsKey(new MapIndex(0, -1))) values.Add(new MapIndex(0, -1), Color.FromArgb(0,0,0,0));
        if (!values.ContainsKey(new MapIndex(0, 1))) values.Add(new MapIndex(0, 1), Color.FromArgb(0,0,0,0));
        if (!values.ContainsKey(new MapIndex(1, -1))) values.Add(new MapIndex(1, -1), Color.FromArgb(0,0,0,0));
        if (!values.ContainsKey(new MapIndex(1, 0))) values.Add(new MapIndex(1, 0), Color.FromArgb(0,0,0,0));
        if (!values.ContainsKey(new MapIndex(1, 1))) values.Add(new MapIndex(1, 1), Color.FromArgb(0,0,0,0));
        
        return values;
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
        this.facades = facadesArray.ToList();
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

    private void InitGradientBackground(Dictionary<MapIndex, Color> values)
    {
        if (values.Count == 0) return;
        if (backgroundInitialized) return;

        byte alpha = (byte)(Alpha / 3);
        
        while (values.Count > 0)
        {
            var index1 = values.First().Key;
            var index2 = GetInvertedIndex(index1);
            Color color1 = values[index1].A == 0 ? values[index1] : Color.FromArgb(alpha, values[index1].R, values[index1].G, values[index1].B);
            Color color2 = values[index2].A == 0 ? values[index2] : Color.FromArgb(alpha, values[index2].R, values[index2].G, values[index2].B);
            values.Remove(index1);
            values.Remove(index2);

            Content.Children.Add(new Rectangle
            {
                Fill = new LinearGradientBrush(
                    color1, 
                    color2, 
                    GetPosition(index1, color1.A == 0), 
                    GetPosition(index2, color2.A == 0))
            });
        }

        backgroundInitialized = true;
        return;

        static Point GetPosition(MapIndex index, bool distanced)
        {
            if (distanced) return new Point(index.X == 0 ? 0.5 : index.X, index.Y == 0 ? 0.5 : index.Y);
            return new Point((index.X + 1) * 0.5, (index.Y + 1) * 0.5);
        }
        
        static MapIndex GetInvertedIndex(MapIndex index)
        {
            var X = index.X < 0 ? index.X + 2 : index.X > 0 ? index.X - 2 : index.X;
            var Y = index.Y < 0 ? index.Y + 2 : index.Y > 0 ? index.Y - 2 : index.Y;
            return new MapIndex(X, Y);
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
