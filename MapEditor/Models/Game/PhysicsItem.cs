using System.Windows.Media;

namespace MapEditor.Models.Game;

public class PhysicsItem : ARoomItem
{
    public PhysicsItem(int id, string name, MapIndex leftTopCorner, Color color, string iconName, StoredPreviewState state, Room roomOwner) 
        : base(id, name, leftTopCorner, color, iconName, state, roomOwner)
    {
        
    }
}