using System.Windows.Media;

namespace MapEditor.Models.Game;

public abstract class ARoomItem : AWorldElement
{
    public Room RoomOwner { get; }

    protected ARoomItem(int id, string name, MapIndex leftTopCorner, int floor, Color color, StoredPreviewState state,
        Room roomOwner)
        : base(id, name, leftTopCorner, floor, color, state)
    {
        RoomOwner = roomOwner;
    }
}