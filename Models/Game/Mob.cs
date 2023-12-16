using System.Windows.Media;

namespace MapEditor.Models.Game;

public class Mob : ARoomItem
{
    public Mob(int id, string name, MapIndex leftTopCorner, Color color, string iconName, StoredPreviewState state, Room roomOwner) 
        : base(id, name, leftTopCorner, color, iconName, state, roomOwner) {}
}