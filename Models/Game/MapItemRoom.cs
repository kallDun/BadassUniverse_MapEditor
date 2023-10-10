using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BadassUniverse_MapEditor.Models.Game
{
    public class MapItemRoom : MapItem
    {
        public int Index { get; set; }

        public MapItemRoom(int index)
        {
            Index = index;
        }

        public override object Clone()
        {
            return new MapItemRoom(Index);
        }
    }
}
