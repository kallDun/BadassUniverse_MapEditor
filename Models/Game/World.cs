using System.Collections.Generic;

namespace BadassUniverse_MapEditor.Models.Game
{
    public class World
    {
        public required string Name { get; set; }
        public List<Room> Rooms { get; } = new List<Room>();
        public List<Facade> Facades { get; } = new List<Facade>();
        public List<PhysicsItem> Items { get; } = new List<PhysicsItem>();
        public List<Mob> Mobs { get; } = new List<Mob>(); 
        public required Map Map { get; set; }
    }
}
