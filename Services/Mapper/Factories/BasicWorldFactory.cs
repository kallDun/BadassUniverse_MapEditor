﻿using System;
using System.Linq;
using MapEditor.Models.Game;
using MapEditor.Models.Server;

namespace MapEditor.Services.Mapper.Factories;

public class BasicWorldFactory : IWorldFactory
{
    public World CreateWorld(WorldDTO worldDto, WorldMapperContext mapperContext)
    {
        World world = new()
        {
            Name = worldDto.Name,
            Map = Map.InitMap(worldDto.YLenght, worldDto.XLenght)
        };

        if (mapperContext.SubFactories.FirstOrDefault(x => x is IRoomSubFactory) is IRoomSubFactory roomFactory)
        {
            foreach (var roomDto in worldDto.Rooms)
            {
                Room room = roomFactory.CreateRoom(roomDto, mapperContext.GameStorage);
                world.Rooms.Add(room);

                bool result = world.Map.FillWithInnerMap(room.LocalMap, roomDto.Floor, room.LeftTopCorner, out Map outMap);
                if (!result) throw new ArgumentException("Invalid intersection of room map");
                world.Map = outMap;
            }
        }

        if (mapperContext.SubFactories.FirstOrDefault(x => x is IFacadeSubFactory) is IFacadeSubFactory facadeFactory)
        {
            foreach (var facadeDto in worldDto.Facades)
            {
                Facade facade = facadeFactory.CreateFacade(facadeDto, mapperContext.GameStorage);
                world.Facades.Add(facade);

                bool result = world.Map.FillWithInnerMap(facade.LocalMap, facadeDto.Floor, facade.LeftTopCorner, out Map outMap);
                if (!result) throw new ArgumentException("Invalid intersection of facade map");
                world.Map = outMap;
            }
        }

        return world;
    }
}