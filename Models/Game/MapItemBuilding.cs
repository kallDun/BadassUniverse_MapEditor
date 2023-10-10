using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BadassUniverse_MapEditor.Models.Game
{
    public class MapItemBuilding : MapItem
    {
        public int Index { get; set; }

        public MapItemBuilding(int index)
        {
            Index = index;
        }

        public override object Clone()
        {
            return new MapItemBuilding(Index);
        }
    }
}
