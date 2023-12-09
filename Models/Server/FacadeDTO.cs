using BadassUniverse_MapEditor.Models.Game;
using Newtonsoft.Json;

namespace BadassUniverse_MapEditor.Models.Server
{
    public class FacadeDTO : AItemDTO
    {
        public int Id { get; set; }
        public int MapId { get; set; }
        public int InGameFacadeId { get; set; }
        public required string? Name { get; set; }
        public int MapOffsetX { get; set; }
        public int MapOffsetY { get; set; }
        public ColorDTO Color { get; set; }
        public int Floor { get; set; }
        public MapDirection Rotation { get; set; }
        
        public override object Clone()
        {
            return new FacadeDTO
            {
                Id = Id,
                MapId = MapId,
                InGameFacadeId = InGameFacadeId,
                Name = Name,
                MapOffsetX = MapOffsetX,
                MapOffsetY = MapOffsetY,
                Color = Color,
                Floor = Floor,
                Rotation = Rotation,
                State = State
            };
        }
    }
}
