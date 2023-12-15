using System;
using System.Windows.Media;

namespace MapEditor.Models.Game.Data;

public readonly struct RoomItemData : IEquatable<RoomItemData>
{
    public int Id { get; }
    public string Name { get; }
    public MapIndex LeftTopCorner { get; }
    public Color Color { get; }
    public StoredPreviewState State { get; }
    public int RoomId { get; }
    public int RoomHashCode { get; }
    
    public RoomItemData(int id, string name, MapIndex leftTopCorner, Color color, StoredPreviewState state, int roomId, int roomHashCode)
    {
        Id = id;
        Name = name;
        LeftTopCorner = leftTopCorner;
        Color = color;
        State = state;
        RoomId = roomId;
        RoomHashCode = roomHashCode;
    }
    
    public static RoomItemData FromARoomItem(ARoomItem roomItem)
    {
        return new RoomItemData(roomItem.Id, roomItem.Name, roomItem.LeftTopCorner, 
            roomItem.Color, roomItem.State, roomItem.RoomOwner.Id, roomItem.GetHashCode());
    }
    
    public bool Equals(RoomItemData other)
    {
        return Id == other.Id && Name == other.Name && LeftTopCorner.Equals(other.LeftTopCorner) 
               && Color.Equals(other.Color) && State.Equals(other.State) 
               && RoomId == other.RoomId && RoomHashCode == other.RoomHashCode;
    }

    public override bool Equals(object? obj)
    {
        return obj is RoomItemData other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Id, Name, LeftTopCorner, Color, State, RoomId, RoomHashCode);
    }

    public static bool operator ==(RoomItemData left, RoomItemData right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(RoomItemData left, RoomItemData right)
    {
        return !(left == right);
    }
}