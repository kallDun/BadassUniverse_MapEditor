using System.Windows.Media;

namespace MapEditor.Models.Game;

public class PhysicsItem : ARoomItem
{
    public PhysicsItem(int id, string name, MapIndex leftTopCorner, Color color, StoredPreviewState state, Room roomOwner) 
        : base(id, name, leftTopCorner, color, state, roomOwner)
    {
        
    }
}