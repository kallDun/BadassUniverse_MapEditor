using System.Windows.Media;
using MapEditor.Models.Game.Data;

namespace MapEditor.Models.Game
{
    public class Facade : WorldElement
    {
        private readonly MapDirection rotation;
        private readonly AnyFormBuildingParameters parameters;
        
        public Facade(int id, string name, int floor, Color color, MapDirection rotation, StoredPreviewState state,
            AnyFormBuildingParameters parameters) 
            : base(id, name, floor, color, state)
        {
            this.rotation = rotation;
            this.parameters = parameters;
        }

        protected override Map GenerateMap()
        {
            Map map = Map.InitMap(parameters.Width + 2, parameters.Length + 2);
            foreach (var squareRoom in parameters.SquareRooms)
            {
                map.BuildingInit_FillSquareBuildingSpace(Id, 
                    new MapIndex(0, 0), 
                    new MapIndex(squareRoom.Width + 1, squareRoom.Length + 1),
                    Floor, State);
            }
            map = map.RotateMap(rotation);
            return map;
        }
    }
}
