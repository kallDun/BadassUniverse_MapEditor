using System.Windows.Media;

namespace MapEditor.Models.Game
{
    public abstract class WorldElement
    {
        protected WorldElement(int id, string name, int floor, Color color, StoredPreviewState state)
        {
            Id = id;
            Name = name;
            Color = color;
            Floor = floor;
            State = state;
        }

        public int Id { get; set; }
        public string Name { get; private set; }
        public Color Color { get; private set; }
        public int Floor { get; private set; }
        public StoredPreviewState State { get; set; }

        private Map? map;
        public Map LocalMap => map ??= GenerateMap();
        protected abstract Map GenerateMap();
    }
}
