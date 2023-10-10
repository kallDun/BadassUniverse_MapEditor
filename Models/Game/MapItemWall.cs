using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BadassUniverse_MapEditor.Models.Game
{
    public class MapItemWall : MapItem
    {
        public int RelatedRoomIndex { get; set; }

        public MapItemWall(int index)
        {
            RelatedRoomIndex = index;
        }

        public override object Clone()
        {
            return new MapItemWall(RelatedRoomIndex);
        }
    }
}
