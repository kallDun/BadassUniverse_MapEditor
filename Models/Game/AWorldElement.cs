using System.Windows.Media;

namespace MapEditor.Models.Game
{
    public abstract class AWorldElement
    {
        protected AWorldElement(int id, string name, MapIndex leftTopCorner, Color color, StoredPreviewState state)
        {
            Id = id;
            Name = name;
            LeftTopCorner = leftTopCorner;
            Color = color;
            State = state;
        }

        public int Id { get; }
        public string Name { get; }
        public MapIndex LeftTopCorner { get; }
        public Color Color { get; }
        public StoredPreviewState State { get; }
    }
}
