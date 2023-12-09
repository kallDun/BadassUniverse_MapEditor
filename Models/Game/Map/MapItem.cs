using System;

namespace BadassUniverse_MapEditor.Models.Game
{
    public abstract class MapItem : ICloneable
    {
        public StoredPreviewState State { get; set; } = StoredPreviewState.Stored;
        
        public abstract object Clone();

        public override int GetHashCode()
        {
            return State.GetHashCode() * 31;
        }
    }
}
