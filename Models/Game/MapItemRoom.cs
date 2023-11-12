﻿namespace BadassUniverse_MapEditor.Models.Game
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
