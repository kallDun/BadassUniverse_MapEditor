using System;
using MapEditor.Models.Game;
using MapEditor.Models.Server;

namespace MapEditor.Services.Mapper.Factories;

public class BasicMobSubFactory : IMobSubFactory
{
    public Mob CreateMob(MobDTO mobDto, Room roomOwner)
    {
        return new Mob(mobDto.Id, mobDto.Name, 
            new MapIndex(mobDto.RoomOffsetY, mobDto.RoomOffsetX), 
            mobDto.Color, $"mob_{(int)mobDto.Icon}", mobDto.State, roomOwner);
    }
}