using System;
using System.Windows.Media;

namespace MapEditor.Models.Game.Data;

public enum RoomItemDataType
{
    PhysicsItem,
    Mob
}

public struct RoomItemData : IEquatable<RoomItemData>
{
    public int Id { get; }
    public string Name { get; }
    public MapIndex LeftTopCorner { get; }
    public Color Color { get; }
    public string IconName { get; }
    public StoredPreviewState State { get; }
    public RoomItemDataType Type { get; }
    public int RoomId { get; }
    public int Floor { get; }
    public int RoomHashCode { get; }
    
    public RoomItemData(int id, string name, MapIndex leftTopCorner, Color color, string iconName, 
        StoredPreviewState state, RoomItemDataType type, int roomId, int floor, int roomHashCode)
    {
        Id = id;
        Name = name;
        LeftTopCorner = leftTopCorner;
        Color = color;
        IconName = iconName;
        State = state;
        Type = type;
        RoomId = roomId;
        Floor = floor;
        RoomHashCode = roomHashCode;
    }
    
    public static RoomItemData FromARoomItem(ARoomItem roomItem)
    {
        RoomItemDataType type = roomItem is PhysicsItem ? RoomItemDataType.PhysicsItem : RoomItemDataType.Mob;
        return new RoomItemData(roomItem.Id, roomItem.Name, roomItem.LeftTopCorner, 
            roomItem.Color, roomItem.IconName, roomItem.State, type, 
            roomItem.RoomOwner.Id, roomItem.RoomOwner.Floor, roomItem.GetHashCode());
    }
    
    public bool Equals(RoomItemData other)
    {
        return Id == other.Id && Name == other.Name && LeftTopCorner.Equals(other.LeftTopCorner) 
               && Color.Equals(other.Color) && IconName.Equals(other.IconName) && State.Equals(other.State)
               && Type == other.Type && RoomId == other.RoomId && Floor == other.Floor && RoomHashCode == other.RoomHashCode;
    }

    public override bool Equals(object? obj)
    {
        return obj is RoomItemData other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Id, Name, LeftTopCorner, Color, IconName, (int)State + Type, RoomId + Floor, RoomHashCode);
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