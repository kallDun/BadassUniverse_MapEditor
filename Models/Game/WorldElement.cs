using System.Windows.Media;

namespace BadassUniverse_MapEditor.Models.Game
{
    public abstract class WorldElement
    {
        protected WorldElement(int id, string name, int floor, Color color)
        {
            Id = id;
            Name = name;
            Color = color;
            Floor = floor;
            LocalMap = GenerateMap();
        }

        public int Id { get; set; }
        public string Name { get; private set; }
        public Color Color { get; private set; }
        public int Floor { get; private set; }
        public Map LocalMap { get; private set; }

        protected abstract Map GenerateMap();
    }
}
