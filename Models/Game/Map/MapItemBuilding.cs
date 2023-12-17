using System.Windows.Media;

namespace MapEditor.Models.Game
{
    public class MapItemBuilding : MapItem
    {
        public int Index { get; }

        public MapItemBuilding(int index, Color color) : base(color)
        {
            Index = index;
        }

        public override object Clone()
        {
            return new MapItemBuilding(Index, Color);
        }
        
        public override int GetHashCode()
        {
            return 13 + Index.GetHashCode() + Color.GetHashCode() * 31;
        }
    }
}
