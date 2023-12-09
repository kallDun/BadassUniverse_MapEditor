using System;

namespace MapEditor.Models.Game
{
    public abstract class MapItem : ICloneable
    {
        public abstract object Clone();
    }
}
