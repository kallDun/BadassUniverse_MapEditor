namespace MapEditor.Models.Game
{
    public class MapItemDoor : MapItem
    {
        public int DoorIndex { get; set; }
        public int RelatedRoomIndex { get; set; }
        public int RoomFloorDisplacement { get; set; }
        
        public MapItemDoor(int doorIndex, int relatedRoomIndex, int roomFloorDisplacement)
        {
            DoorIndex = doorIndex;
            RelatedRoomIndex = relatedRoomIndex;
            RoomFloorDisplacement = roomFloorDisplacement;
        }

        public override object Clone()
        {
            return new MapItemDoor(DoorIndex, RelatedRoomIndex, RoomFloorDisplacement);
        }
        
        public override int GetHashCode()
        {
            return 17 
                + DoorIndex.GetHashCode() * 61
                + RelatedRoomIndex.GetHashCode() * 31
                + RoomFloorDisplacement.GetHashCode() * 17;
        }
    }
}
