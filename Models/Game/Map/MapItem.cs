using System;
using System.Windows.Media;

namespace MapEditor.Models.Game
{
    public abstract class MapItem : ICloneable
    {
        public Color Color { get; }
        
        protected MapItem(Color color)
        {
            Color = color;
        }
        
        public abstract object Clone();
    }
}
