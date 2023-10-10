using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BadassUniverse_MapEditor.Models.Game
{
    public abstract class MapItem : ICloneable
    {
        public abstract object Clone();
    }
}
