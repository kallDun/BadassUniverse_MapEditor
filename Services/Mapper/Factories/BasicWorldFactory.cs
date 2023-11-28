using BadassUniverse_MapEditor.Models.Game;
using BadassUniverse_MapEditor.Models.Server;
using System;
using System.Linq;

namespace BadassUniverse_MapEditor.Services.Mapper.Factories
{
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

                    bool result = world.Map.FillWithInnerMap(room.LocalMap, roomDto.Floor, 
                        new MapIndex(roomDto.MapOffsetY, roomDto.MapOffsetX), out Map outMap);
                    if (!result) throw new ArgumentException("Invalid intersection of room map");
                    world.Map = outMap;
                }
            }

            /*if (mapperContext.SubFactories.FirstOrDefault(x => x is IFacadeSubFactory) is IFacadeSubFactory facadeFactory)
            {
                foreach (var facadeDto in mapDto.Facades)
                {
                    Facade facade = facadeFactory.CreateFacade(facadeDto, mapperContext.GameStorage);
                    world.Facades.Add(facade);

                    bool result = world.Map.FillWithInnerMap(facade.LocalMap, facadeDto.Floor,
                        new MapIndex(facadeDto.MapOffsetY, facadeDto.MapOffsetX), out Map outMap);
                    if (!result) throw new ArgumentException("Invalid intersection of facade map");
                    world.Map = outMap;
                }
            }*/

            return world;
        }
    }
}
