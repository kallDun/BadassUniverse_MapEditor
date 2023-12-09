using System;

namespace BadassUniverse_MapEditor.Models.Game
{
    public abstract class MapItem : ICloneable
    {
        public abstract object Clone();
    }
}
