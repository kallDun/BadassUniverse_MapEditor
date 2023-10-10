using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BadassUniverse_MapEditor.Models.Game
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
    }
}
