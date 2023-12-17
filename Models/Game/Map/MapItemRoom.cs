using System.Windows.Media;

namespace MapEditor.Models.Game
{
    public class MapItemRoom : MapItem
    {
        public int Index { get; }

        public MapItemRoom(int index, Color color) : base(color)
        {
            Index = index;
        }

        public override object Clone()
        {
            return new MapItemRoom(Index, Color);
        }
        
        public override int GetHashCode()
        {
            return 13 + Index.GetHashCode() + Color.GetHashCode() * 61;
        }
    }
}
