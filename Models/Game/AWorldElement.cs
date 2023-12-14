using System.Windows.Media;

namespace MapEditor.Models.Game
{
    public abstract class AWorldElement
    {
        protected AWorldElement(int id, string name, MapIndex leftTopCorner,
            int floor, Color color, StoredPreviewState state)
        {
            Id = id;
            Name = name;
            LeftTopCorner = leftTopCorner;
            Color = color;
            Floor = floor;
            State = state;
        }

        public int Id { get; }
        public string Name { get; }
        public MapIndex LeftTopCorner { get; }
        public Color Color { get; }
        public int Floor { get; }
        public StoredPreviewState State { get; }
    }
}
