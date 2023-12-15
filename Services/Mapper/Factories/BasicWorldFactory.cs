using System;
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
            var physicsItemSubFactory = mapperContext.SubFactories.FirstOrDefault(x => x is IPhysicsItemSubFactory) as IPhysicsItemSubFactory;
            var mobSubFactory = mapperContext.SubFactories.FirstOrDefault(x => x is IMobSubFactory) as IMobSubFactory;
            
            foreach (var roomDto in worldDto.Rooms)
            {
                Room room = roomFactory.CreateRoom(roomDto, mapperContext.GameStorage);
                world.Rooms.Add(room);

                bool result = world.Map.FillWithInnerMap(room.LocalMap, roomDto.Floor, room.LeftTopCorner, out Map outMap);
                if (!result) throw new ArgumentException("Invalid intersection of room map");
                world.Map = outMap;
                
                if (physicsItemSubFactory != null)
                {
                    foreach (var physicsItem in roomDto.PhysicsItems
                                 .Select(physicsItemDto => physicsItemSubFactory.CreatePhysicsItem(physicsItemDto, room)))
                    {
                        room.PhysicsItems.Add(physicsItem);
                    }
                }
                if (mobSubFactory != null)
                {
                    foreach (var mob in roomDto.Mobs
                                 .Select(mobDto => mobSubFactory.CreateMob(mobDto, room)))
                    {
                        room.Mobs.Add(mob);
                    }
                }
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