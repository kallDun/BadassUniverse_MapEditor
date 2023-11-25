using BadassUniverse_MapEditor.Models.Game;

namespace BadassUniverse_MapEditor.Models.Server
{
    public class FacadeDTO
    {
        public int Id { get; set; }
        public int MapId { get; set; }
        public int InGameFacadeId { get; set; }
        public int MapOffsetX { get; set; }
        public int MapOffsetY { get; set; }
        public ColorDTO Color { get; set; }
        public int Floor { get; set; }
        public MapDirection Rotation { get; set; }
    }
}
