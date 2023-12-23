using System.Windows.Media;

namespace MapEditor.Models.Game
{
    public class MapItemDoor : MapItem
    {
        public int DoorIndex { get; }
        public int RelatedRoomIndex { get; }
        public int RoomFloorDisplacement { get; }
        
        public MapItemDoor(int doorIndex, int relatedRoomIndex, int roomFloorDisplacement, Color color) : base(color)
        {
            DoorIndex = doorIndex;
            RelatedRoomIndex = relatedRoomIndex;
            RoomFloorDisplacement = roomFloorDisplacement;
        }

        public override object Clone()
        {
            return new MapItemDoor(DoorIndex, RelatedRoomIndex, RoomFloorDisplacement, Color);
        }
        
        public override int GetHashCode()
        {
            return 17 
                + DoorIndex.GetHashCode() * 61
                + RelatedRoomIndex.GetHashCode() * 31
                + RoomFloorDisplacement.GetHashCode() * 17
                + Color.GetHashCode() * 13;
        }
    }
}
