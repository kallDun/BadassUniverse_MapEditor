using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using MapEditor.Extensions.Game;
using MapEditor.Models.Game;

namespace MapEditor.Views.Elements
{
    public partial class GraphicsCell : UserControl
    {
        private readonly World world;
        private readonly MapIndex position;
        private readonly MapCell mapCell;
        private readonly int currentFloor;

        private bool backgroundInitialized = false;

        public GraphicsCell(World world, MapIndex position, MapCell mapCell, int currentFloor)
        {
            InitializeComponent();
            this.world = world;
            this.position = position;
            this.mapCell = mapCell;
            this.currentFloor = currentFloor;

            InitializeView();
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
            if (room == null) return false;
            InitBackground(room.Color);
            return true;
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
            if (hasAnyDoors)
            {
                InitGradientBackground(values);
                InitDoorBackground();
            }
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

        public void InitBackground(Color color)
        {
            if (backgroundInitialized) return;
            BackgroundView.Background = new SolidColorBrush(color);
            BackgroundView.Visibility = Visibility.Visible;
            backgroundInitialized = true;
        }

        public void InitGradientBackground(List<(MapIndex Index, Color Color)> values)
        {
            if (values.Count == 0) return;
            if (backgroundInitialized) return;

            BackgroundGradientView.Children.Clear();
            BackgroundGradientView.Visibility = Visibility.Visible;

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

                BackgroundGradientView.Children.Add(new Rectangle()
                {
                    Fill = new LinearGradientBrush(color1, color2, position1, position2)
                });
            }

            backgroundInitialized = true;

            static Point GetPosition(MapIndex index)
            {
                return new((index.X + 1) * 0.5, (index.Y + 1) * 0.5);
            }

            static Point GetInvertedPosition(Point position)
            {
                var X = position.X < 0.45 ? position.X + 1 : position.X > 0.55 ? position.X - 1 : position.X;
                var Y = position.Y < 0.45 ? position.Y + 1 : position.Y > 0.55 ? position.Y - 1 : position.Y;
                return new(X, Y);
            }
        }

        private void InitWallBackground()
        {
            WallView.Visibility = Visibility.Visible;
        }

        private void InitDoorBackground()
        {
            DoorView.Visibility = Visibility.Visible;
        }

        private void InitFacadeBackground()
        {
            FacadeView.Visibility = Visibility.Visible;
        }

        #endregion
    }
}
