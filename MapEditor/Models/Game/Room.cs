using System.Collections.Generic;
using System.Windows.Media;

namespace MapEditor.Models.Game;

public abstract class Room : AWorldElementWithMap
{
    public List<PhysicsItem> PhysicsItems { get; set; } = new();
    public List<Mob> Mobs { get; set; } = new();
        
    protected Room(int id, string name, MapIndex leftTopCorner, int floor, Color color, StoredPreviewState state) 
        : base(id, name, leftTopCorner, floor, color, state) {}
}