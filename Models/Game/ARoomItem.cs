using System.Windows.Media;

namespace MapEditor.Models.Game;

public abstract class ARoomItem : AWorldElement
{
    public Room RoomOwner { get; }

    protected ARoomItem(int id, string name, MapIndex leftTopCorner, Color color, StoredPreviewState state,
        Room roomOwner)
        : base(id, name, leftTopCorner, color, state)
    {
        RoomOwner = roomOwner;
    }
}