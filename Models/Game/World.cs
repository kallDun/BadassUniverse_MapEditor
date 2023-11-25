using System;
using System.Collections.Generic;

namespace BadassUniverse_MapEditor.Models.Game
{
    public class World
    {
        public List<Room> Rooms { get; set; }
        public List<PhysicsItem> Items { get; set; }
        public List<Facade> Facades { get; set; }
        public List<Mob> Mobs { get; set; }
        public Map Map { get; set; }
    }
}
