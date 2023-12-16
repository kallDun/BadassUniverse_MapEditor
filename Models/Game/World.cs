using System.Collections.Generic;

namespace MapEditor.Models.Game;

public class World
{
    public required string Name { get; set; }
    public List<Room> Rooms { get; } = new();
    public List<Facade> Facades { get; } = new();
    public required Map Map { get; set; }
    public MapIndex Size => new MapIndex(Map.GetSizeY(), Map.GetSizeX());
    
    public IEnumerable<ARoomItem> GetAllRoomItems()
    {
        foreach (Room room in Rooms)
        {
            foreach (PhysicsItem item in room.PhysicsItems)
            {
                yield return item;
            }
            foreach (Mob item in room.Mobs)
            {
                yield return item;
            }
        }
    }
}