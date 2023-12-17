using System.Windows.Media;

namespace MapEditor.Models.Game
{
    public class MapItemWall : MapItem
    {
        public int RelatedRoomIndex { get; }

        public MapItemWall(int index, Color color) : base(color)
        {
            RelatedRoomIndex = index;
        }

        public override object Clone()
        {
            return new MapItemWall(RelatedRoomIndex, Color);
        }
        
        public override int GetHashCode()
        {
            return 17 + RelatedRoomIndex.GetHashCode() + Color.GetHashCode() * 13;
        }
    }
}
