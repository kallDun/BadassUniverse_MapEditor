using System.Windows.Media;

namespace MapEditor.Models.Game;

public abstract class ARoomItem : AWorldElement
{
    public Room RoomOwner { get; }
    
    public string IconName { get; }

    protected ARoomItem(int id, string name, MapIndex leftTopCorner, Color color, string iconName, StoredPreviewState state,
        Room roomOwner)
        : base(id, name, leftTopCorner, color, state)
    {
        IconName = iconName;
        RoomOwner = roomOwner;
    }
}