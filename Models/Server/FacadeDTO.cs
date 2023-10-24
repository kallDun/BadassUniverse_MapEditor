using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BadassUniverse_MapEditor.Models.Server
{
    public class FacadeDTO
    {
        public int Id { get; set; }
        public int MapId { get; set; }
        public int InGameFacadeId { get; set; }
        public int MapOffsetX { get; set; }
        public int MapOffsetY { get; set; }

    }
}
